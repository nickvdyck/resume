using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.ObjectPool;
using Resume.Configuration;
using Resume.Services;
using Resume.Views.ViewModels;
using SelectPdf;

namespace Resume
{
    class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Starting ...");
            var serviceScopeFactory = InitializeServices();
            var page = await RenderViewAsync(serviceScopeFactory);

            Directory.CreateDirectory("./artifacts");

            using (var stream = new FileStream("./artifacts/resume.html", FileMode.Create))
            {
                var bytes = Encoding.UTF8.GetBytes(page);
                await stream.WriteAsync(bytes);
            }

            var converter = new HtmlToPdf();

            var document = converter.ConvertHtmlString(page);

            using (var stream = new FileStream("./artifacts/resume.pdf", FileMode.Create))
            {
                document.Save(stream);
                document.Close();
            }

            Console.WriteLine("Done...");
        }

        public static IServiceScopeFactory InitializeServices(string customApplicationBasePath = null)
        {
            // Initialize the necessary services
            var services = new ServiceCollection();
            ConfigureDefaultServices(services, customApplicationBasePath);


            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IServiceScopeFactory>();
        }

        public static ResumeViewModel ResumeToViewModel(IResumeClient resumeClient)
        {

            var resume = resumeClient.GetResume();

            var model = new ResumeViewModel()
            {
                Name = resume.Basics.Name,
                JobTitle = resume.Basics.Label,
                Picture = resume.Basics.Picture,
                AboutMe = resume.Basics.Summary.Split("\n\n").ToList(),
                WorkPlaces = resume.Work,
                ContactInfo = new List<ContactRecord>(),
                Schools = resume.Education,
                Languages = resume.Languages,
            };

            model.ContactInfo.Add(new ContactRecord()
            {
                Type = "Email",
                Data = resume.Basics.Email,
            });

            foreach (var profile in resume.Basics.Profiles)
            {
                model.ContactInfo.Add(new ContactRecord()
                {
                    Type = profile.Network,
                    Data = profile.Url,
                });
            }

            return model;

        }

        public static Task<string> RenderViewAsync(IServiceScopeFactory scopeFactory)
        {
            using (var serviceScope = scopeFactory.CreateScope())
            {
                var helper = serviceScope.ServiceProvider.GetRequiredService<RazorViewToStringRenderer>();
                var resumeClient = serviceScope.ServiceProvider.GetRequiredService<IResumeClient>();

                var model = ResumeToViewModel(resumeClient);

                return helper.RenderViewToStringAsync("Views/Resume_Default.cshtml", model);
            }
        }

        private static void ConfigureDefaultServices(IServiceCollection services, string customApplicationBasePath)
        {
            string applicationName;
            IFileProvider fileProvider;
            if (!string.IsNullOrEmpty(customApplicationBasePath))
            {
                applicationName = Path.GetFileName(customApplicationBasePath);
                fileProvider = new PhysicalFileProvider(customApplicationBasePath);
            }
            else
            {
                applicationName = Assembly.GetEntryAssembly().GetName().Name;
                fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            }

            services.AddSingleton<IFileProvider>(_ => fileProvider);

            services.AddSingleton<IHostingEnvironment>(new HostingEnvironment
            {
                ApplicationName = applicationName,
                WebRootFileProvider = fileProvider,
            });
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Clear();
                options.FileProviders.Add(fileProvider);
            });
            var diagnosticSource = new DiagnosticListener("Microsoft.AspNetCore");
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<DiagnosticSource>(diagnosticSource);
            services.AddSingleton<IResumeClient, ResumeFileClient>();

            services.Configure<ResumeClientOptions>(options =>
            {
                options.Location = "./resume.json";
            });

            services.AddLogging();
            services.AddMvc();
            services.AddTransient<RazorViewToStringRenderer>();
        }
    }
}

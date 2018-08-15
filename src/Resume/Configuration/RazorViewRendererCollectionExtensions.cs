using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.ObjectPool;
using Resume.Services;

namespace Resume.Configuration
{
    public static class RazorViewRendererCollectionExtensions
    {

        public static IServiceCollection AddRazorViewRenderer(this IServiceCollection services)
        {
            var fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

            services.AddSingleton<IHostingEnvironment>(new HostingEnvironment()
            {
                ApplicationName = Assembly.GetEntryAssembly().GetName().Name,
                WebRootFileProvider = fileProvider,
            });

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Add(fileProvider);
            });

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<DiagnosticSource>(_ => new DiagnosticListener("Microsoft.AspNetCore"));

            services.AddSingleton<RazorViewRenderer>();

            services.AddMvc();

            return services;
        }

    }
}

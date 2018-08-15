using System.IO;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Resume.Commands;
using Resume.Configuration;
using Resume.Services;

namespace Resume
{
    [Command("resume")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand("build", typeof(BuildCommand))]
    [Subcommand("validate", typeof(ValidateCommand))]
    class Program
    {
        [LegalFilePath]
        [Option(Description = "Root directory", ShortName = "r", Inherited=true)]
        public string Root { get; set; } = Directory.GetCurrentDirectory();

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFileProvider>(_ => new PhysicalFileProvider(Root));

            services.AddSingleton<IResumeClient, ResumeFileClient>();
            services.AddHttpClient<IResumeValidator, ResumeValidator>();

            services.AddRazorViewRenderer();
        }

        public int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 1;
        }

        public static int Main(string[] args) => ConsoleApplicationHost.Run<Program>(args);

        private static string GetVersion()
            => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}

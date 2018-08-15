using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Resume
{
    public class ConsoleApplicationHost
    {
        public static int Run<T>(string[] args) where T : class
        {
            var services = new ServiceCollection()
                .AddSingleton<IConsole>(PhysicalConsole.Singleton);

            var app = new CommandLineApplication<T>();

            var model = app.ModelFactory();
            var methodInfo = model.GetType().GetMethod("ConfigureServices");

            if (methodInfo != null)
                methodInfo.Invoke(model, new object[] { services });

            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(services.BuildServiceProvider());

            return app.Execute(args);
        }
    }
}

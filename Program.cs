using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using VEEAM1;
using VEEAM1.Services;

internal class Program
{




    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<ArgsOptions>(args)
            .WithParsed<ArgsOptions>(options =>
            {
                IServiceProvider serviceProvider = BuildServiceProvider(options);

                AppBuilder appBuilder = serviceProvider.GetService<AppBuilder>();

                appBuilder.Start();
            });
    }


    static IServiceProvider BuildServiceProvider(ArgsOptions options)
    {
        IServiceCollection collection = new ServiceCollection();

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsetings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();


        IConfig config = configuration.Get<Config>();
        
        
        Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(configuration)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File(options.LoggerPath, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
           .CreateLogger();

       
        collection.AddSingleton<IArgsOptions>(options);

        collection.AddSingleton<AppBuilder, AppBuilder>();

        collection.AddSingleton<IConfig>(config);

        collection.AddSingleton<IFileComparer, FileComparer>();
        collection.AddSingleton<ICopyDirectory, CopyDirectory>();


        return collection.BuildServiceProvider();
    }
}

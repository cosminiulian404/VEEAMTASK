using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Timers;

namespace VEEAM1
{
    internal class AppBuilder
    {
        private static string? _sourceDirectory;
        private static string? _targetDirectory;
        private static string? _loggerPath;
        private System.Timers.Timer _aTimer;
        private readonly IFileComparer _fileComparer;
        private ICopyDirectory _copyDirectory;



        public AppBuilder(string sourceDirectory, string targetDirectory, string loggerPath, int time)
        {
            _loggerPath = loggerPath;
            _aTimer = new System.Timers.Timer(time);
            _sourceDirectory = sourceDirectory;
            _targetDirectory = targetDirectory;
            _fileComparer = new FileComparer();
            _copyDirectory = new CopyDirectory();
           

        }

        public void Starting()
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);


            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(_loggerPath!, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .CreateLogger();
            Log.Logger.Information("Application Starting");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                })
                .UseSerilog()
                .Build();
            SetTimer();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsetings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }



        private void SetTimer()
        {

            _aTimer.Elapsed += OnTimeEvent;

            _aTimer.AutoReset = true;
            _aTimer.Enabled = true;
            _aTimer.Start();
            Console.ReadLine();

        }

        private void OnTimeEvent(object? sender, ElapsedEventArgs e)
        {
            if (Console.KeyAvailable)
            {
                string userInput = Console.ReadLine();
                if (userInput.ToLower() == "x")
                {
                    Log.Logger.Information("Application exiting gracefully.");

                    _aTimer.Stop(); // Stop the timer
                    // Perform any necessary cleanup here
                    return; // Exit the method
                }
            }
            _fileComparer.Compare(_sourceDirectory!, _targetDirectory!);
            _copyDirectory.CopyAll(_sourceDirectory!, _targetDirectory!);


        }




    }
}

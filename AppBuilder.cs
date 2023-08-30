using Microsoft.Extensions.Options;
using Serilog;
using System.Timers;

namespace VEEAM1
{
    public class AppBuilder
    {
        private static string? _sourceDirectory;
        private static string? _targetDirectory;
        private System.Timers.Timer _aTimer;
        private readonly IFileComparer _fileComparer;
        private readonly ICopyDirectory _copyDirectory;



        public AppBuilder(IArgsOptions options, IFileComparer fileComparer, ICopyDirectory copyDirectory)
        {
            _aTimer = new System.Timers.Timer(options.Periodicity);
            _sourceDirectory = options.SourceDirectory;
            _targetDirectory = options.TargetDirectory;


            _fileComparer = fileComparer;
            _copyDirectory = copyDirectory;
            
            
            _aTimer.Elapsed += OnTimeEvent;
            _aTimer.AutoReset = true;
            _aTimer.Enabled = true;


        }

        public void Start()
        {
            Log.Logger.Information("Application Starting");
            _aTimer.Start();
            Console.ReadLine();
        }

        private void OnTimeEvent(object? sender, ElapsedEventArgs e)
        {

            _fileComparer.Compare(_sourceDirectory!, _targetDirectory!);
            _copyDirectory.CopyAll(_sourceDirectory!, _targetDirectory!);
        }

    }

}

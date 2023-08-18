using System.Timers;

namespace VEEAM1
{
    internal class AppBuilder
    {
        private static string? _sourceDirectory;
        private static string? _targetDirectory;
        private static string? _loggerPath;
        private System.Timers.Timer _aTimer;

        public AppBuilder(string sourceDirectory, string targetDirectory, string loggerPath, int time)
        {
            _loggerPath = loggerPath;
            _aTimer = new System.Timers.Timer(time);
            _sourceDirectory = sourceDirectory;
            _targetDirectory = targetDirectory;
        }

        public void Start()
        {
            SetTimer();
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
            CopyDir copyDir = new CopyDir(_sourceDirectory!, _targetDirectory!);
            Comparer comparer = new Comparer();
            comparer.compare(_sourceDirectory!, _targetDirectory!, _loggerPath!);
           
        }

        


    }
}

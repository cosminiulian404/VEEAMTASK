namespace VEEAM1.Services
{

    public interface IConfig
    {
       public string SourceDirectory { get; }
       public string TargetDirectory { get; }
       public string LoggerPath { get; }
    }

    public class Config : IConfig
    {
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
        public string LoggerPath { get; set; }

    }

}

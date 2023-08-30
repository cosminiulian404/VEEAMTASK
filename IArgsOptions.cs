using CommandLine;

public interface IArgsOptions
{
    string LoggerPath { get; }
    int Periodicity { get; }
    string SourceDirectory { get; }
    string TargetDirectory { get; }
}

public class ArgsOptions : IArgsOptions
{
    [Option('s', "source", Required = false, Default = "d:\\newSource", HelpText = "The source directory you want to copy.")]
    public string SourceDirectory { get; set; }
    [Option('t', "target", Required = false, Default = "d:\\targetFile", HelpText = "The target directory you want to copy in.")]
    public string TargetDirectory { get; set; }
    [Option('p', "time", Required = true, Default = 1000, HelpText = "The time period you want the directory to be copied.")]
    public int Periodicity { get; set; }
    [Option('l', "loggerFile", Required = false, Default = "d:\\Serilog.txt", HelpText = "The path in the file you want to log.")]
    public string LoggerPath { get; set; }
}
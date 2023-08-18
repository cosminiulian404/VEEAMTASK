namespace VEEAM1
{
    public interface ICopyDirectory
    {
        void CopyAll(string sourceDirectory, string targetDirectory);
    }
}
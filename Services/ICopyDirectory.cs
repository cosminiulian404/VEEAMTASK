namespace VEEAM1
{
    public interface ICopyDirectory
    {
        public void CopyAll(string sourceDirectory, string targetDirectory);
    }
}
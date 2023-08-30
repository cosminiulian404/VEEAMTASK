namespace VEEAM1
{
    public interface IFileComparer
    {
        void Compare(string source, string target);
    }
}
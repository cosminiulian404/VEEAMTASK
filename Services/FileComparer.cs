using Serilog;

namespace VEEAM1
{
    internal class FileComparer : IFileComparer
    {



        public void Compare(string source, string target)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(source);
            DirectoryInfo targetDirectory = new DirectoryInfo(target);

            // Take a snapshot of the file system.  
            IEnumerable<FileInfo> sourceList = sourceDirectory.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<FileInfo> targetList = targetDirectory.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            FileCompare myFileCompare = new FileCompare();

            IEnumerable<FileInfo> queryCopyed = sourceList.Intersect(targetList, myFileCompare);

            if (queryCopyed.Any())
            {

                foreach (var v in queryCopyed)
                {
                    Log.Logger.Information($"{DateTime.Now} Copying: {v.FullName}");
                    //shows which items end up in result list  

                }
            }
            else
            {
                Log.Logger.Warning("There is no common files in the two folders.");
            }

            var queryCreatedFiles = sourceList.Except(targetList, myFileCompare);


            foreach (var v in queryCreatedFiles)
            {
                Log.Logger.Warning($"{DateTime.Now} Created: {v.FullName}");
            }


            var queryDeletedFiles = targetList.Except(sourceList, myFileCompare);



            foreach (var v in queryDeletedFiles)
            {
                Log.Logger.Warning($"Deleted: {v.Name}");

            }

            foreach (var v in queryDeletedFiles)
            {
                v.Delete();
            }

        }

        class FileCompare : System.Collections.Generic.IEqualityComparer<FileInfo>
        {
            public FileCompare() { }

            public bool Equals(FileInfo? f1, FileInfo? f2)
            {
                return (f1!.Name == f2!.Name) && (f1.Length == f2.Length);
            }

            // Return a hash that reflects the comparison criteria. According to the
            // rules for IEqualityComparer<T>, if Equals is true, then the hash codes must  
            // also be equal. Because equality as defined here is a simple value equality, not  
            // reference identity, it is possible that two or more objects will produce the same  
            // hash code.  
            public int GetHashCode(FileInfo fi)
            {
                string s = $"{fi.Name}{fi.Length}";
                return s.GetHashCode();
            }
        }

    }

}

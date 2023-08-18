using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEEAM1
{
    internal class Comparer
    {
        public void compare(string source, string target, string loggerPath)
        {


            System.IO.DirectoryInfo sourceDirectory = new System.IO.DirectoryInfo(source);
            System.IO.DirectoryInfo targetDirectory = new System.IO.DirectoryInfo(target);

            // Take a snapshot of the file system.  
            IEnumerable<System.IO.FileInfo> sourceList = sourceDirectory.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> targetList = targetDirectory.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            FileCompare myFileCompare = new FileCompare();


            using (StreamWriter outputFile = new StreamWriter(loggerPath, true))
            {

                var queryCopyed = sourceList.Intersect(targetList, myFileCompare);

                if (queryCopyed.Any())
                {
                    
                    foreach (var v in queryCopyed)
                    {
                        outputFile.WriteLine($"{DateTime.Now} Copying: {v.FullName}");
                        Console.WriteLine($"{DateTime.Now} Copying: {v.FullName}");//shows which items end up in result list  

                    }
                }
                else
                {
                    Console.WriteLine("There are no common files in the two folders.");
                }

                var queryCreatedFiles = (from file in sourceList
                                         select file).Except(targetList, myFileCompare);

                Console.WriteLine($"The following files have been created in {source}:");
                foreach (var v in queryCreatedFiles)
                {
                    outputFile.WriteLine($"{DateTime.Now} Created: {v.FullName}");
                    Console.WriteLine($"{DateTime.Now} Created: {v.FullName}");
                }


                var queryDeletedFiles = (from file in targetList
                                         select file).Except(sourceList, myFileCompare);


               
                foreach (var v in queryDeletedFiles)
                {
                    outputFile.WriteLine($"{DateTime.Now} Deleted: {v.FullName}");
                    Console.WriteLine($"{DateTime.Now} Deleted: {v.FullName}");

                }

                foreach (var v in queryDeletedFiles)
                {
                    v.Delete();
                }
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

    }
    class FileCompare : System.Collections.Generic.IEqualityComparer<System.IO.FileInfo>
    {
        public FileCompare() { }

        public bool Equals(System.IO.FileInfo f1, System.IO.FileInfo f2)
        {
            return (f1.Name == f2.Name &&
                    f1.Length == f2.Length);
        }

        // Return a hash that reflects the comparison criteria. According to the
        // rules for IEqualityComparer<T>, if Equals is true, then the hash codes must  
        // also be equal. Because equality as defined here is a simple value equality, not  
        // reference identity, it is possible that two or more objects will produce the same  
        // hash code.  
        public int GetHashCode(System.IO.FileInfo fi)
        {
            string s = $"{fi.Name}{fi.Length}";
            return s.GetHashCode();
        }
    }
}

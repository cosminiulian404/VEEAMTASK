using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEEAM1
{
    public class CopyDir
    {
        private static DirectoryInfo? _source;
        private static DirectoryInfo? _target;
        public CopyDir(string source, string target)
        {
            _source = new DirectoryInfo(source);
            _target = new DirectoryInfo(target);
            CopyAll(_source, _target);

        }




        public void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {

            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }




            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {

                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }


            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }

        }
    }


}

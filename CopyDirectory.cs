namespace VEEAM1
{
    internal class CopyDirectory : ICopyDirectory
    {


        public void CopyAll(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo source = new DirectoryInfo(sourceDirectory);
            DirectoryInfo target = new DirectoryInfo(targetDirectory);


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
                CopyAll(diSourceSubDir.FullName, nextTargetSubDir.FullName);
            }

        }
    }


}

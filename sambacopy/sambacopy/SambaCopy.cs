using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sambacopy
{
    public class SambaCopy
    {
        public string SourceFolder { get; set; }
        public string TargetFolder { get; set; }

        public enum Verbose { None, Medium, High };

        public Verbose Verbosity { get; set; }

        public List<string> ExcludeList { get; set; } 

        public void CopyAllFiles()
        {
            CopyFiles(SourceFolder, TargetFolder);

            string[] directories = Directory.GetDirectories(SourceFolder, "*", SearchOption.AllDirectories);
            foreach (string directory in directories)
            {
                string targetFolder = String.Format("{0}{1}{2}", TargetFolder, Path.DirectorySeparatorChar, directory.Replace(SourceFolder, ""));
                CopyFiles(directory, targetFolder);
            }
        }

        private void CopyFiles(string sourceFolder, string destinationFolder)
        {
            foreach (string file in Directory.GetFiles(sourceFolder, "*", SearchOption.TopDirectoryOnly))
            {
                Directory.CreateDirectory(destinationFolder);
                string targetFile = Path.Combine(destinationFolder, Path.GetFileName(file));
                bool doCopy = CheckFileModification(file, targetFile);
                if (doCopy)
                {
                    try
                    {
                        if (!ExcludeList.Contains(Path.GetFileName(file)))
                        {
                            File.Copy(file, targetFile, true);
                            if (Verbosity != Verbose.None)
                                Console.WriteLine("[OK] Copied " + targetFile);
                        }
                        else
                        {
                            if (Verbosity == Verbose.High)
                                Console.WriteLine("Excluded: " + file);    
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("!!! Cannot copy '" + file + "' due to bad weather and " + ex.Message);
                    }
                }
                else
                {
                    if (Verbosity == Verbose.High)
                        Console.WriteLine("...Skipped: " + file);                        
                }
            }
        }

        public bool CheckFileModification(string file, string targetFile)
        {
            bool doCopy = true;

            if (File.Exists(targetFile))
            {
                // compare timestamp - samba does not include the millisecond field / even seconds are not relevant from a backup perspective
                FileInfo targetPath = new FileInfo(targetFile);
                FileInfo filePath = new FileInfo(file);

                LimitedDate limitedDate = new LimitedDate();
                int compare = limitedDate.Compare(filePath.LastWriteTimeUtc, targetPath.LastWriteTimeUtc);
                if (compare > 0)
                {
                    doCopy = true;
                    File.SetAttributes(targetFile, FileAttributes.Normal);
                }
                else
                {
                    doCopy = false;
                }
            }

            return doCopy;
        }

        // reference: http://www.csharp411.com/c-copy-folder-recursively/
        public void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                Console.WriteLine("Checking folder " + dir.FullName);
                DirectoryInfo targetSubDirectory = target.CreateSubdirectory(dir.Name);
                CopyFilesRecursively(dir, targetSubDirectory);
            }
            try
            {
                Parallel.ForEach(source.GetFiles(),
                filePath =>
                {
                    System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        bool doCopy = true;

                        string targetFile = Path.Combine(target.FullName, filePath.Name);

                        if (File.Exists(targetFile))
                        {
                            // compare timestamp - samba does not include the millisecond field / even seconds are not relevant from a backup perspective
                            FileInfo targetPath = new FileInfo(targetFile);

                            LimitedDate limitedDate = new LimitedDate();
                            int compare = limitedDate.Compare(filePath.LastWriteTimeUtc, targetPath.LastWriteTimeUtc);
                            if (compare > 0)
                            {
                                doCopy = true;
                                File.SetAttributes(targetFile, FileAttributes.Normal);
                            }
                            else
                            {
                                doCopy = false;
                            }
                        }

                        if (doCopy)
                        {
                            filePath.CopyTo(targetFile, true);
                            lock (Console.Out)
                            {
                                Console.WriteLine("Copied " + targetFile);
                            }
                        }
                        else
                        {
                            lock (Console.Out)
                            {
                                Console.WriteLine("Skipped " + targetFile);
                            }
                        }

                    });
                });
            }
            catch (Exception ex)
            {
                lock (Console.Out)
                {
                    Console.WriteLine("Fatal Exception Error " + ex.Message);
                }
                throw;
            }

        }

        public class LimitedDate : Comparer<DateTime>
        {
            // Compares by Length, Height, and Width. 
            public override int Compare(DateTime x, DateTime y)
            {
                if (x.Year.CompareTo(y.Year) != 0)
                {
                    return x.Year.CompareTo(y.Year);
                }
                else if (x.Month.CompareTo(y.Month) != 0)
                {
                    return x.Month.CompareTo(y.Month);
                }
                else if (x.Day.CompareTo(y.Day) != 0)
                {
                    return x.Day.CompareTo(y.Day);
                }
                else if (x.Hour.CompareTo(y.Hour) != 0)
                {
                    return x.Hour.CompareTo(y.Hour);
                }
                else if (x.Minute.CompareTo(y.Minute) != 0)
                {
                    return x.Minute.CompareTo(y.Minute);
                }
                else
                {
                    return 0;
                }
            }

        }

    }
}

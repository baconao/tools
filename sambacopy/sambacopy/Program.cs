using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sambacopy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString() + " - Executing File Copy...");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            //DirectoryInfo source = new DirectoryInfo(args[0]);
            //DirectoryInfo target = new DirectoryInfo(args[1]);
            //CopyFilesRecursively(source, target);
            
            CopyAllFiles(args[0], args[1]);

            stopwatch.Stop();

            Console.WriteLine(DateTime.Now.ToLongTimeString() + " File Copy time in milliseconds: {0}", stopwatch.ElapsedMilliseconds);

#if DEBUG
            Console.ReadKey();
#endif

        }


        public static void CopyAllFiles(string sourceFolder, string destinationFolder)
        {
            CopyFiles(sourceFolder, destinationFolder);

            string[] directories = Directory.GetDirectories(sourceFolder, "*", SearchOption.AllDirectories);
            foreach (string directory in directories)
            {
                string targetFolder = String.Format("{0}{1}{2}", destinationFolder, Path.DirectorySeparatorChar, directory.Replace(sourceFolder, ""));
                CopyFiles(directory, targetFolder);
            }
        }

        private static void CopyFiles(string sourceFolder, string destinationFolder)
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
                        File.Copy(file, targetFile, true);
                        Console.WriteLine("[OK] Copied " + targetFile);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("!!! Cannot copy " + targetFile + " due to bad weather and " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("...Skipped " + targetFile);
                }
            }
        }

        private static bool CheckFileModification(string file, string targetFile)
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
        private static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static sambacopy.SambaCopy;

namespace sambacopy
{
    class FolderItem
    {
        public string folderPath { get; }

        public FolderItem(string directoryPath)
        {
            folderPath = directoryPath;
        }

        public void Copy(string sourceFolderBase, string targetFolderBase,
            VerboseLevel verbosity, List<string> excludeList)
        {
            string targetPath = string.Format("{0}{1}", 
                targetFolderBase, folderPath.Replace(sourceFolderBase, ""));
            CopyFilesInDirectory(folderPath, targetPath, verbosity, excludeList);
        }

        private void CopyFilesInDirectory(string sourceFolder, string destinationFolder,
            VerboseLevel verbosity, List<string> excludeList)
        {

            List<string> sourceFiles = Directory.GetFiles(sourceFolder, "*", SearchOption.TopDirectoryOnly)
                .ToList<string>();

            if (!CleanupFolder(destinationFolder, sourceFiles, verbosity))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            Parallel.ForEach(sourceFiles, (file) =>
            {
                string targetFile = Path.Combine(destinationFolder, Path.GetFileName(file));
                bool doCopy = CheckFileModification(file, targetFile);
                if (doCopy)
                {
                    try
                    {
                        if (!excludeList.Contains(Path.GetFileName(file)))
                        {
                            File.Copy(file, targetFile, true);
                            if (verbosity != VerboseLevel.None)
                                Console.WriteLine("[OK] Copied " + targetFile);
                        }
                        else
                        {
                            if (verbosity == VerboseLevel.High)
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
                    if (verbosity == VerboseLevel.High)
                        Console.WriteLine("...Skipped: " + file);
                }
            });
            

        }

        private bool CleanupFolder(string destinationFolder, List<string> sourceFiles, VerboseLevel verbosity)
        {
            if (!Directory.Exists(destinationFolder))
                return false; // no need cleanup

            try
            {
                // cleanup                
                var targetFileNames = from tf in Directory.GetFiles(destinationFolder, "*", SearchOption.TopDirectoryOnly)
                                      select Path.GetFileName(tf);
                var sourceFileNames = from sf in sourceFiles
                                      select Path.GetFileName(sf);

                var filesToDelete = targetFileNames.Except(sourceFileNames);

                Parallel.ForEach(filesToDelete, (fileToDelete) =>
                {
                    try
                    {
                        if (verbosity == VerboseLevel.High)
                            Console.WriteLine($"Deleting: '{fileToDelete}'");

                        File.Delete(Path.Combine(destinationFolder, fileToDelete));
                    }
                    catch (Exception ex)
                    {
                        if (verbosity != VerboseLevel.None)
                            Console.WriteLine($"Error on cleanup. Cannot delete file '{fileToDelete}' due to:  {ex.Message}");
                    }
                });

            }
            catch (Exception ex)
            {
                if (verbosity != VerboseLevel.None)
                    Console.WriteLine("Error on cleanup. Cannot delete file(s) on '" + destinationFolder + "' due to: " + ex.Message);
            }

            return true;
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

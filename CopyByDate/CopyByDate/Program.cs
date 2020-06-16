using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Threading;

namespace CopyByDate
{
    class Program
    {
        public static string RootPath { get; set; }

        static void Main(string[] args)
        {
            RootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // set threadpool
            ThreadPool.GetMinThreads(out int minThreads, out int minPortThreads);
            ThreadPool.SetMaxThreads(minThreads * 2, minPortThreads * 2);

            foreach (string file in Directory.GetFiles(RootPath, "*.*", SearchOption.TopDirectoryOnly))
            {
                if (String.Compare(file, Assembly.GetExecutingAssembly().Location, true) == 0)
                {
                    continue;
                }

                ThreadPool.QueueUserWorkItem(new WaitCallback(CopyFile), file);
            }

            WaitForAllThreadsToComplete();

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();

        }

        private static void WaitForAllThreadsToComplete()
        {
            ThreadPool.GetMaxThreads(out int maxThreads, out _);
            int availThreads;
            do
            {
                Thread.Sleep(500);
                ThreadPool.GetAvailableThreads(out availThreads, out _);

            } while (availThreads < maxThreads);

        }

        private static void CopyFile(object fileParam)
        {

            DateTime time;
            string file = fileParam as string;

            string fileName = Path.GetFileNameWithoutExtension(file);

            try
            {

                string prefix = "";

                if (fileName.IndexOf("_") == 8)
                {
                    prefix = fileName.Substring(0, fileName.IndexOf("_"));
                    prefix = String.Format("{0}-{1}-{2}", prefix.Substring(0, 4), prefix.Substring(4, 2), prefix.Substring(6, 2));
                }

                if (!DateTime.TryParse(prefix, out time))
                {
                    time = File.GetCreationTime(file);
                }

                string folderName = RootPath + "\\" + time.Year.ToString() + "_" + time.Month.ToString() + "_" + time.Day.ToString();

                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }

                Console.WriteLine($"Moving: '{file}' to Folder: '{folderName}'");
                
                string targetFile = Path.Combine(folderName, Path.GetFileName(file));
                if (File.Exists(targetFile))
                    try
                    {
                        File.Delete(targetFile);
                    }
                    catch { }
                File.Move(file, targetFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot move: " + file + " due to: " + ex.Message);
            }
        }
    }
}

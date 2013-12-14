using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace CopyByDate
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach (string file in Directory.GetFiles(rootPath, "*.*", SearchOption.TopDirectoryOnly))
            {
                if (String.Compare(file, Assembly.GetExecutingAssembly().Location, true) == 0)
                {
                    continue;
                }
                
                DateTime time = File.GetCreationTime(file);
                
                string folderName = rootPath + "\\" + time.Year.ToString() + "_" + time.Month.ToString() + "_" + time.Day.ToString();

                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }

                try
                {
                    File.Move(file, Path.Combine(folderName, Path.GetFileName(file)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cannot move: " + file + " due to: " + ex.Message);
                }
            }
        }
    }
}

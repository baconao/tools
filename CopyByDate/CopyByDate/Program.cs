using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Globalization;

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

                DateTime time;

                string fileName = Path.GetFileNameWithoutExtension(file);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace theReplacer
{
    static public class FileUtils
    {
        static public ArrayList GetFileList(string path, string searchPattern)
        {
            ArrayList result = new ArrayList();

            string delimStr = " ,;";
            char[] delimiter = delimStr.ToCharArray();

            string[] filters = searchPattern.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in filters)
            {
                string [] ret = Directory.GetFiles(path, item, SearchOption.AllDirectories);

                result.AddRange(ret);

            }

            return result;
            
        }
    }
}

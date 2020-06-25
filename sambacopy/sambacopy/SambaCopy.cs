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
        
        public enum VerboseLevel { None, Medium, High };

        public VerboseLevel Verbosity { get; set; }
        public string SourceFolder { get; set; }
        public string TargetFolder { get; set; }
        public List<string> ExcludeList { get; set; }

        public void CopyAllFiles()
        {
            List<string> directories = new List<string>() { SourceFolder };
            directories.AddRange(Directory.GetDirectories(SourceFolder, "*", SearchOption.AllDirectories));
            directories.AsParallel().ForAll((directory) =>
            {
                new FolderItem(directory).Copy(SourceFolder, TargetFolder, Verbosity, ExcludeList);                
            });            
        }        

    }
}

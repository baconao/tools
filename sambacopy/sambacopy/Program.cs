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

            SambaCopy sambaCopy = new SambaCopy() { SourceFolder = args[0], TargetFolder = args[1] };
            sambaCopy.Verbosity = SambaCopy.Verbose.Medium;

            if (args.Length > 2)
            {
                for (int i = 2; i < args.Length ; i++)
                {
                    string flag = args[i].Substring(0, 3);
                    switch (flag)
                    {
                        case "-v1":
                            sambaCopy.Verbosity = SambaCopy.Verbose.Medium;
                            break;
                        case "-v0":
                            sambaCopy.Verbosity = SambaCopy.Verbose.None;
                            break;
                        case "-v2":
                            sambaCopy.Verbosity = SambaCopy.Verbose.High;
                            break;
                        case "-x:":
                            sambaCopy.ExcludeList = args[i].Substring(3).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                            break;
                        default:
                            break;
                    }
                }
            }

            sambaCopy.CopyAllFiles();

            stopwatch.Stop();

            Console.WriteLine(DateTime.Now.ToLongTimeString() + " File Copy time in milliseconds: {0}", stopwatch.ElapsedMilliseconds);

        }


        
    }
}

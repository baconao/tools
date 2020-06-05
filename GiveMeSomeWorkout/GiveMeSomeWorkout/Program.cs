using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveMeSomeWorkout
{
    class Program
    {
        static void Main(string[] args)
        {

            var workout = new Workout();
            
            string json = JsonConvert.SerializeObject(workout);
            string jsonFormatted = JValue.Parse(json).ToString(Formatting.Indented);

            Console.WriteLine(jsonFormatted);

        }
    }
}

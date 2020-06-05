using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveMeSomeWorkout.Utils
{
    class Randomizer
    {
        Random rnd = null;

        public Randomizer()
        {
            rnd = new Random();
        }

        public int GiveMeANumber(int Min, int Max)
        {
            return rnd.Next(Min, Max);
        }

        public int GiveMeANumber(int Max)
        {
            return rnd.Next(Max);
        }
    }
}

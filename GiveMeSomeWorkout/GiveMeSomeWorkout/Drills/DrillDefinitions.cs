using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveMeSomeWorkout
{
    class DrillDefinition
    {

        public enum DrillUnits { Reps, Mins, Secs }

        public string Name { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }
        public DrillUnits Units { get; set; }
        public WorkoutBase.Types TargetSection { get; set; }
    }
}

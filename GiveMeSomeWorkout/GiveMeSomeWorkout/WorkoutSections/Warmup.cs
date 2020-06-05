using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveMeSomeWorkout.WorkoutSections
{
    class Warmup: WorkoutBase
    {
        public Warmup() : base(Types.Warmup, 2, 4, Styles.AllForTime | Styles.OnTheMinute)
        {

        }
    }
}

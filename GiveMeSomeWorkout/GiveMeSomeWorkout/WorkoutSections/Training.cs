using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveMeSomeWorkout.WorkoutSections
{
    class Training : WorkoutBase
    {
        public Training() : base(Types.Training, 3, 5, Styles.MaxRepsOnTime | Styles.AllForTime)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveMeSomeWorkout.WorkoutSections
{
    class Cooldown : WorkoutBase
    {
        public Cooldown() : base(Types.Cooldown, 2, 4, Styles.Tabata | Styles.AllForTime)
        {

        }
    }
}

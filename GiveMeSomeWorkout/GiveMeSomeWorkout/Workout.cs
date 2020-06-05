using GiveMeSomeWorkout.WorkoutSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveMeSomeWorkout
{
    class Workout
    {
        public Warmup Warmup { get; set; }        
        public Training Training { get; set; }
        public Cooldown Cooldown { get; set; }
        public Workout()
        {
            this.Warmup = new Warmup();
            this.Training = new Training();
            this.Cooldown = new Cooldown();
        }

    }
}

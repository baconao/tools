using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveMeSomeWorkout
{
    class DrillMenu
    {
        public List<DrillDefinition> FullTrainingMenu { get; set; }
        
        public DrillMenu()
        {
            FullTrainingMenu = new List<DrillDefinition>()
            {
                new DrillDefinition() { Name = "Pushups", Min = 5, Max = 20, Units = DrillDefinition.DrillUnits.Reps,
                    TargetSection = WorkoutBase.Types.Warmup | WorkoutBase.Types.Training },
                new DrillDefinition() { Name = "Hand Release Pushups", Min = 5, Max = 20 , Units = DrillDefinition.DrillUnits.Reps,
                    TargetSection = WorkoutBase.Types.Training },
                new DrillDefinition() { Name = "Air Squats", Min = 10, Max = 30, Units = DrillDefinition.DrillUnits.Reps,
                    TargetSection = WorkoutBase.Types.Warmup | WorkoutBase.Types.Training },
                new DrillDefinition() { Name = "Jump Squats", Min = 10, Max = 30 , Units = DrillDefinition.DrillUnits.Reps,
                    TargetSection = WorkoutBase.Types.Warmup | WorkoutBase.Types.Training },
                new DrillDefinition() { Name = "Lunges", Min = 10, Max = 20, Units = DrillDefinition.DrillUnits.Reps,
                    TargetSection = WorkoutBase.Types.Warmup | WorkoutBase.Types.Training },
                new DrillDefinition() { Name = "Rope Jump", Min = 50, Max = 100, Units = DrillDefinition.DrillUnits.Reps,
                    TargetSection = WorkoutBase.Types.Training },
                new DrillDefinition() { Name = "Plank", Min = 30, Max = 60, Units = DrillDefinition.DrillUnits.Secs,
                    TargetSection = WorkoutBase.Types.Cooldown | WorkoutBase.Types.Warmup },
                new DrillDefinition() { Name = "Side Plank", Min = 30, Max = 60, Units = DrillDefinition.DrillUnits.Secs,
                    TargetSection = WorkoutBase.Types.Warmup | WorkoutBase.Types.Cooldown },
                new DrillDefinition() { Name = "Hollow", Min = 10, Max = 30, Units = DrillDefinition.DrillUnits.Secs,
                    TargetSection = WorkoutBase.Types.Warmup | WorkoutBase.Types.Training },
                new DrillDefinition() { Name = "Superman", Min = 10, Max = 30 , Units = DrillDefinition.DrillUnits.Secs,
                    TargetSection = WorkoutBase.Types.Warmup | WorkoutBase.Types.Cooldown },
                new DrillDefinition() { Name = "Scalp Pushups", Min = 50, Max = 100 , Units = DrillDefinition.DrillUnits.Reps,
                    TargetSection = WorkoutBase.Types.Warmup }
            };
        }
    }
}

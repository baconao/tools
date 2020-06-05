using GiveMeSomeWorkout.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveMeSomeWorkout
{
    class WorkoutBase
    {
        [Flags]
        public enum Types { Warmup = 1, Cooldown = 2, Training = 4}
        [Flags]
        public enum Styles { OnTheMinute = 1, MaxRepsOnTime = 2, AllForTime = 4, Tabata = 8}
        public string Style { get { return _style.ToString(); } }
        public List<Drill> Drills { get; set; }
        
        private Types _sectionType;
        private int _minDrills;
        private int _maxDrills;
        private Styles _style;

        public WorkoutBase(Types workoutSectionType, int MinDrills, int MaxDrills, Styles WorkoutStyles)
        {
            _sectionType = workoutSectionType;
            _minDrills = MinDrills;
            _maxDrills = MaxDrills;
            _style = ChooseStyle(WorkoutStyles);
            Drills = GetDrillsFromMenu(new DrillMenu());
        }

        private Styles ChooseStyle(Styles WorkoutStyles)
        {
            var matchingStyles = Enum.GetValues(typeof(Styles)).Cast<Styles>().Where(s => (s & WorkoutStyles) == s).ToArray();
            Styles choosenStyle = matchingStyles[new Randomizer().GiveMeANumber(matchingStyles.Length)];
            return choosenStyle;
        }

        private List<Drill> GetDrillsFromMenu(DrillMenu drillMenu)
        {
            var rnd = new Randomizer();

            var drills = new List<Drill>();
            var suitableDrills = drillMenu.FullTrainingMenu.Where(t => (t.TargetSection & _sectionType) == _sectionType).ToArray();

            var numberOfDrillsForWorkout = rnd.GiveMeANumber(_minDrills, _maxDrills);
            for (int i = 0; i < numberOfDrillsForWorkout; i++)
            {
                DrillDefinition choosenOne;
                do
                {
                    choosenOne = suitableDrills[rnd.GiveMeANumber(0, suitableDrills.Length)];

                } while (drills.Any(d => d.Name == choosenOne.Name));
                    
                drills.Add(new Drill()
                {
                    Name = choosenOne.Name,
                    Reps = rnd.GiveMeANumber(choosenOne.Min, choosenOne.Max)
                });
            }
            
            return drills;
        }
    }
}

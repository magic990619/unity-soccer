using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.FootballGameEngine_Indie.Scripts.Utilities.Objects
{
    public class Range
    {
        public float Max { get; set; }

        public float Min { get; set; }

        public Range(float min, float max)
        {
            Max = max;
            Min = min;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Utilities
{
    static class Maths
    {
        public static double Clamp(double value, double min, double max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        public static uint SafeAddSubtract(uint value, uint modifier, bool is_subtract = false)
        {
            double mod = is_subtract ? -((double)modifier) : (double)modifier;
            return (uint)Clamp((double)value + mod, uint.MinValue, uint.MaxValue);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ACE.Utilities.Maths;

namespace ACE.Containers
{
    public class XP
    {
        public uint Value { get; private set; }

        public XP(uint value = 0)
        {
            Value = value;
        }

        public void Add(uint value)
        {
            Value = SafeAddSubtract(Value, value, false);
        }

        public void Subtract(uint value)
        {
            Value = SafeAddSubtract(Value, value, true);
        }

        public void Set(uint value)
        {
            Value = value;
        }

        public void Reset()
        {
            Set(0);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ACE.Sounds
{
    public class SoundHandler
    {
        public enum ID : short
        {
            //add new IDs
        }

        private readonly Dictionary<ID, string> _paths = new Dictionary<ID, string>()
        {
            //add path to new IDs
        };

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Shouldn't need to change anything below here ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public SoundHandler()
        {
            //TODO
        }
    }
}

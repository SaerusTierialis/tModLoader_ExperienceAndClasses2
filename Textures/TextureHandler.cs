using EAC2.Containers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace EAC2.Textures
{
    public class TextureHandler
    {
        public enum ID : short
        {
            Unknown,
            Blank,
            //add new IDs
        }

        private static Dictionary<ID, string> _paths = new Dictionary<ID, string>()
        {
            [ID.Unknown] = "Misc/Unknown",
            [ID.Blank] = "Misc/Blank",
            //add path to new IDs
        };

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Shouldn't need to change anything below here ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public ArrayByEnum<Texture2D, ID> Lookup { get; private set; } = new ArrayByEnum<Texture2D, ID>();

        public TextureHandler()
        {
            //only fill if non-server
            if (!LocalData.IS_SERVER)
            {
                //populate
                foreach (ID id in _paths.Keys)
                {
                    Lookup[id] = ModContent.GetTexture($"EAC2/Textures/{_paths[id]}");
                }

                //default to unknown
                foreach (ID id in (ID[])Enum.GetValues(typeof(ID)))
                {
                    if (Lookup[id] == null)
                    {
                        Utilities.Logger.Error($"Did not set texture path for {id}");
                        Lookup[id] = Lookup[ID.Blank];
                    }
                }
            }
        }
    }
}

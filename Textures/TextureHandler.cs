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
            Solid,
            //add new IDs
        }

        private readonly Dictionary<ID, string> _paths = new Dictionary<ID, string>()
        {
            [ID.Unknown] = "Misc/Unknown",
            [ID.Blank] = "Misc/Blank",
            [ID.Solid] = "Misc/Solid",
            //add path to new IDs
        };

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Shouldn't need to change anything below here ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private readonly Dictionary<ID, Texture2D> _textures = new Dictionary<ID, Texture2D>();
        public Texture2D Get(ID id) => _textures[id];

        public TextureHandler()
        {
            //only fill if non-server
            if (!LocalData.IS_SERVER)
            {
                //populate
                foreach (ID id in _paths.Keys)
                {
                    _textures[id] = ModContent.GetTexture($"EAC2/Textures/{_paths[id]}");
                }

                //default to unknown
                foreach (ID id in (ID[])Enum.GetValues(typeof(ID)))
                {
                    if (_textures[id] == null)
                    {
                        Utilities.Logger.Error($"Did not set texture path for {id}");
                        _textures[id] = _textures[ID.Blank];
                    }
                }
            }
        }
    }
}

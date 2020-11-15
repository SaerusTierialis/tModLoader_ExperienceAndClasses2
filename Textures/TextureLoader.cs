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
    public static class TextureLoader
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

        public static ArrayByEnum<Texture2D, ID> Textures { get; private set; }

        public static void Load()
        {
            //init
            Textures = new ArrayByEnum<Texture2D, ID>();

            //populate
            foreach (ID id in _paths.Keys)
            {
                Textures[id] = ModContent.GetTexture($"EAC2/Textures/{_paths[id]}");
            }

            //default to unknown
            foreach (ID id in (ID[])Enum.GetValues(typeof(ID)))
            {
                if (Textures[id] == null)
                {
                    Utilities.Logger.Error($"Did not set texture path for {id}");
                    Textures[id] = Textures[ID.Blank];
                }
            }
        }

        public static void Unload()
        {
            Textures = null;
        }
    }
}

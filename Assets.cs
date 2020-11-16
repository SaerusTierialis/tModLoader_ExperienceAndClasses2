using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAC2
{
    public static class Assets
    {
        public static Textures.TextureHandler Textures { get; private set; }
        public static Sounds.SoundHandler Sounds { get; private set; }

        /// <summary>
        /// Must be called AFTER setting/updating netmode to ensure that server does not load assets and that everyone else does load assets.
        /// </summary>
        public static void Load()
        {
            Textures = new Textures.TextureHandler();
            Sounds = new Sounds.SoundHandler();
        }

        public static void Unload()
        {
            Textures = null;
            Sounds = null;
        }
    }
}

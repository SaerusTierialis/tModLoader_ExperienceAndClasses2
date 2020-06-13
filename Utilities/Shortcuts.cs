using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace EAC2.Utilities
{
    static class Shortcuts
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Static Fields ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //Time (wall)
        public static DateTime Now { get; private set; }

        //Mod Shortcut
        public static Mod MOD { get; private set; } //TODO

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Shortcuts ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        //shortcuts to config so I don't have to keep adding ModContent
        public static ConfigClient GetConfigClient { get { return GetInstance<ConfigClient>(); } }
        public static ConfigServer GetConfigServer { get { return GetInstance<ConfigServer>(); } }

        public static int[] Version
        {
            get
            {
                return new int[] { MOD.Version.Major, MOD.Version.Minor, MOD.Version.Build };
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Timing ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public static void UpdateTime()
        {
            Now = DateTime.Now;
        }
    }
}

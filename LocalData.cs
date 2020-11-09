using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace EAC2
{
    /// <summary>
    /// Contains misc non-synced local data
    /// </summary>
    public static class LocalData
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Fields ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //Netmode
        public static bool IS_SERVER { get; private set; }
        public static bool IS_CLIENT { get; private set; }
        public static bool IS_SINGLEPLAYER { get; private set; }
        public static bool IS_PLAYER { get; private set; }
        public static bool IS_EFFECTIVELY_SERVER { get; private set; }
        public static int WHO_AM_I { get; private set; }

        //ModPlayer
        public static EACPlayer LOCAL_PLAYER { get; private set; }
        public static bool LOCAL_PLAYER_VALID { get; private set; }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Public Methods ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /// <summary>
        /// Called during mod load/unload. Also called indirectly by clients during OnEnterWorld processes.
        /// </summary>
        public static void ResetLocalData()
        {
            //net mode
            UpdateNetmode();

            //default to non-player
            LOCAL_PLAYER = null;
            LOCAL_PLAYER_VALID = false;
            WHO_AM_I = -1;

            //reset local data stored elsewhere...

            //xp lookup
            Systems.XPRewards.NPCRewards.ClearLoookup();
        }

        public static void SetLocalPlayer(EACPlayer local_player)
        {
            //init
            ResetLocalData();

            //set local player
            LOCAL_PLAYER = local_player;
            LOCAL_PLAYER_VALID = true;
            WHO_AM_I = LOCAL_PLAYER.player.whoAmI;

            //set data to indicate local
            LOCAL_PLAYER.PlayerData.SetAsLocal();
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Private Methods ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private static void UpdateNetmode()
        {
            IS_SERVER = (Main.netMode == NetmodeID.Server);
            IS_CLIENT = (Main.netMode == NetmodeID.MultiplayerClient);
            IS_SINGLEPLAYER = (Main.netMode == NetmodeID.SinglePlayer);

            IS_PLAYER = IS_CLIENT || IS_SINGLEPLAYER;
            IS_EFFECTIVELY_SERVER = IS_SERVER || IS_SINGLEPLAYER;
        }

    }
}

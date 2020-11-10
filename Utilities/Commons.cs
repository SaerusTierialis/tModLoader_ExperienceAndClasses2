﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace EAC2.Utilities
{
    static class Commons
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Timing ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private const uint TICKS_PER_SECOND = 60;
        public static uint TicksPerTime(double minutes, double seconds = 0)
        {
            seconds += (minutes * 60);
            return (uint)Math.Round(seconds * TICKS_PER_SECOND);
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Searches ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public static List<EACPlayer> GetEACPlayers(bool require_alive = false, Vector2 position = new Vector2(), float range = -1)
        {
            List<EACPlayer> players = new List<EACPlayer>();

            Player player;
            EACPlayer eacplayer;
            for (int player_index = 0; player_index < Main.maxPlayers; player_index++)
            {
                player = Main.player[player_index];

                //must exist
                if (!player.active) continue;

                //must be alive?
                if (require_alive && player.dead) continue;

                //distance check
                if ((range > 0) && (player.Distance(position) > range)) continue;

                //get eacplayer...
                eacplayer = player.GetModPlayer<EACPlayer>();

                //all good
                //if ((eacplayer != null) && eacplayer.PlayerData.Initialized)
                if (eacplayer != null)  
                {
                    players.Add(eacplayer);
                }
            }

            return players;
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Misc ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /// <summary>
        /// Create Object by name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="parent_type"></param>
        /// <returns></returns>
        public static T CreateObjectFromName<T>(string name, Type parent_type = null)
        {
            if (parent_type == null)
                return (T)(Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName + "+" + name));
            else
            {
                return (T)(Assembly.GetExecutingAssembly().CreateInstance(parent_type.FullName + "+" + name));
            }
        }

    }
}

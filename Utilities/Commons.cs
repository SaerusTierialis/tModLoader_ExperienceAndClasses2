﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace EAC2.Utilities
{
    static class Commons
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Misc ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

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
                players.Add(eacplayer);
            }

            return players;
        }

    }
}

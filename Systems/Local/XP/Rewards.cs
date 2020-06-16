using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace EAC2.Systems.Local.XP
{
    public static class Rewards
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Constants ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private const double BONUS_XP_RATIO_PER_PLAYER = 0.1;

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Variables ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /// <summary>
        /// true while catch-up is active locally
        /// </summary>
        public static bool CATCHUP_ACTIVE { get; private set; } = false;

        private static double XP_MULTIPLIER = 1.0;

        private static Containers.XP xp_overhead = new Containers.XP();
        private static int xp_overhead_index = Main.maxCombatText - 1;

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Public ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public static void GiveXP(uint xp, Microsoft.Xna.Framework.Rectangle location)
        {
            //apply final modifiers
            xp = CalcFinalXP(xp);

            //give xp
            LocalData.LOCAL_PLAYER.PlayerData.AddXP(xp);

            //display
            DisplayXPReward(xp, location);
        }

        public static void UpdateXPMultiplier()
        {
            if (LocalData.IS_PLAYER && LocalData.LOCAL_PLAYER_VALID)
            {
                //calc new mult...
                XP_MULTIPLIER = 1.0;

                //default to no catchup
                CATCHUP_ACTIVE = false;

                //multiplayer...
                if (LocalData.IS_CLIENT)
                {
                    //adjust for number of players
                    int number_players = CountEligiblePlayers();
                    XP_MULTIPLIER *= 1 + ((number_players - 1) * BONUS_XP_RATIO_PER_PLAYER);
                    XP_MULTIPLIER /= number_players;

                    //catch-up system
                    if (ConfigServer.Instance.AllowXPCatchup)
                    {
                        //get highest character level
                        int highest_level = 0;
                        foreach (EACPlayer eacplayer in Utilities.Commons.GetEACPlayers())
                        {
                            highest_level = (int)Math.Max(highest_level, eacplayer.PlayerData.Character.Level);
                        }

                        int level_difference = (int)(highest_level - LocalData.LOCAL_PLAYER.PlayerData.Character.Level);
                        if (level_difference >= 5)
                        {
                            CATCHUP_ACTIVE = true;
                            XP_MULTIPLIER *= 3.0 + ((level_difference - 5.0) / 50.0); //300% multiplier plus 2%/level
                        }
                    }
                }

                //apply ModConfig rate
                XP_MULTIPLIER *= ConfigServer.Instance.XPRate;
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Private ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /// <summary>
        /// Returns the number players eligible for rewards including the local player
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        private static int CountEligiblePlayers()
        {
            return Math.Max(1, Utilities.Commons.GetEACPlayers().Count);
        }

        private static void DisplayXPReward(uint xp, Microsoft.Xna.Framework.Rectangle location, bool bonus = false)
        {
            if (Main.combatText[xp_overhead_index].active)
            {
                //update text
                Main.combatText[xp_overhead_index].active = false;
            }
            else
            {
                //new text
                xp_overhead.Reset();
            }

            //add to counter
            xp_overhead.Add(xp);

            //change location to player if too far
            if (location.Distance(Main.LocalPlayer.position) > 1000f) //TODO use current screen size and check if on-scren rather than cutoff distance
            {
                location = Main.LocalPlayer.getRect();
            }

            //display
            xp_overhead_index = CombatText.NewText(location, UI.Constants.COLOUR_XP_BRIGHT, "+" + xp_overhead.Value + " XP");
            Main.combatText[xp_overhead_index].crit = true;
        }

        private static uint CalcFinalXP(uint base_xp)
        {
            return (uint)Math.Ceiling(base_xp * XP_MULTIPLIER);
        }

    }
}

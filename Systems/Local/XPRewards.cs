using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static EAC2.LocalData;
using Terraria.ID;
using EAC2.Utilities;

namespace EAC2.Systems.Local
{
    class XPRewards : GlobalNPC
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Constants ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        const double MINUTES_BETWEEN_CHECK_LOOKUP = 1;
        const double BONUS_XP_RATIO_PER_PLAYER = 0.1;

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Static Fields ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private static Dictionary<int, double> XP_lookup = new Dictionary<int, double>();
        private static DateTime time_last_check_lookup = DateTime.MinValue;

        private static Containers.XP xp_overhead = new Containers.XP();
        private static int xp_overhead_index = Main.maxCombatText - 1;

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Instance Fields ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private double base_xp_value = 0;
        private bool treat_as_boss = false;

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Overrides ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /// <summary>
        /// Instance per entity to store base xp, etc.
        /// </summary>
        public override bool InstancePerEntity => true;

        public override void SetDefaults(NPC npc)
        {
            base.SetDefaults(npc);
            //calculate xp value on base stats (uses non-expert, single-player stats)
            treat_as_boss = TreatAsBoss(npc);
            base_xp_value = GetBaseXP(npc);
        }

        public override bool CheckDead(NPC npc)
        {
            bool dying = base.CheckDead(npc);

            if (dying && IS_PLAYER && LOCAL_PLAYER_VALID)
            {
                //gives xp...
                if (base_xp_value > 0)
                {
                    //start with default
                    double xp = base_xp_value;

                    //multiplayer...
                    if (IS_CLIENT)
                    {
                        //how many players will gain xp?
                        int number_players = CountEligiblePlayers(npc);

                        //apply bonus xp per player after the first
                        xp *= 1 + ((number_players - 1) * BONUS_XP_RATIO_PER_PLAYER);

                        //divide xp by number of players
                        xp /= number_players;
                    }

                    //round up
                    uint xp_final = (uint)Math.Ceiling(xp);

                    //display xp
                    DisplayXPReward(xp_final, npc.getRect());
                    
                    //give xp
                    //TODO
                }
            }

            return dying;
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Private ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private static double CalcBaseXP(NPC npc)
        {
            //no exp from statue, critter, or friendly
            if (npc.SpawnedFromStatue || npc.lifeMax <= 5 || npc.friendly || !npc.active) return 0f;

            //calculate
            double xp = 0;
            if (npc.defense >= 1000)
                xp = (npc.lifeMax / 80d) * (1d + (npc.damage / 20d));
            else
                xp = (npc.lifeMax / 100d) * (1d + (npc.defense / 10d)) * (1d + (npc.damage / 25d));

            //adjustment to keep xp approx where it was pre-revamp
            xp *= 3.0;

            //special cases
            switch (npc.type)
            {
                case NPCID.EaterofWorldsHead:
                    xp *= 1.801792115f;
                    break;

                case NPCID.EaterofWorldsBody:
                    xp *= 1.109713024f;
                    break;

                case NPCID.EaterofWorldsTail:
                    xp *= 0.647725809f;
                    break;
            }

            //round up
            xp = Math.Ceiling(xp);

            //apply ModConfig rate
            xp *= Utilities.Shortcuts.GetConfigServer.XPRate;

            return xp;
        }

        /// <summary>
        /// Returns the number players eligible for rewards including the local player
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        private int CountEligiblePlayers(NPC npc)
        {
            return Commons.GetEACPlayers(false).Count;
        }

        private static bool TreatAsBoss(NPC npc)
        {
            if (npc.boss)
                return true;
            else
            {
                switch (npc.type)
                {
                    case NPCID.EaterofWorldsHead:
                    case NPCID.EaterofWorldsBody:
                    case NPCID.EaterofWorldsTail:
                        return true;
                    default:
                        return false;
                }
            }
        }

        private void DisplayXPReward(uint amount, Microsoft.Xna.Framework.Rectangle location, bool bonus = false)
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
            xp_overhead.Add(amount);

            //change location to player if too far
            if (location.Distance(Main.LocalPlayer.position) > 1000f) //TODO use current screen size and check if on-scren rather than cutoff distance
            {
                location = Main.LocalPlayer.getRect();
            }
            
            //display
            xp_overhead_index = CombatText.NewText(location, UI.Constants.COLOUR_XP_BRIGHT, "+" + xp_overhead.Value + " XP");
            Main.combatText[xp_overhead_index].crit = true;
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Public ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public static double GetBaseXP(NPC npc)
        {
            if (!XP_lookup.ContainsKey(npc.type))
            {
                //calculate if not in lookup
                XP_lookup[npc.type] = CalcBaseXP(npc);
            }
            else if (Utilities.Shortcuts.Now.Subtract(time_last_check_lookup).TotalMinutes > MINUTES_BETWEEN_CHECK_LOOKUP)
            {
                //every so often, check values in lookup - if mismatch then clear entire lookup
                double current_value = CalcBaseXP(npc);
                if (XP_lookup[npc.type] != current_value)
                {
                    ClearLoookup();
                    XP_lookup[npc.type] = current_value;
                }
                time_last_check_lookup = Utilities.Shortcuts.Now.AddMinutes(MINUTES_BETWEEN_CHECK_LOOKUP);
            }

            //return lookup value
            return XP_lookup[npc.type];
        }

        public static void ClearLoookup()
        {
            XP_lookup.Clear();
        }

    }
}

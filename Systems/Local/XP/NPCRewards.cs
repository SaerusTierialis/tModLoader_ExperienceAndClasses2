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

namespace EAC2.Systems.Local.XP
{
    class NPCRewards : GlobalNPC
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Constants ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private const uint TICKS_BETWEEN_CHECK_LOOKUP = 3600; //1 minute

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Static Fields ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private static Dictionary<int, uint> XP_lookup = new Dictionary<int, uint>();
        private static uint time_next_check_lookup = TICKS_BETWEEN_CHECK_LOOKUP;

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Overrides ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /// <summary>
        /// Instance per entity to store base xp, etc.
        /// </summary>
        public override bool InstancePerEntity => true;

        public override void SetDefaults(NPC npc)
        {
            base.SetDefaults(npc);
            //calculate xp value on base stats (uses non-expert, single-player stats)
            UpdateXPValue(npc);
        }

        public override bool CheckDead(NPC npc)
        {
            bool dying = base.CheckDead(npc);

            if (dying && IS_PLAYER && LOCAL_PLAYER_VALID && (XP_lookup[npc.type] > 0))
            {
                Rewards.GiveXP(XP_lookup[npc.type], npc.getRect());
            }

            return dying;
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Private ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private static void UpdateXPValue(NPC npc)
        {
            //get base xp
            if (!XP_lookup.ContainsKey(npc.type))
            {
                //calculate if not in lookup
                XP_lookup[npc.type] = CalcNPCValue(npc);
            }
            else if (Main.GameUpdateCount > time_next_check_lookup)
            {
                //every so often, check values in lookup - if mismatch then clear entire lookup
                uint current_value = CalcNPCValue(npc);
                if (XP_lookup[npc.type] != current_value)
                {
                    ClearLoookup();
                    XP_lookup[npc.type] = current_value;
                }
                time_next_check_lookup = Main.GameUpdateCount + TICKS_BETWEEN_CHECK_LOOKUP;
            }
        }

        private static uint CalcNPCValue(NPC npc)
        {
            //no exp from statue, critter, or friendly
            if (npc.SpawnedFromStatue || npc.lifeMax <= 5 || npc.friendly || !npc.active) return 0;

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
                    xp *= 1.801792115d;
                    break;

                case NPCID.EaterofWorldsBody:
                    xp *= 1.109713024d;
                    break;

                case NPCID.EaterofWorldsTail:
                    xp *= 0.647725809d;
                    break;
            }

            //round up and return
            return (uint)Math.Ceiling(xp);
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Public ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public static void ClearLoookup()
        {
            XP_lookup.Clear();
        }

    }
}

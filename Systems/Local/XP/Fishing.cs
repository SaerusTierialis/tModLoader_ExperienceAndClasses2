using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAC2.Systems.Local.XP
{
    public class Fishing : GlobalItem
    {
        /// <summary>
        /// in multiplayer, only called by the client that catches
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stack"></param>
        public override void CaughtFishStack(int type, ref int stack)
        {
            base.CaughtFishStack(type, ref stack);

            if ((type != ItemID.None) && (stack > 0))
            {
                //local xp
                GiveReward(type);

                //xp for other clients
                if (LocalData.IS_CLIENT)
                {
                    Utilities.PacketHandler.FishXP.Send(-1, LocalData.WHO_AM_I, type);
                }
            }
        }

        public static void GiveReward(int type)
        {
            //get item from type
            Item item = new Item();
            item.SetDefaults(type);

            //TODO
            uint xp = 1;

            //apply
            Rewards.GiveXP(xp, Main.LocalPlayer.getRect());
        }

    }

  
}

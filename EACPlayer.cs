using EAC2.Containers;
using EAC2.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EAC2
{
    public class EACPlayer : ModPlayer
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Init ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /// <summary>
        /// A container to store fields with defaults in a way that is easy to reinitialize
        /// </summary>
        public Containers.PlayerData PlayerData { get; private set; }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Init/Deinit ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public override void Initialize()
        {
            PlayerData = new Containers.PlayerData(player.whoAmI == Main.LocalPlayer.whoAmI);
            base.Initialize();
        }

        public override void OnEnterWorld(Player player)
        {
            base.OnEnterWorld(player);

            //initialize local data
            LocalData.SetLocalPlayer(this);

            //calc XP mult
            Systems.XPRewards.Rewards.UpdateXPMultiplier();

            //request world data from server
            if (LocalData.IS_CLIENT)
            {
                PacketHandler.ClientRequestWorldData.Send(-1, LocalData.WHO_AM_I);
            }
        }

        public override void PlayerConnect(Player player)
        {
            base.PlayerConnect(player);
            Systems.XPRewards.Rewards.UpdateXPMultiplier();
        }

        public override void PlayerDisconnect(Player player)
        {
            base.PlayerDisconnect(player);
            Systems.XPRewards.Rewards.UpdateXPMultiplier();
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Sync ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            base.SendClientChanges(clientPlayer);
            PlayerData.DoSyncs();
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Temp ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override void PreUpdate()
        {
            base.PreUpdate();

            //reset values that are calculated each cycle
            PlayerData.Update();


            if (LocalData.IS_PLAYER)
            {
                //Main.NewText(player.name + " Level " + PlayerData.Character.Level + ": " + PlayerData.Character.XP + " / " + PlayerData.Character.XP_Needed);
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Save/Load ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override TagCompound Save()
        {
            //base fields
            TagCompound tag = base.Save();
            if (tag == null)
                tag = new TagCompound();

            //mod fields
            tag = PlayerData.Save(tag);

            //return
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            //base load
            base.Load(tag);

            //mod fields
            PlayerData.Load(tag);
        }

    }
}

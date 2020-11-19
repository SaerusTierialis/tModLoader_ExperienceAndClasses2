using ACE.Containers;
using ACE.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ACE
{
    public class ACEPlayer : ModPlayer
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Init ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /// <summary>
        /// A container to store fields with defaults in a way that is easy to reinitialize
        /// </summary>
        public PlayerData PlayerData { get; private set; }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Init/Deinit ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public override void Initialize()
        {
            PlayerData = new Containers.PlayerData(this);
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

        /// <summary>
        /// When a client joins a world that already has at least once client, each client calls this.
        /// The joining client will call this once per existing player (duplicate calls).
        /// Passed Player and ACEPlayer are not yet initialized (doesn't even have name, whoami, etc.)
        /// Server never calls this.
        /// </summary>
        /// <param name="player"></param>
        public override void PlayerConnect(Player player)
        {
            base.PlayerConnect(player);

            //anything here may be called several times in a row by a client joining a populated server
            Systems.XPRewards.Rewards.UpdateXPMultiplier();
        }

        /// <summary>
        /// Each client calls this when a different client leaves. Player and ACEPlayer data are correct.
        /// Server never calls this.
        /// </summary>
        /// <param name="player"></param>
        public override void PlayerDisconnect(Player player)
        {
            base.PlayerDisconnect(player);
            Systems.XPRewards.Rewards.UpdateXPMultiplier();
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Sync ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /// <summary>
        /// Called by each client on each cycle. Mean to detect and send changes.
        /// </summary>
        /// <param name="clientPlayer"></param>
        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            base.SendClientChanges(clientPlayer);
            PlayerData.DoSyncs();
        }

        /// <summary>
        /// Server calls this in two steps.
        ///     1. Send data for new client to prior clients (to -1), but there probably isn't much to send at that point
        ///     2. Then, send data from EACH prior client to just the new client (not to -1).
        /// 
        /// Clients seem to never call this. newPlayer doesn't appear to be used.
        /// </summary>
        /// <param name="toWho"></param>
        /// <param name="fromWho"></param>
        /// <param name="newPlayer"></param>
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            base.SyncPlayer(toWho, fromWho, newPlayer);
            if (toWho != -1)
            {
                //sending this player's data to the new client
                PlayerData.DoTargetedSyncFromServer(toWho);
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Update ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override void PreUpdate()
        {
            base.PreUpdate();

            //reset values that are calculated each cycle
            PlayerData.PreUpdate();
        }

        public override void PostUpdateMiscEffects()
        {
            base.PostUpdateMiscEffects();

            //apply per-cycle effects from PlayerData
            PlayerData.Update();
        }

        public override void PostUpdate()
        {
            base.PostUpdate();

            //apply per-cycle effects from PlayerData
            PlayerData.PostUpdate();
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

            //also trigger UI save
            LocalData.UIData?.Save();

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

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Inputs ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            Hotkeys.CheckHotkeys(triggersSet);
        }

    }
}

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

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Init ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public override void Initialize()
        {
            PlayerData = new Containers.PlayerData();
            base.Initialize();
        }

        public override void OnEnterWorld(Player player)
        {
            base.OnEnterWorld(player);

            //initialize local data
            LocalData.SetLocalPlayer(this);
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

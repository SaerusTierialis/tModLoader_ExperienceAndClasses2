using ACE.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ACE.Systems.PlayerModules
{
    public class CharacterLevel : AutoDataPlayer<uint>
    {
        public CharacterLevel(PlayerModule parent, byte id, uint value_initial, bool syncs = false, bool resets = false) : base(parent, id, value_initial, syncs, resets)
        {
        }

        protected override void OnChange()
        {
            Systems.XPRewards.Rewards.UpdateXPMultiplier();
        }
    }

    public class AttributeFinalPower : AutoDataPlayer<int>
    {
        public AttributeFinalPower(PlayerModule parent, byte id) : base(parent, id, 0, false, false) { }

        protected override void OnUpdate()
        {
            //placeholder 10% damage per point
            ParentPlayerModule.ParentPlayerData.ACEPlayer.player.allDamageMult += (value / 10f);
            if (LocalData.IS_PLAYER)
                Main.NewText($"{ParentPlayerModule.ParentPlayerData.ACEPlayer.player.name} {ParentPlayerModule.ParentPlayerData.ACEPlayer.player.allDamageMult}");
        }
    }
}

using EAC2.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAC2.Systems.PlayerModules
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
}

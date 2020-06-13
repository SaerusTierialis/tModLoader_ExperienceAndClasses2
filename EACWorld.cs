using EAC2.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace EAC2
{
    class EACWorld : ModWorld
    {
        public override void PreUpdate()
        {
            Shortcuts.UpdateTime();
            base.PreUpdate();
        }
    }
}

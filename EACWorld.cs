using EAC2.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EAC2
{
    class EACWorld : ModWorld
    {
        public override TagCompound Save()
        {
            return new TagCompound
            {
                ["eac_tiles_placed"] = Systems.XPRewards.TileRewards.GetCoordList()
            };
        }

        public override void Load(TagCompound tag)
        {
            //value tiles placed
            Systems.XPRewards.TileRewards.SetTilesPlaced(SaveLoad.TagTryGet<List<int>>(tag, "eac_tiles_placed", new List<int>()));
        }

    }
}

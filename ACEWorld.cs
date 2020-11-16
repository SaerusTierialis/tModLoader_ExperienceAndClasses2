using ACE.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ACE
{
    class ACEWorld : ModWorld
    {
        public override TagCompound Save()
        {
            return new TagCompound
            {
                [Tags.Get(Tags.ID.World_Tiles_Placed)] = Systems.XPRewards.TileRewards.GetCoordList()
            };
        }

        public override void Load(TagCompound tag)
        {
            //value tiles placed
            Systems.XPRewards.TileRewards.SetTilesPlaced(SaveLoad.TagTryGet<List<int>>(tag, Tags.Get(Tags.ID.World_Tiles_Placed), new List<int>()));
        }

    }
}

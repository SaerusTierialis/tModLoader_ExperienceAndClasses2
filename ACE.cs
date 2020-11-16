using ACE.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;
using Terraria.UI;

namespace ACE
{
	public class ACE : Mod
	{
		public ACE(){}

        public static Mod MOD;

        public const string MOD_NAME = "ACE";

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ UI ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override void UpdateUI(GameTime time)
        {
            LocalData.UIData?.Update(time);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer($"{MOD_NAME}_UIMain",
                    delegate {
                        LocalData.UIData?.Draw();
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Load/Unload ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override void Load()
        {
            //shortcut to mod
            MOD = ModLoader.GetMod(MOD_NAME);

            //add serializers
            Terraria.ModLoader.IO.TagSerializer.AddSerializer(new Containers.XPLevel.XPLevelSerializer());

            //reset local data
            LocalData.ResetLocalData();

            //load textures
            Assets.Load();
        }

        public override void Unload()
        {
            MOD = null;
            Assets.Unload();
            LocalData.ResetLocalData(); //should help with garbage collection
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Packets ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            //first 2 bytes are always type and origin
            byte packet_type = reader.ReadByte();
            int origin = reader.ReadInt32();

            if (packet_type >= 0 && packet_type < PacketHandler.LOOKUP.Count)
            {
                Utilities.PacketHandler.LOOKUP[(PacketHandler.PACKET_TYPE)packet_type].Recieve(reader, origin);
            }
            else
            {
                Utilities.Logger.Error("Cannot handle packet type id " + packet_type + " originating from " + origin);
            }
        }

    }
}
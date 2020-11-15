using EAC2.Utilities;
using System.IO;
using Terraria.ModLoader;

namespace EAC2
{
	public class EAC2 : Mod
	{
		public EAC2(){}

        public static Mod MOD;

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Load/Unload ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override void Load()
        {
            //shortcut to mod
            MOD = ModLoader.GetMod("EAC2");

            //add serializers
            Terraria.ModLoader.IO.TagSerializer.AddSerializer(new Containers.XPLevel.XPLevelSerializer());

            //reset local data
            LocalData.ResetLocalData();
        }

        public override void Unload()
        {
            MOD = null;
            LocalData.ResetLocalData(); //should help with garbage collection
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Packets ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            //first 2 bytes are always type and origin
            byte packet_type = reader.ReadByte();
            int origin = reader.ReadInt32();

            if (packet_type >= 0 && packet_type < PacketHandler.LOOKUP.Length)
            {
                Utilities.PacketHandler.LOOKUP[packet_type].Recieve(reader, origin);
            }
            else
            {
                Utilities.Logger.Error("Cannot handle packet type id " + packet_type + " originating from " + origin);
            }
        }

    }
}
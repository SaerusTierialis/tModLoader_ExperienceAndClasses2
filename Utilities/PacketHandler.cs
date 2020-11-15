using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using EAC2.Containers;

namespace EAC2.Utilities
{
    public static class PacketHandler
    {
        //IMPORTANT: each type MUST have a class with the exact same name
        public enum PACKET_TYPE : byte
        {
            ClientAutoDataPlayer,
            ClientBroadcast,
            ClientFishXP,
            ClientPlaceValueTile,
            ClientRequestWorldData,
            ServerPlacedValueTiles,
        };

        //lookup for receiving of packets
        public static readonly Dictionary<PACKET_TYPE, Handler> LOOKUP = new Dictionary<PACKET_TYPE, Handler>();

        static PacketHandler()
        {
            foreach (PACKET_TYPE type in Enum.GetValues(typeof(PACKET_TYPE)))
            {
                LOOKUP[type] = Commons.CreateObjectFromName<Handler>(Enum.GetName(typeof(PACKET_TYPE), type), typeof(PacketHandler));
            }
        }

        //base type
        public abstract class Handler
        {
            public PACKET_TYPE ID { get; private set; }
            public byte ID_Num { get; private set; }

            public Handler(PACKET_TYPE id)
            {
                ID = id;
                ID_Num = (byte)ID;
            }

            public ModPacket GetPacket(int origin)
            {
                ModPacket packet = EAC2.MOD.GetPacket();
                packet.Write(ID_Num);
                packet.Write(origin);
                return packet;
            }

            public void Recieve(BinaryReader reader, int origin)
            {
                //do not read anything from reader here (called multiple times when processing full sync packet)

                bool do_trace = ConfigServer.Instance.Trace && (ID != PACKET_TYPE.ClientBroadcast);

                if (do_trace)
                {
                    Logger.Trace("Handling " + ID + " originating from " + origin);
                }

                EACPlayer origin_eacplayer = null;
                if ((origin >= 0) && (origin <= Main.maxPlayers))
                {
                    origin_eacplayer = Main.player[origin].GetModPlayer<EACPlayer>();
                }

                RecieveBody(reader, origin, origin_eacplayer);

                if (do_trace)
                {
                    Logger.Trace("Done handling " + ID + " originating from " + origin);
                }
            }

            protected abstract void RecieveBody(BinaryReader reader, int origin, EACPlayer origin_eacplayer);
        }

        public sealed class ClientAutoDataPlayer : Handler
        {
            private const PACKET_TYPE ptype = PACKET_TYPE.ClientAutoDataPlayer;

            public ClientAutoDataPlayer() : base(ptype) { }

            public static void Send<T>(int target, int origin, byte module_index, DATATYPE datatype, byte data_index, T value)
            {
                //get packet containing header
                ModPacket packet = LOOKUP[ptype].GetPacket(origin);

                //byte module index
                packet.Write(module_index);

                //byte datatype
                packet.Write((byte)datatype);

                //byte data index in module
                packet.Write(data_index);

                //write value
                switch (datatype)
                {
                    case DATATYPE.BOOL:
                        packet.Write(Convert.ToBoolean(value));
                        break;
                    case DATATYPE.BYTE:
                        packet.Write(Convert.ToByte(value));
                        break;
                    case DATATYPE.FLOAT:
                        packet.Write(Convert.ToSingle(value));
                        break;
                    case DATATYPE.INT32:
                        packet.Write(Convert.ToInt32(value));
                        break;
                    case DATATYPE.UINT32:
                        packet.Write(Convert.ToUInt32(value));
                        break;
                    default:
                        Logger.Error($"Attempted to send AutoData packet of unsupported type {datatype}");
                        return;
                }

                //send
                packet.Send(target, origin);
            }

            protected override void RecieveBody(BinaryReader reader, int origin, EACPlayer origin_eacplayer)
            {
                //byte module index
                byte module_index = reader.ReadByte();

                //byte datatype
                DATATYPE datatype = (DATATYPE)reader.ReadByte();

                //byte data index in module
                byte data_index = reader.ReadByte();

                switch (datatype)
                {
                    case DATATYPE.BOOL:
                        HandleValue(origin, origin_eacplayer, module_index, datatype, data_index, reader.ReadBoolean());
                        break;
                    case DATATYPE.BYTE:
                        HandleValue(origin, origin_eacplayer, module_index, datatype, data_index, reader.ReadByte());
                        break;
                    case DATATYPE.FLOAT:
                        HandleValue(origin, origin_eacplayer, module_index, datatype, data_index, reader.ReadSingle());
                        break;
                    case DATATYPE.INT32:
                        HandleValue(origin, origin_eacplayer, module_index, datatype, data_index, reader.ReadInt32());
                        break;
                    case DATATYPE.UINT32:
                        HandleValue(origin, origin_eacplayer, module_index, datatype, data_index, reader.ReadUInt32());
                        break;
                    default:
                        Logger.Error($"Received AutoData packet of unsupported type {datatype}");
                        return;
                }
            }

            private static void HandleValue<T>(int origin, EACPlayer origin_eacplayer, byte module_index, DATATYPE datatype, byte data_index, T value)
            {
                origin_eacplayer.PlayerData.SetAutoData(module_index, datatype, data_index, value);
                //if server, relay to other clients
                if (LocalData.IS_SERVER)
                    Send(-1, origin, module_index, datatype, data_index, value);
            }
        }

        /// <summary>
        /// Client request broadcast from server
        /// </summary>
        public sealed class ClientBroadcast : Handler
        {
            private const PACKET_TYPE ptype = PACKET_TYPE.ClientBroadcast;

            public enum BROADCAST_TYPE : byte
            {
                MESSAGE,
                TRACE,
                ERROR
            }

            public ClientBroadcast() : base(ptype) { }

            public static void Send(int target, int origin, string message, BROADCAST_TYPE type)
            {
                //get packet containing header
                ModPacket packet = LOOKUP[ptype].GetPacket(origin);

                //type
                packet.Write((byte)type);

                //message
                packet.Write(message);

                //send
                packet.Send(target, origin);
            }

            protected override void RecieveBody(BinaryReader reader, int origin, EACPlayer origin_eacplayer)
            {
                //type
                BROADCAST_TYPE type = (BROADCAST_TYPE)reader.ReadByte();

                //message
                string message = reader.ReadString();

                //colour
                Color colour = Color.White;
                switch (type)
                {
                    case BROADCAST_TYPE.ERROR:
                        colour = UI.Constants.COLOUR_MESSAGE_ERROR;
                        break;
                    case BROADCAST_TYPE.TRACE:
                        colour = UI.Constants.COLOUR_MESSAGE_TRACE;
                        break;
                    case BROADCAST_TYPE.MESSAGE:
                        colour = UI.Constants.COLOUR_MESSAGE_BROADCAST;
                        break;
                }

                //broadcast
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(message), colour);

                //also write in console
                Console.WriteLine(message);
            }
        }

        public sealed class ClientFishXP : Handler
        {
            private const PACKET_TYPE ptype = PACKET_TYPE.ClientFishXP;

            public ClientFishXP() : base(ptype) { }

            public static void Send(int target, int origin, int type)
            {
                //get packet containing header
                ModPacket packet = LOOKUP[ptype].GetPacket(origin);

                //item type
                packet.Write(type);

                //send
                packet.Send(target, origin);
            }

            protected override void RecieveBody(BinaryReader reader, int origin, EACPlayer origin_eacplayer)
            {
                //type
                int type = reader.ReadInt32();

                if (LocalData.IS_SERVER)
                {
                    //relay
                    Send(-1, origin, type);
                }
                else //client
                {
                    //get xp for type
                    Systems.XPRewards.FishRewards.GiveReward(type);
                }
            }
        }

        public sealed class ClientPlaceValueTile : Handler
        {
            private const PACKET_TYPE ptype = PACKET_TYPE.ClientPlaceValueTile;

            public ClientPlaceValueTile() : base(ptype) { }

            public static void Send(int target, int origin, int i, int j)
            {
                //get packet containing header
                ModPacket packet = LOOKUP[ptype].GetPacket(origin);

                //tile coord
                packet.Write(i);
                packet.Write(j);

                //send
                packet.Send(target, origin);
            }

            protected override void RecieveBody(BinaryReader reader, int origin, EACPlayer origin_eacplayer)
            {
                //tile coord
                int i = reader.ReadInt32();
                int j = reader.ReadInt32();

                if (LocalData.IS_SERVER)
                {
                    //relay to clients
                    Send(-1, origin, i, j);
                }

                //set coord as placed (client + server)
                Systems.XPRewards.TileRewards.PlaceTile(i, j);
            }
        }

        public sealed class ClientRequestWorldData : Handler
        {
            private const PACKET_TYPE ptype = PACKET_TYPE.ClientRequestWorldData;

            public ClientRequestWorldData() : base(ptype) { }

            public static void Send(int target, int origin)
            {
                //get packet containing header
                ModPacket packet = LOOKUP[ptype].GetPacket(origin);

                //send
                packet.Send(target, origin);
            }

            protected override void RecieveBody(BinaryReader reader, int origin, EACPlayer origin_eacplayer)
            {
                //send list of plaved value times
                ServerPlacedValueTiles.Send(origin, -1);
            }
        }

        public sealed class ServerPlacedValueTiles : Handler
        {
            private const PACKET_TYPE ptype = PACKET_TYPE.ServerPlacedValueTiles;

            public ServerPlacedValueTiles() : base(ptype) { }

            public static void Send(int target, int origin)
            {
                //get packet containing header
                ModPacket packet = LOOKUP[ptype].GetPacket(origin);

                //tile coord
                List<int> coords = Systems.XPRewards.TileRewards.GetCoordList();
                packet.Write(coords.Count);
                foreach (int c in coords)
                {
                    packet.Write(c);
                }

                //send
                packet.Send(target, origin);
            }

            protected override void RecieveBody(BinaryReader reader, int origin, EACPlayer origin_eacplayer)
            {
                //read tile coord
                int count = reader.ReadInt32();
                List<int> coords = new List<int>();
                for (int i=0; i<count; i++)
                {
                    coords.Add(reader.ReadInt32());
                }

                //set coord
                Systems.XPRewards.TileRewards.SetTilesPlaced(coords);
            }
        }
    }
}

using EAC2.Systems.PlayerModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader.IO;

namespace EAC2.Containers
{
    /// <summary>
    /// Contains EAC-related ModPlayer data
    /// </summary>
    public class PlayerData
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Constants ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private readonly uint TiCKS_FULL_SYNC = Utilities.Commons.TicksPerTime(2);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Fields ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public readonly EACPlayer EACPlayer;
        public bool Is_Local { get; private set; } = false;

        private readonly ArrayByEnum<PlayerModule, Modules> _modules = new ArrayByEnum<PlayerModule, Modules>();
        private enum Modules : byte
        {
            Character,
        }
        public Character Character { get { return (Character)_modules[Modules.Character]; } }
        //ADD OTHER MODULE SHORTCUTS HERE

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Variables ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private uint _ticks_until_full_sync;

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Init ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public PlayerData(EACPlayer eacplayer)
        {
            EACPlayer = eacplayer;
            foreach (Modules m in (Modules[])Enum.GetValues(typeof(Modules)))
            {
                switch (m)
                {
                    case Modules.Character:
                        _modules[Modules.Character] = new Character(this, (byte)m);
                        break;

                    //ADD OTHER MODULE INITS HERE

                    default:
                        Utilities.Logger.Error("Attempted to create non-implemented PlayerModule " + m);
                        break;
                }
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Sync Access ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public void SetAutoData<T>(byte module_index, PlayerModule.DATATYPE datatype, byte data_index, T value)
        {
            _modules[module_index].SetAutoData<T>(datatype, data_index, value);
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Actions ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public void Update()
        {
            //check if first update
            if (!Character.Active)
            {
                //do any frist update setup
                Character.Activate();
                _ticks_until_full_sync = TiCKS_FULL_SYNC;
            }

            //local actions at real time intervals
            if (Is_Local)
            {
                if ((_ticks_until_full_sync--) == 0)
                {
                    FullSync();
                    _ticks_until_full_sync = TiCKS_FULL_SYNC;
                }
            }

            //actions on each cycle...
            foreach (PlayerModule m in _modules)
            {
                m.Update();
            }
        }

        public void DoSyncs()
        {
            if (!Is_Local)
            {
                Utilities.Logger.Error("Attempted DoSyncs in non-local PlayerData");
            }
            else
            {
                foreach (PlayerModule m in _modules)
                {
                    m.DoSyncs();
                }
            }
        }

        public void DoTargetedSyncFromServer(int toWho)
        {
            if (!LocalData.IS_SERVER)
            {
                Utilities.Logger.Error("Attempted DoTargetedSyncFromServer in non-server PlayerData");
            }
            else
            {
                foreach (PlayerModule m in _modules)
                {
                    m.DoTargetedSyncFromServer(toWho);
                }
            }
        }

        public void FullSync()
        {
            if (!Is_Local)
            {
                Utilities.Logger.Error("Attempted FullSync in non-local PlayerData");
            }
            else
            {
                foreach (PlayerModule m in _modules)
                {
                    m.FullSync();
                }
            }
        }

        public void SetAsLocal()
        {
            Is_Local = true;
        }

        public void AddXP(uint xp)
        {
            if (!Is_Local)
            {
                Utilities.Logger.Error("Attempted AddXP in non-local PlayerData");
            }
            else
            {
                Character.AddXP(xp);
                //TODO add xp to current classes too
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Save/Load ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public TagCompound Save(TagCompound tag)
        {
            foreach (PlayerModule m in _modules)
            {
                tag = m.Save(tag);
            }
            return tag;
        }

        public void Load(TagCompound tag)
        {
            foreach (PlayerModule m in _modules)
            {
                m.Load(tag);
            }
        }
    }
}

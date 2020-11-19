using ACE.Systems.PlayerModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader.IO;

namespace ACE.Containers
{
    /// <summary>
    /// Contains ACE-related ModPlayer data
    /// </summary>
    public class PlayerData
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Constants ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private readonly uint TiCKS_FULL_SYNC = Utilities.Commons.TicksPerTime(2);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Fields ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public readonly ACEPlayer ACEPlayer;
        public bool Is_Local { get; private set; } = false;

        private readonly Dictionary<Modules, PlayerModule> _modules = new Dictionary<Modules, PlayerModule>();
        private enum Modules : byte
        {
            Character,
            Attributes,
        }
        public CharacterModule Character { get { return (CharacterModule)_modules[Modules.Character]; } }
        public AttributeModule Attributes { get { return (AttributeModule)_modules[Modules.Attributes]; } }
        //ADD OTHER MODULE SHORTCUTS HERE

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Variables ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private uint _ticks_until_full_sync;

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Init ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public PlayerData(ACEPlayer aceplayer)
        {
            ACEPlayer = aceplayer;
            PopulateModules();

            void PopulateModules()
            {
                //init all modules...
                _modules[Modules.Character] = new CharacterModule(this, (byte)Modules.Character);
                _modules[Modules.Attributes] = new AttributeModule(this, (byte)Modules.Attributes);
                //ADD FUTURE MODULES HERE <------------------------------------

                //warn if any not set
                foreach (Modules m in (Modules[])Enum.GetValues(typeof(Modules)))
                {
                    if (_modules[m] == null)
                        Utilities.Logger.Error($"Did not initialize PlayerModule {m}");
                }
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Sync Access ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public void SetAutoData<T>(byte module_index, DATATYPE datatype, byte data_index, T value)
        {
            _modules[(Modules)module_index].SetAutoData<T>(datatype, data_index, value);
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Actions ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public void PreUpdate()
        {
            //check if first update
            if (!Character.Active)
            {
                //do any frist update setup
                Character.Activate();
                Attributes.Activate();
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
            foreach (PlayerModule m in _modules.Values)
            {
                m?.PreUpdate();
            }
        }

        public void Update()
        {
            //actions on each cycle...
            foreach (PlayerModule m in _modules.Values)
            {
                m?.Update();
            }
        }

        public void PostUpdate()
        {
            //actions on each cycle...
            foreach (PlayerModule m in _modules.Values)
            {
                m?.PostUpdate();
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
                foreach (PlayerModule m in _modules.Values)
                {
                    m?.DoSyncs();
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
                foreach (PlayerModule m in _modules.Values)
                {
                    m?.DoTargetedSyncFromServer(toWho);
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
                foreach (PlayerModule m in _modules.Values)
                {
                    m?.FullSync();
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
            foreach (PlayerModule m in _modules.Values)
            {
                tag = m?.Save(tag);
            }
            return tag;
        }

        public void Load(TagCompound tag)
        {
            foreach (PlayerModule m in _modules.Values)
            {
                m?.Load(tag);
            }
        }
    }
}

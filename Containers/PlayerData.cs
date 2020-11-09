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
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Fields ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private enum Modules : byte
        {
            Character,
        }

        public readonly bool Is_Local;
        private readonly ArrayByEnum<PlayerModule,Modules> _modules;

        public Character Character { get { return (Character)_modules[Modules.Character]; } }
        //ADD OTHER MODULE SHORTCUTS HERE

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Defaults ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public PlayerData(bool local)
        {
            Is_Local = local;
            _modules = new ArrayByEnum<PlayerModule, Modules>();
            foreach (Modules m in (Modules[])Enum.GetValues(typeof(Modules)))
            {
                switch (m)
                {
                    case Modules.Character:
                        _modules[Modules.Character] = new Character(this, (byte)m, Is_Local, true);
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

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Actions ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

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

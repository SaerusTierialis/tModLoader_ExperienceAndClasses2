using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace EAC2.Containers
{
    public abstract class PlayerModule
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Fields ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public readonly PlayerData ParentPlayerData;
        protected abstract AutoData<float>[] GetFloats();
        protected abstract AutoData<bool>[] GetBools();
        protected abstract AutoData<byte>[] GetBytes();
        protected abstract AutoData<int>[] GetInts();
        protected abstract AutoData<uint>[] GetUInts();
        protected readonly bool Is_Local;
        public readonly byte Module_Index;

        public enum DATATYPE : byte
        {
            FLOAT,
            BOOL,
            BYTE,
            INT32,
            UINT32,
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Variables ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /// <summary>
        /// Inactive modules do not update or sync.
        /// </summary>
        public bool Active { get; protected set; }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Constructor ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public PlayerModule(PlayerData parent, byte module_index, bool is_local, bool active)
        {
            ParentPlayerData = parent;
            Is_Local = is_local;
            Active = active;
            Module_Index = module_index;
        }

        /// <summary>
        /// Look for uninitialized AutoData entries and issue warning
        /// </summary>
        protected void CheckAutoData()
        {
            foreach (var d in GetFloats()) { if (d==null) Utilities.Logger.Error($"Empty AutoFloat in module {Module_Index}"); }
            foreach (var d in GetBools()) { if (d == null) Utilities.Logger.Error($"Empty AutoBool in module {Module_Index}"); }
            foreach (var d in GetBytes()) { if (d == null) Utilities.Logger.Error($"Empty AutoByte in module {Module_Index}"); }
            foreach (var d in GetInts()) { if (d == null) Utilities.Logger.Error($"Empty AutoInt in module {Module_Index}"); }
            foreach (var d in GetUInts()) { if (d == null) Utilities.Logger.Error($"Empty AutoUInt in module {Module_Index}"); }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Sync Access ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public void SetAutoData<T>(DATATYPE datatype, byte data_index, T value)
        {
            switch (datatype)
            {
                case DATATYPE.BOOL:
                    GetBools()[data_index].value = Convert.ToBoolean(value);
                    break;
                case DATATYPE.BYTE:
                    GetBytes()[data_index].value = Convert.ToByte(value);
                    break;
                case DATATYPE.FLOAT:
                    GetFloats()[data_index].value = Convert.ToSingle(value);
                    break;
                case DATATYPE.INT32:
                    GetInts()[data_index].value = Convert.ToInt32(value);
                    break;
                case DATATYPE.UINT32:
                    GetUInts()[data_index].value = Convert.ToUInt32(value);
                    break;
                default:
                    Utilities.Logger.Error($"Attempted to set AutoData of unsupported type {datatype} in module {Module_Index}");
                    break;
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Actions ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public void Activate()
        {
            Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }

        /// <summary>
        /// Called by all. Update fields, then OnUpdate, then OnUpdateLocal.
        /// </summary>
        public void Update()
        {
            if (Active)
            {
                foreach (var d in GetFloats()) { d.Update(); }
                foreach (var d in GetBools()) { d.Update(); }
                foreach (var d in GetBytes()) { d.Update(); }
                foreach (var d in GetInts()) { d.Update(); }
                foreach (var d in GetUInts()) { d.Update(); }
                OnUpdate();
                if (Is_Local)
                {
                    OnUpdateLocal();
                }
            }
        }

        /// <summary>
        /// Called by local.
        /// </summary>
        public void DoSyncs()
        {
            if (Active)
            {
                if (!Is_Local)
                {
                    Utilities.Logger.Error("Attempted DoSyncs in non-local PlayerModule");
                }
                else
                {
                    foreach (var d in GetFloats()) { d.SyncIfChanged(); }
                    foreach (var d in GetBools()) { d.SyncIfChanged(); }
                    foreach (var d in GetBytes()) { d.SyncIfChanged(); }
                    foreach (var d in GetInts()) { d.SyncIfChanged(); }
                    foreach (var d in GetUInts()) { d.SyncIfChanged(); }
                }
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Overrides ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /// <summary>
        /// Called by all. Update fields, then OnUpdate, then OnUpdateLocal.
        /// </summary>
        public virtual void OnUpdate() { }
        /// <summary>
        /// Called only by local owner. Update fields, then OnUpdate, then OnUpdateLocal.
        /// </summary>
        public virtual void OnUpdateLocal() { }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Save/Load ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public virtual TagCompound Save(TagCompound tag) { return tag; }
        public virtual void Load(TagCompound tag) { }
    }
}

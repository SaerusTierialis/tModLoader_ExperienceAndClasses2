using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace ACE.Containers
{
    public abstract class PlayerModule
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Fields ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public readonly PlayerData ParentPlayerData;
        protected abstract IEnumerable<AutoDataPlayer<float>> GetFloats();
        protected abstract IEnumerable<AutoDataPlayer<bool>> GetBools();
        protected abstract IEnumerable<AutoDataPlayer<byte>> GetBytes();
        protected abstract IEnumerable<AutoDataPlayer<int>> GetInts();
        protected abstract IEnumerable<AutoDataPlayer<uint>> GetUInts();

        /// <summary>
        /// Index in PlayerData
        /// </summary>
        public readonly byte Module_Index;

        /// <summary>
        /// Core modules cannot be deactivated
        /// </summary>
        public readonly bool Core_Module;

        public bool Is_Local => ParentPlayerData.Is_Local;

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Variables ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /// <summary>
        /// Inactive modules do not update or sync. All modules start as inactive to prevent sycning incorrect values during setup.
        /// </summary>
        public bool Active { get; protected set; } = false;

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Constructor ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public PlayerModule(PlayerData parent, byte module_index, bool core_module)
        {
            ParentPlayerData = parent;
            Module_Index = module_index;
            Core_Module = core_module;
        }

        /// <summary>
        /// Look for uninitialized AutoData entries and issue warning
        /// </summary>
        protected void CheckAutoData(int expected_floats, int expected_bools, int expected_bytes, int expected_ints, int expected_uints)
        {
            if (GetFloats().Count() != expected_floats) Utilities.Logger.Error($"Missing AutoFloat in module {Module_Index}");
            if (GetBools().Count()  != expected_bools)  Utilities.Logger.Error($"Missing AutoBool in module {Module_Index}");
            if (GetBytes().Count()  != expected_bytes)  Utilities.Logger.Error($"Missing AutoByte in module {Module_Index}");
            if (GetInts().Count()   != expected_ints)   Utilities.Logger.Error($"Missing AutoInt in module {Module_Index}");
            if (GetUInts().Count()  != expected_uints)  Utilities.Logger.Error($"Missing AutoUInt in module {Module_Index}");
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Sync Access ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public void SetAutoData<T>(DATATYPE datatype, byte data_index, T value)
        {
            switch (datatype)
            {
                case DATATYPE.BOOL:
                    GetBools().ElementAt(data_index).value = Convert.ToBoolean(value);
                    break;
                case DATATYPE.BYTE:
                    GetBytes().ElementAt(data_index).value = Convert.ToByte(value);
                    break;
                case DATATYPE.FLOAT:
                    GetFloats().ElementAt(data_index).value = Convert.ToSingle(value);
                    break;
                case DATATYPE.INT32:
                    GetInts().ElementAt(data_index).value = Convert.ToInt32(value);
                    break;
                case DATATYPE.UINT32:
                    GetUInts().ElementAt(data_index).value = Convert.ToUInt32(value);
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

            //always resync upon activation
            if (Is_Local)
                FullSync();
        }

        /// <summary>
        /// Core modules cannot be deactivated
        /// </summary>
        public void Deactivate()
        {
            if (Core_Module)
            {
                Utilities.Logger.Error($"Attempted Deactivate in core PlayerModule {Module_Index}");
            }
            else
            {
                Active = false;
            }
        }

        /// <summary>
        /// Called by all. Update fields, then OnUpdate, then OnUpdateLocal.
        /// </summary>
        public void Update()
        {
            if (Active)
            {
                foreach (var d in GetFloats()) { d?.Update(); }
                foreach (var d in GetBools()) { d?.Update(); }
                foreach (var d in GetBytes()) { d?.Update(); }
                foreach (var d in GetInts()) { d?.Update(); }
                foreach (var d in GetUInts()) { d?.Update(); }
                OnUpdate();
                if (Is_Local)
                {
                    OnUpdateLocal();
                }
            }
        }

        /// <summary>
        /// Called by local on each cycle. Triggers sync of syncing data if changed.
        /// </summary>
        public void DoSyncs()
        {
            if (Active)
            {
                if (!Is_Local)
                {
                    Utilities.Logger.Error($"Attempted DoSyncs in non-local PlayerModule {Module_Index}");
                }
                else
                {
                    foreach (var d in GetFloats()) { d?.SyncIfChanged(); }
                    foreach (var d in GetBools()) { d?.SyncIfChanged(); }
                    foreach (var d in GetBytes()) { d?.SyncIfChanged(); }
                    foreach (var d in GetInts()) { d?.SyncIfChanged(); }
                    foreach (var d in GetUInts()) { d?.SyncIfChanged(); }
                }
            }
        }

        public void DoTargetedSyncFromServer(int toWho)
        {
            if (Active)
            {
                if (!LocalData.IS_SERVER)
                {
                    Utilities.Logger.Error($"Attempted DoTargetedSyncFromServer in non-server PlayerModule {Module_Index}");
                }
                else
                {
                    foreach (var d in GetFloats()) { d?.SyncIfSyncs(true, toWho); }
                    foreach (var d in GetBools()) { d?.SyncIfSyncs(true, toWho); }
                    foreach (var d in GetBytes()) { d?.SyncIfSyncs(true, toWho); }
                    foreach (var d in GetInts()) { d?.SyncIfSyncs(true, toWho); }
                    foreach (var d in GetUInts()) { d?.SyncIfSyncs(true, toWho); }
                }
            }
        }

        /// <summary>
        /// Called by local. Triggers sync of syncing data regardless of change.
        /// </summary>
        public void FullSync()
        {
            if (Active)
            {
                if (!Is_Local)
                {
                    Utilities.Logger.Error($"Attempted FullSync in non-local PlayerModule {Module_Index}");
                }
                else
                {
                    foreach (var d in GetFloats()) { d?.SyncIfSyncs(); }
                    foreach (var d in GetBools()) { d?.SyncIfSyncs(); }
                    foreach (var d in GetBytes()) { d?.SyncIfSyncs(); }
                    foreach (var d in GetInts()) { d?.SyncIfSyncs(); }
                    foreach (var d in GetUInts()) { d?.SyncIfSyncs(); }
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

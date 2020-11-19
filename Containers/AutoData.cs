using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ACE.Containers
{
    public enum DATATYPE : byte
    {
        FLOAT,
        BOOL,
        BYTE,
        INT32,
        UINT32,
    }

    public abstract class AutoData<T>
    {
        public T value;
        public T Value_Prior { get; private set; }
        public bool HasChanged { get; private set; }

        public readonly DATATYPE DataType;

        protected readonly bool Syncs, Resets;
        protected readonly T Value_Default;
        protected readonly byte ID;

        protected abstract bool Is_Local();

        public AutoData(byte id, T value_initial, bool syncs = false, bool resets = false)
        {
            ID = id;

            value = value_initial;
            Value_Prior = value_initial;
            Value_Default = value_initial;
            HasChanged = false;

            Syncs = syncs;
            Resets = resets;

            //detect datatype code
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Single:
                    DataType = DATATYPE.FLOAT;
                    break;

                case TypeCode.Boolean:
                    DataType = DATATYPE.BOOL;
                    break;

                case TypeCode.Byte:
                    DataType = DATATYPE.BYTE;
                    break;

                case TypeCode.Int32:
                    DataType = DATATYPE.INT32;
                    break;

                case TypeCode.UInt32:
                    DataType = DATATYPE.UINT32;
                    break;

                default:
                    Syncs = false;
                    Utilities.Logger.Error("Attempted to create AutoData with non-implemented sycning type " + Type.GetTypeCode(typeof(T)));
                    break;
            }
        }

        /// <summary>
        /// To be called by all client+server during PreUpdate
        /// </summary>
        public void PreUpdate()
        {
            DetectChange();
            ResetIfNeeded();
            OnPreUpdate();
        }

        /// <summary>
        /// To be called by all client+server during Update
        /// </summary>
        public void Update()
        {
            OnUpdate();
        }

        private void DetectChange()
        {
            HasChanged = !value.Equals(Value_Prior);
            Value_Prior = value;
            if (HasChanged)
            {
                OnChange();
                if (Is_Local()) OnChangeLocal();
            }
        }

        private void ResetIfNeeded()
        {
            if (Resets)
                value = Value_Default;
        }

        /// <summary>
        /// To be called by local client during SendClientChanges
        /// </summary>
        public void SyncIfChanged()
        {
            if (Syncs && HasChanged && Is_Local())
            {
                Sync();
            }
        }

        /// <summary>
        /// Force sync if data is syncing type (for new player joining, etc.)
        /// Can be sent from non-local if from server
        /// </summary>
        public void SyncIfSyncs(bool from_server = false, int server_to_who = -1)
        {
            if (Syncs)
            {
                if ((Is_Local()) || (from_server && LocalData.IS_SERVER))
                {
                    Sync(from_server, server_to_who);
                }
            }
        }

        /// <summary>
        /// Can be called by AutoData that do not normally sync.
        /// </summary>
        private void Sync(bool from_server = false, int toWho = -1)
        {
            if (!LocalData.IS_SINGLEPLAYER)
            {
                if ((Is_Local()) || (from_server && LocalData.IS_SERVER))
                {
                    DoSync(from_server, toWho);
                }
                else
                {
                    Utilities.Logger.Error("AutoData Sync called by non-local");
                }
            }
        }

        protected virtual void DoSync(bool from_server, int toWho)
        {
            Utilities.Logger.Error("AutoData DoSync not implemented for this object type");
        }

        protected virtual void OnChange() { }
        protected virtual void OnChangeLocal() { }
        protected virtual void OnPreUpdate() { }
        protected virtual void OnUpdate() { }
    }

    public class AutoDataPlayer<T> : AutoData<T>
    {
        public readonly PlayerModule ParentPlayerModule;

        protected override bool Is_Local() => ParentPlayerModule.Is_Local;

        public AutoDataPlayer(PlayerModule parent, byte id, T value_initial, bool syncs = false, bool resets = false) : base(id, value_initial, syncs, resets)
        {
            ParentPlayerModule = parent;
        }

        protected override void DoSync(bool from_server, int toWho)
        {
            int fromWho = from_server ? ParentPlayerModule.ParentPlayerData.ACEPlayer.player.whoAmI : LocalData.WHO_AM_I;
            Utilities.PacketHandler.ClientAutoDataPlayer.Send<T>(toWho, fromWho, ParentPlayerModule.Module_Index, DataType, ID, value);
        }
    }
}

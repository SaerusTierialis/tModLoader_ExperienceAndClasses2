﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace EAC2.Containers
{
    public class AutoData<T>
    {
        public readonly PlayerModule ParentPlayerModule;

        public T value;
        public T Value_Prior { get; private set; }
        public bool HasChanged { get; private set; }

        public readonly PlayerModule.DATATYPE DataType;

        protected readonly bool Syncs, Resets;
        protected readonly T Value_Default;
        protected readonly byte ID;

        protected bool Is_Local => ParentPlayerModule.Is_Local;

        public AutoData(PlayerModule parent, byte id, T value_initial, bool syncs = false, bool resets = false)
        {
            ParentPlayerModule = parent;
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
                    DataType = PlayerModule.DATATYPE.FLOAT;
                    break;

                case TypeCode.Boolean:
                    DataType = PlayerModule.DATATYPE.BOOL;
                    break;

                case TypeCode.Byte:
                    DataType = PlayerModule.DATATYPE.BYTE;
                    break;

                case TypeCode.Int32:
                    DataType = PlayerModule.DATATYPE.INT32;
                    break;

                case TypeCode.UInt32:
                    DataType = PlayerModule.DATATYPE.UINT32;
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
        public void Update()
        {
            DetectChange();
            ResetIfNeeded();
        }

        private void DetectChange()
        {
            HasChanged = !value.Equals(Value_Prior);
            Value_Prior = value;
            if (HasChanged)
            {
                OnChange();
                if (Is_Local) OnChangeLocal();
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
            if (Syncs && HasChanged && Is_Local)
            {
                DoSync();
            }
        }

        /// <summary>
        /// Force sync if data is syncing type (for new player joining, etc.)
        /// </summary>
        public void SyncIfSyncs()
        {
            if (Syncs && Is_Local)
            {
                DoSync();
            }
        }

        /// <summary>
        /// Can be called by AutoData that do not normally sync.
        /// </summary>
        private void DoSync()
        {
            if (Is_Local)
            {
                Utilities.PacketHandler.ClientAutoData.Send<T>(-1, LocalData.WHO_AM_I, ParentPlayerModule.Module_Index, DataType, ID, value);
            }
            else
            {
                Utilities.Logger.Error("AutoData DoSync called by non-local");
            }
        }

        protected virtual void OnChange() { }
        protected virtual void OnChangeLocal() { }
    }

    public class TestValue : AutoData<uint>
    {
        public TestValue(PlayerModule parent, byte id, uint value_initial, bool syncs = false, bool resets = false) : base(parent, id, value_initial, syncs, resets)
        {
        }

        protected override void OnChange()
        {
            string str = $"{ParentPlayerModule.ParentPlayerData.EACPlayer.player.name} ({Is_Local}) value set to {value}";
            if (LocalData.IS_PLAYER)
                Main.NewText(str);
            else
                Console.WriteLine(str);
        }
    }
}
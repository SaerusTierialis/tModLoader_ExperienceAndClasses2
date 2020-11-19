using ACE.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader.IO;

namespace ACE.Systems.PlayerModules
{
    public class AttributeModule : PlayerModule
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ AutoData ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private Dictionary<AutoFloat, AutoDataPlayer<float>> _floats = new Dictionary<AutoFloat, AutoDataPlayer<float>>();
        protected override IEnumerable<AutoDataPlayer<float>> GetFloats() => _floats.Values;
        private enum AutoFloat : byte
        {
        }


        private Dictionary<AutoBool, AutoDataPlayer<bool>> _bools = new Dictionary<AutoBool, AutoDataPlayer<bool>>();
        protected override IEnumerable<AutoDataPlayer<bool>> GetBools() => _bools.Values;
        private enum AutoBool : byte
        {
        }


        private Dictionary<AutoByte, AutoDataPlayer<byte>> _bytes = new Dictionary<AutoByte, AutoDataPlayer<byte>>();
        protected override IEnumerable<AutoDataPlayer<byte>> GetBytes() => _bytes.Values;
        private enum AutoByte : byte
        {
        }


        private Dictionary<AutoInt, AutoDataPlayer<int>> _ints = new Dictionary<AutoInt, AutoDataPlayer<int>>();
        protected override IEnumerable<AutoDataPlayer<int>> GetInts() => _ints.Values;
        private enum AutoInt : byte
        {
            Allocated_Power,
            Bonus_Power,
            //INSERT HERE
        }


        private Dictionary<AutoUInt, AutoDataPlayer<uint>> _uints = new Dictionary<AutoUInt, AutoDataPlayer<uint>>();
        protected override IEnumerable<AutoDataPlayer<uint>> GetUInts() => _uints.Values;
        private enum AutoUInt : byte
        {
        }


        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Other Variables ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private enum ID : byte
        {
            Power,
            //INSERT HERE (order here determines order elsewhere)
        }
        private readonly Dictionary<ID, AttributeContainer> _attributes = new Dictionary<ID, AttributeContainer>();
        public AttributePower Power => (AttributePower)_attributes[ID.Power];

        //allocation
        public int Points_Available { get; private set; } = 0;


        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Constructor ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public AttributeModule(PlayerData parent, byte module_index) : base(parent, module_index, false)
        {
            //any specific init


            //each AutoData must be initialized
            _ints[AutoInt.Bonus_Power] = new AutoDataPlayer<int>(this, (byte)AutoInt.Bonus_Power, 0, false, true); //resets, recalculated by all
            _ints[AutoInt.Allocated_Power] = new AllocationPointData(this, (byte)AutoInt.Allocated_Power, 0, true, false); //syncs
            //INSERT HERE


            //array of attributes in order to present
            _attributes[ID.Power] = new AttributePower(_ints[AutoInt.Allocated_Power], _ints[AutoInt.Bonus_Power]);
            //INSERT HERE


            //check for uninitialized AutoData
            CheckAutoData(Enum.GetNames(typeof(AutoFloat)).Length,
                            Enum.GetNames(typeof(AutoBool)).Length,
                            Enum.GetNames(typeof(AutoByte)).Length,
                            Enum.GetNames(typeof(AutoInt)).Length,
                            Enum.GetNames(typeof(AutoUInt)).Length);

            //init points
            RecalculatePoints();
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Internal Actions ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private int GetPointsAllocated()
        {
            int allocated = 0;
            foreach (AttributeContainer a in _attributes.Values)
            {
                allocated += a.Allocated;
            }
            return allocated;
        }

        private int GetPointsTotal()
        {
            return (4 + (int)ParentPlayerData.Character.Character_Level.value);
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Actions ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public void RecalculatePoints()
        {
            Points_Available = GetPointsTotal() - GetPointsAllocated();
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Overrides ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override void OnPostUpdate()
        {
            foreach (AttributeContainer a in _attributes.Values)
            {
                a.PostUpdate(ParentPlayerData);
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Unique Classes ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public class AllocationPointData : AutoDataPlayer<int>
        {
            public AllocationPointData(PlayerModule parent, byte id, int value_initial, bool syncs = false, bool resets = false) : base(parent, id, value_initial, syncs, resets) { }
            protected override void OnChangeLocal()
            {
                //recalculate available points when local allocation changes
                ParentPlayerModule.ParentPlayerData.Attributes.RecalculatePoints();
            }
        }

        public class AttributePower : AttributeContainer
        {
            private const float DAMAGE_PER_POINT = 0.01f;

            public AttributePower(AutoDataPlayer<int> allocated, AutoDataPlayer<int> bonus) : base(allocated, bonus)
            {
                Name = Utilities.LocalizedText.Get("Attributes.Power_Name");
                Tooltip = Utilities.LocalizedText.Get("Attributes.Power_Tooltip", DAMAGE_PER_POINT * 100);
            }

            protected override void DoEffect(PlayerData player_data, int value)
            {
                player_data.ACEPlayer.player.allDamage += (value * DAMAGE_PER_POINT);
                if (LocalData.IS_PLAYER)
                    Main.NewText($"{player_data.ACEPlayer.player.name} {player_data.ACEPlayer.player.allDamage}");
            }
        }

    }
}

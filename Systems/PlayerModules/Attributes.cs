using ACE.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace ACE.Systems.PlayerModules
{
    public class Attributes : PlayerModule
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
            Final_Power,
        }
        public AutoDataPlayer<int> Allocated_Power => _ints[AutoInt.Allocated_Power];
        public AutoDataPlayer<int> Bonus_Power => _ints[AutoInt.Bonus_Power];
        public AutoDataPlayer<int> Final_Power => _ints[AutoInt.Final_Power];


        private Dictionary<AutoUInt, AutoDataPlayer<uint>> _uints = new Dictionary<AutoUInt, AutoDataPlayer<uint>>();
        protected override IEnumerable<AutoDataPlayer<uint>> GetUInts() => _uints.Values;
        private enum AutoUInt : byte
        {
        }



        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Other Variables ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Constructor ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public Attributes(PlayerData parent, byte module_index) : base(parent, module_index, false)
        {
            //any specific init


            //each AutoData must be initialized
            _ints[AutoInt.Bonus_Power] = new AutoDataPlayer<int>(this, (byte)AutoInt.Bonus_Power, 0, false, true); //resets, recalculated by all
            _ints[AutoInt.Final_Power] = new AttributeFinalPower(this, (byte)AutoInt.Final_Power);
            _ints[AutoInt.Allocated_Power] = new AutoDataPlayer<int>(this, (byte)AutoInt.Allocated_Power, 0, true, false); //syncs


            //check for uninitialized AutoData
            CheckAutoData(Enum.GetNames(typeof(AutoFloat)).Length,
                            Enum.GetNames(typeof(AutoBool)).Length,
                            Enum.GetNames(typeof(AutoByte)).Length,
                            Enum.GetNames(typeof(AutoInt)).Length,
                            Enum.GetNames(typeof(AutoUInt)).Length);
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Actions ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/



        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Overrides ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override void OnPreUpdate()
        {
            _ints[AutoInt.Final_Power].value = Allocated_Power.value + Bonus_Power.value;
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Save/Load ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override TagCompound Save(TagCompound tag)
        {
            return tag;
        }

        public override void Load(TagCompound tag)
        {

        }
    }
}

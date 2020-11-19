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
        }


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


            //check for uninitialized AutoData
            CheckAutoData(Enum.GetNames(typeof(AutoFloat)).Length,
                            Enum.GetNames(typeof(AutoBool)).Length,
                            Enum.GetNames(typeof(AutoByte)).Length,
                            Enum.GetNames(typeof(AutoInt)).Length,
                            Enum.GetNames(typeof(AutoUInt)).Length);
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Actions ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/



        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Overrides ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override void OnUpdate()
        {

        }

        public override void OnUpdateLocal()
        {

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

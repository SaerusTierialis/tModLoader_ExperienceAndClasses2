using EAC2.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace EAC2.Systems.PlayerModules
{
    class Template : PlayerModule
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ AutoData ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        private ArrayByEnum<AutoData<float>, AutoFloat> _floats = new ArrayByEnum<AutoData<float>, AutoFloat>();
        protected override AutoData<float>[] GetFloats() => _floats.Array;
        private enum AutoFloat : byte
        {
        }


        private ArrayByEnum<AutoData<bool>, AutoBool> _bools = new ArrayByEnum<AutoData<bool>, AutoBool>();
        protected override AutoData<bool>[] GetBools() => _bools.Array;
        private enum AutoBool : byte
        {
        }


        private ArrayByEnum<AutoData<byte>, AutoByte> _bytes = new ArrayByEnum<AutoData<byte>, AutoByte>();
        protected override AutoData<byte>[] GetBytes() => _bytes.Array;
        private enum AutoByte : byte
        {
        }


        private ArrayByEnum<AutoData<int>, AutInt> _ints = new ArrayByEnum<AutoData<int>, AutInt>();
        protected override AutoData<int>[] GetInts() => _ints.Array;
        private enum AutInt : byte
        {
        }


        private ArrayByEnum<AutoData<uint>, AutoUInt> _uints = new ArrayByEnum<AutoData<uint>, AutoUInt>();
        protected override AutoData<uint>[] GetUInts() => _uints.Array;
        private enum AutoUInt : byte
        {
        }



        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Other Variables ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Constructor ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public Template(PlayerData parent, byte module_index, bool is_local, bool active = false) : base(parent, module_index, is_local, active)
        {
            //any specific init


            //each AutoData must be initialized


            //check for uninitialized AutoData
            CheckAutoData();
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

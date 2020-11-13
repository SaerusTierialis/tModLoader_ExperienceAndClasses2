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

        private ArrayByEnum<AutoDataPlayer<float>, AutoFloat> _floats = new ArrayByEnum<AutoDataPlayer<float>, AutoFloat>();
        protected override AutoDataPlayer<float>[] GetFloats() => _floats.Array;
        private enum AutoFloat : byte
        {
        }


        private ArrayByEnum<AutoDataPlayer<bool>, AutoBool> _bools = new ArrayByEnum<AutoDataPlayer<bool>, AutoBool>();
        protected override AutoDataPlayer<bool>[] GetBools() => _bools.Array;
        private enum AutoBool : byte
        {
        }


        private ArrayByEnum<AutoDataPlayer<byte>, AutoByte> _bytes = new ArrayByEnum<AutoDataPlayer<byte>, AutoByte>();
        protected override AutoDataPlayer<byte>[] GetBytes() => _bytes.Array;
        private enum AutoByte : byte
        {
        }


        private ArrayByEnum<AutoDataPlayer<int>, AutInt> _ints = new ArrayByEnum<AutoDataPlayer<int>, AutInt>();
        protected override AutoDataPlayer<int>[] GetInts() => _ints.Array;
        private enum AutInt : byte
        {
        }


        private ArrayByEnum<AutoDataPlayer<uint>, AutoUInt> _uints = new ArrayByEnum<AutoDataPlayer<uint>, AutoUInt>();
        protected override AutoDataPlayer<uint>[] GetUInts() => _uints.Array;
        private enum AutoUInt : byte
        {
        }



        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Other Variables ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Constructor ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public Template(PlayerData parent, byte module_index) : base(parent, module_index, false)
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

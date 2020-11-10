using EAC2.Containers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader.IO;

namespace EAC2.Systems.PlayerModules
{
    public class Character : PlayerModule
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
            In_Combat,
        }
        public AutoData<bool> In_Combat => _bools[AutoBool.In_Combat];


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
            Character_Level,
            Character_XP,
        }
        public AutoData<uint> Character_Level => _uints[AutoUInt.Character_Level];
        public AutoData<uint> Character_XP => _uints[AutoUInt.Character_XP];


        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Other Variables ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /// <summary>
        /// Local XP.
        /// </summary>
        private XPLevel local_XPLevel;

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Constructor ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public Character(PlayerData parent, byte module_index) : base(parent, module_index, true)
        {
            //any specific init
            local_XPLevel = new XPLevel(0, 1, 0, Is_Local);

            //each AutoData must be initialized
            _bools[AutoBool.In_Combat] = new AutoData<bool>(this, (byte)AutoBool.In_Combat, false, true);
            _uints[AutoUInt.Character_Level] = new CharacterLevel(this, (byte)AutoUInt.Character_Level, 1, true);
            _uints[AutoUInt.Character_XP] = new TestValue(this, (byte)AutoUInt.Character_XP, 0, true);

            //check for uninitialized AutoData
            CheckAutoData();
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Actions ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public void AddXP(uint xp)
        {
            local_XPLevel.AddXP(xp);
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Overrides ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override void OnUpdateLocal()
        {
            Character_Level.value = local_XPLevel.Level;
            Character_XP.value = local_XPLevel.XP;
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Save/Load ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override TagCompound Save(TagCompound tag)
        {
            tag.Add(Utilities.SaveLoad.TAG_NAMES.CHARACTER_LEVEL, local_XPLevel);
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            local_XPLevel = Utilities.SaveLoad.TagTryGet(tag, Utilities.SaveLoad.TAG_NAMES.CHARACTER_LEVEL, new XPLevel(0, 1, 0, Is_Local));
        }
    }
}

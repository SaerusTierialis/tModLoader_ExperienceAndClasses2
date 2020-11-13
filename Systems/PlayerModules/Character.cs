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

        private ArrayByEnum<AutoDataPlayer<float>, AutoFloat> _floats = new ArrayByEnum<AutoDataPlayer<float>, AutoFloat>();
        protected override AutoDataPlayer<float>[] GetFloats() => _floats.Array;
        private enum AutoFloat : byte
        {
        }


        private ArrayByEnum<AutoDataPlayer<bool>, AutoBool> _bools = new ArrayByEnum<AutoDataPlayer<bool>, AutoBool>();
        protected override AutoDataPlayer<bool>[] GetBools() => _bools.Array;
        private enum AutoBool : byte
        {
            In_Combat,
        }
        public AutoDataPlayer<bool> In_Combat => _bools[AutoBool.In_Combat];


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
            Character_Level,
        }
        public AutoDataPlayer<uint> Character_Level => _uints[AutoUInt.Character_Level];


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
            _bools[AutoBool.In_Combat] = new AutoDataPlayer<bool>(this, (byte)AutoBool.In_Combat, false, true);
            _uints[AutoUInt.Character_Level] = new CharacterLevel(this, (byte)AutoUInt.Character_Level, 1, true);

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

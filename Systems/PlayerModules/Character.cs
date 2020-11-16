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
        
        private Dictionary<AutoFloat, AutoDataPlayer<float>> _floats = new Dictionary<AutoFloat, AutoDataPlayer<float>>();
        protected override IEnumerable<AutoDataPlayer<float>> GetFloats() => _floats.Values;
        private enum AutoFloat : byte
        {
        }


        private Dictionary<AutoBool, AutoDataPlayer<bool>> _bools = new Dictionary<AutoBool, AutoDataPlayer<bool>>();
        protected override IEnumerable<AutoDataPlayer<bool>> GetBools() => _bools.Values;
        private enum AutoBool : byte
        {
            In_Combat,
        }
        public AutoDataPlayer<bool> In_Combat => _bools[AutoBool.In_Combat];


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
            Character_Level,
        }
        public CharacterLevel Character_Level => (CharacterLevel)_uints[AutoUInt.Character_Level];


        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Other Variables ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /// <summary>
        /// Local XP.
        /// </summary>
        public XPLevel local_XPLevel { get; private set; }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Constructor ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public Character(PlayerData parent, byte module_index) : base(parent, module_index, true)
        {
            //any specific init
            local_XPLevel = new XPLevel(0, 1, 0, Is_Local);

            //each AutoData must be initialized
            _bools[AutoBool.In_Combat] = new AutoDataPlayer<bool>(this, (byte)AutoBool.In_Combat, false, true);
            _uints[AutoUInt.Character_Level] = new CharacterLevel(this, (byte)AutoUInt.Character_Level, 1, true);

            //check for uninitialized AutoData
            CheckAutoData(  Enum.GetNames(typeof(AutoFloat)).Length,
                            Enum.GetNames(typeof(AutoBool)).Length,
                            Enum.GetNames(typeof(AutoByte)).Length,
                            Enum.GetNames(typeof(AutoInt)).Length,
                            Enum.GetNames(typeof(AutoUInt)).Length);
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
            tag.Add(Tags.Get(Tags.ID.Character_XPLevel), local_XPLevel);
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            local_XPLevel = Utilities.SaveLoad.TagTryGet(tag, Tags.Get(Tags.ID.Character_XPLevel), new XPLevel(0, 1, 0, Is_Local));
        }
    }
}

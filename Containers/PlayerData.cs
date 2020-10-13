using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace EAC2.Containers
{
    /// <summary>
    /// Contains EAC-related ModPlayer data
    /// </summary>
    public class PlayerData
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Fields ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /// <summary>
        /// Set true during load for local player and during first sync for non-local
        /// </summary>
        public bool Initialized { get; private set; } = false;

        /// <summary>
        /// Defaults to false, set true during load
        /// </summary>
        public bool Is_Local { get; private set; } = false;

        /// <summary>
        /// (LOCAL) TODO currently always false
        /// </summary>
        public bool In_Combat = false;

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ XP ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public XPLevel Character { get; private set; } = new XPLevel(0, 1, 0);

        public void AddXP(uint xp)
        {
            if (!Is_Local)
            {
                Utilities.Logger.Error("Attempted AddXP in non-local PlayerData");
            }
            else
            {
                Character.AddXP(xp);
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Save/Load ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public TagCompound Save(TagCompound tag)
        {
            tag.Add(Utilities.SaveLoad.TAG_NAMES.CHARACTER_LEVEL, Character);
            return tag;
        }

        public void Load(TagCompound tag)
        {
            Is_Local = true;
            Initialized = true;
            Character = Utilities.SaveLoad.TagTryGet(tag, Utilities.SaveLoad.TAG_NAMES.CHARACTER_LEVEL, new XPLevel(0, 1, 0, true));
        }
    }
}

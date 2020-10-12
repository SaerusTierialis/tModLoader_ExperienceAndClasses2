using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAC2.Systems.XPRewards;
using Terraria.ModLoader.IO;

namespace EAC2.Containers
{
    public class XPLevel
    {
        private readonly XP XP_Container;

        public readonly bool Is_Local;
        public readonly byte Tier;

        public uint Level { get; private set; }
        public uint XP_Needed { get; private set; }

        public bool Unlocked { get { return Level > 0; } }
        public uint tLevel { get { return Level + Limits.TIER_tLEVEL_ADJUST[Tier]; } }
        public uint XP { get { return XP_Container.Value; } }
        public bool Maxed { get { return Level == Max_Level; } }
        public uint Max_Level { get { return Limits.TIER_MAX_LEVEL[Tier]; } }

        public XPLevel(byte tier = 0, uint level = 1, uint xp = 0, bool is_local=false)
        {
            Tier = tier;
            Is_Local = is_local;
            Level = level;

            if (Level > Max_Level)
            {
                Utilities.Logger.Error("Initialized XPLevel with level greater than max. Level will be set to max.");
                Level = Max_Level;
            }

            XP_Container = new XP(xp);
            UpdateXPNeeded();
        }

        /// <summary>
        /// Returns true on level-up
        /// </summary>
        /// <param name="xp"></param>
        /// <returns></returns>
        public bool AddXP(uint xp)
        {
            if (!Is_Local)
            {
                Utilities.Logger.Error("Attempted AddXP on non-local XPLevel");
                return false;
            }
            else
            {
                XP_Container.Add(xp);
                return CheckIfLevel();
            }
        }

        public void SubtractXP(uint xp)
        {
            if (!Is_Local)
            {
                Utilities.Logger.Error("Attempted SubtractXP on non-local XPLevel");
            }
            else
            {
                XP_Container.Subtract(xp);
            }
        }

        /// <summary>
        /// Returns true on level-up
        /// </summary>
        /// <param name="xp"></param>
        /// <returns></returns>
        public bool SetXP(uint xp)
        {
            if (!Is_Local)
            {
                Utilities.Logger.Error("Attempted SetXP on non-local XPLevel");
                return false;
            }
            else
            {
                XP_Container.Set(xp);
                return CheckIfLevel();
            }
        }

        /// <summary>
        /// Set new level and reset current XP
        /// </summary>
        /// <param name="level"></param>
        public void SetLevel(uint level)
        {
            if (level > Max_Level)
            {
                Utilities.Logger.Error("Attempted set level above max. Will set level to max instead.");
                Level = Max_Level;
            }
            else
            {
                Level = level;
            }

            XP_Container.Reset();
            UpdateXPNeeded();
            OnLevelChange();
        }

        private bool CheckIfLevel()
        {
            bool leveled = false;

            if (!Is_Local)
            {
                Utilities.Logger.Error("Attempted CheckIfLevel on non-local XPLevel");
            }
            else
            {
                while (!Maxed && (XP >= XP_Needed))
                {
                    XP_Container.Subtract(XP_Needed);
                    Level++;
                    UpdateXPNeeded();
                    leveled = true;
                }

                if (leveled)
                {
                    OnLevelChange();
                }
            }

            return leveled;
        }

        private void UpdateXPNeeded()
        {
            if (!Is_Local)
            {
                Utilities.Logger.Error("Attempted UpdateXPNeeded on non-local XPLevel");
            }
            else if (Maxed)
            {
                XP_Needed = 0;
            }
            else
            {
                XP_Needed = Requirements.XP_PER_tLEVEL[tLevel];
            }
        }

        /// <summary>
        /// Called before AddXP or SettLevel returns
        /// </summary>
        private void OnLevelChange()
        {
            //on any level change, all players recalc xp rate for catchup system
            if (LocalData.IS_PLAYER)
            {
                Systems.XPRewards.Rewards.UpdateXPMultiplier();
            }

            //if local, update ui etc.
            if (Is_Local)
            {
                //TODO
            }
        }

        public class XPLevelSerializer : TagSerializer<XPLevel, TagCompound>
        {
            public override TagCompound Serialize(XPLevel value) => new TagCompound
            {
                ["tier"] = value.Tier,
                ["level"] = value.Level,
                ["xp"] = value.XP,
            };

            public override XPLevel Deserialize(TagCompound tag) => new XPLevel(tag.GetByte("tier"), tag.Get<uint>("level"), tag.Get<uint>("xp"), true);
        }
    }
}

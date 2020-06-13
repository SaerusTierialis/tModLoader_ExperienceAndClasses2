using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAC2.Systems.Local.XP;

namespace EAC2.Containers
{
    public class XPLevel
    {
        private uint LevelAdjust;
        private readonly XP XPContainer;

        public uint Level { get { return tLevel - LevelAdjust; } }
        public uint tLevel { get; private set; }
        public uint Max_tLevel { get; private set; }
        public uint XP { get { return XPContainer.Value; } }
        public uint XP_Needed { get; private set; }

        public XPLevel(uint level_adjust = 0, uint tlevel = 1, uint max_tlevel = Requirements.MAX_tLEVEL, uint xp = 0)
        {
            LevelAdjust = level_adjust;
            tLevel = tlevel;
            Max_tLevel = Math.Min(max_tlevel, Requirements.MAX_tLEVEL);
            XPContainer = new XP(xp);
            UpdateXPNeeded();
        }

        /// <summary>
        /// Returns true on level-up
        /// </summary>
        /// <param name="xp"></param>
        /// <returns></returns>
        public bool AddXP(uint xp)
        {
            XPContainer.Add(xp);
            return CheckIfLevel();
        }

        public void SubtractXP(uint xp)
        {
            XPContainer.Subtract(xp);
        }

        /// <summary>
        /// Returns true on level-up
        /// </summary>
        /// <param name="xp"></param>
        /// <returns></returns>
        public bool SetXP(uint xp)
        {
            XPContainer.Set(xp);
            return CheckIfLevel();
        }

        /// <summary>
        /// Set new tLvl and reset current XP
        /// </summary>
        /// <param name="tlevel"></param>
        public void SettLevel(uint tlevel)
        {
            tLevel = tlevel;
            XPContainer.Reset();
            UpdateXPNeeded();
            OnLevelChange();
        }

        private bool CheckIfLevel()
        {
            bool leveled = false;

            while (XP >= XP_Needed)
            {
                XPContainer.Subtract(XP_Needed);
                tLevel++;
                UpdateXPNeeded();
                leveled = true;
            }

            if (leveled)
            {
                OnLevelChange();
            }

            return leveled;
        }

        private void UpdateXPNeeded()
        {
            XP_Needed = Requirements.XP_PER_tLEVEL[tLevel];
        }

        private void OnLevelChange()
        {
            //TODO
        }
    }
}

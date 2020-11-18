using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace ACE.Utilities
{
    public static class Hotkeys
    {
        public static ModHotKey UI_Toggle { get; private set; }

        public static void Load()
        {
            //DO NOT USE LOCALIZATION TEXT HERE - IT ISN'T AVAILABLE AT THIS STAGE OF LOADING
            UI_Toggle = ACE.MOD.RegisterHotKey("Open/Close UI", "P");
        }
        public static void Unload()
        {
            UI_Toggle = null;
        }

        public static void CheckHotkeys(TriggersSet keys)
        {
            if (UI_Toggle.JustPressed)
            {
                LocalData.UIData.MainUI.ToggleVisibility();
            }
        }
    }
}

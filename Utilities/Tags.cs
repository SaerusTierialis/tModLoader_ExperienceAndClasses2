using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Utilities
{
    public static class Tags
    {
        //IDs can be renamed/reordered without issue
        public enum ID : ushort
        {
            World_Tiles_Placed,
            Character_XPLevel,
            UI_XPOverlay_Location,
            UI_MainUI_Location,
            //ADD HERE
        }

        /// <summary>
        /// These strings must NEVER change - would cause old data to not load. Note that mod name is added automatically.
        /// </summary>
        private static readonly Dictionary<ID, string> _lookup = new Dictionary<ID, string>()
        {
            [ID.World_Tiles_Placed] = "Tiles_Placed",
            [ID.Character_XPLevel] = "Character_XPLevel",
            [ID.UI_XPOverlay_Location] = "UI_XPOverlay_Location",
            [ID.UI_MainUI_Location] = "UI_MainUI_Location",
            //ADD HERE
        };

        static Tags()
        {
            //check for non-set, warn and default to "unknown"
            foreach (ID id in (ID[])Enum.GetValues(typeof(ID)))
            {
                if (!_lookup.ContainsKey(id))
                {
                    Utilities.Logger.Error($"Tag string is not set for {id}. This will fail to save/load correctly.");
                    _lookup[id] = "unknown";
                }
            }
        }

        public static string Get(ID id) => $"{ACE.MOD_NAME}_{_lookup[id]}"; 
    }
}

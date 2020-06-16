using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace EAC2
{
    public class ConfigServer : ModConfig
    {
        public static ConfigServer Instance { get { return GetInstance<ConfigServer>(); } }

        public override ConfigScope Mode => ConfigScope.ServerSide;

        //Rewards
        [Header("$Mods.EAC2.Common.Config_Header_XP")]

        [Label("$Mods.EAC2.Common.Config_XP_Rate_Label")]
        [Range(0f, 10f)]
        [Increment(.05f)]
        [DefaultValue(1f)]
        public float XPRate { get; set; }

        [Label("$Mods.EAC2.Common.Config_XP_Catchup_Label")]
        [Tooltip("$Mods.EAC2.Common.Config_XP_Catchup_Tooltip")]
        [DefaultValue(true)]
        public bool AllowXPCatchup { get; set; }

        //DEBUG
        [Header("$Mods.EAC2.Common.Config_Header_Debug")]

        [Label("$Mods.EAC2.Common.Config_Trace_Label")]
        [Tooltip("$Mods.EAC2.Common.Config_Trace_Tooltip")]
        [DefaultValue(false)]
        public bool Trace { get; set; }

        public override void OnChanged()
        {
            Systems.Local.XP.Rewards.UpdateXPMultiplier();
        }
    }

    public class ConfigClient : ModConfig
    {
        public static ConfigClient Instance { get { return GetInstance<ConfigClient>(); } }

        public override ConfigScope Mode => ConfigScope.ClientSide;

        
    }
}

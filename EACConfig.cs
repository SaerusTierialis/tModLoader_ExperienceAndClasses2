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
    class ConfigServer : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        //Rewards
        [Header("$Mods.ExperienceAndClasses.Common.Config_Header_XP")]

        [Label("$Mods.ExperienceAndClasses.Common.Config_XP_Rate_Label")]
        [Range(0f, 10f)]
        [Increment(.05f)]
        [DefaultValue(1f)]
        public float XPRate { get; set; }

        public override void OnChanged()
        {
            Systems.Local.XPRewards.ClearLoookup();
        }
    }

    class ConfigClient : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        
    }
}

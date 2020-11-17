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
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace ACE
{
    public class ConfigServer : ModConfig
    {
        public static ConfigServer Instance { get { return GetInstance<ConfigServer>(); } }

        public override ConfigScope Mode => ConfigScope.ServerSide;


        //Rewards
        [Header("$Mods.ACE.Common.Config_Header_XP")]

        [Label("$Mods.ACE.Common.Config_XP_Rate_Label")]
        [Range(0f, 10f)]
        [Increment(.05f)]
        [DefaultValue(1f)]
        public float XPRate;

        [Label("$Mods.ACE.Common.Config_XP_Catchup_Label")]
        [Tooltip("$Mods.ACE.Common.Config_XP_Catchup_Tooltip")]
        [DefaultValue(true)]
        public bool AllowXPCatchup;


        //DEBUG
        [Header("$Mods.ACE.Common.Config_Header_Debug")]

        [Label("$Mods.ACE.Common.Config_Trace_Label")]
        [Tooltip("$Mods.ACE.Common.Config_Trace_Tooltip")]
        [DefaultValue(false)]
        public bool Trace;


        public override void OnChanged()
        {
            Systems.XPRewards.Rewards.UpdateXPMultiplier();
        }
    }

    public class ConfigClient : ModConfig
    {
        public static ConfigClient Instance { get { return GetInstance<ConfigClient>(); } }

        public override ConfigScope Mode => ConfigScope.ClientSide;
    

        //UI
        [Header("$Mods.ACE.Common.Config_Header_UI")]

        [Label("$Mods.ACE.Common.Config_XPOverlay_Show_Label")]
        [Tooltip("$Mods.ACE.Common.Config_XPOverlay_Show_Tooltip")]
        [DefaultValue(true)]
        public bool XPOverlay_Show;

        [Label("$Mods.ACE.Common.Config_XPOverlay_Background_Colour_Label")]
        [Tooltip("$Mods.ACE.Common.Config_XPOverlay_Background_Colour_Tooltip")]
        [DefaultValue(typeof(Color), "50, 50, 200, 255"), ColorNoAlpha]
        public Color XPOverlay_Background_Colour;

        [Label("$Mods.ACE.Common.Config_XPOverlay_Transparency_Label")]
        [Tooltip("$Mods.ACE.Common.Config_XPOverlay_XPOverlay_Transparency_Tooltip")]
        [DefaultValue(0.33f)]
        [Range(0f, 1f)]
        public float XPOverlay_Transparency;

        [Label("$Mods.ACE.Common.Config_XPOverlay_Dims_Label")]
        [Tooltip("$Mods.ACE.Common.Config_XPOverlay_Dims_Tooltip")]
        public UIDims XPOverlay_Dims = new UIDims(250, 30);


        public override void OnChanged()
        {
            LocalData.UIData?.ApplyModConfig(Instance);
        }
    }

    public class UIDims
    {
        [Range(1, 5000)]
        public int width = 1;
        [Range(1, 5000)]
        public int height = 1;

        public UIDims() { }

        public UIDims(int w, int h)
        {
            width = w;
            height = h;
        }

        public override bool Equals(object obj)
        {
            if (obj is UIDims other)
                return width == other.width && height == other.height;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return new { width , height }.GetHashCode();
        }
    }
}

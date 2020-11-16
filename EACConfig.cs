﻿using Microsoft.Xna.Framework;
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
        public float XPRate;

        [Label("$Mods.EAC2.Common.Config_XP_Catchup_Label")]
        [Tooltip("$Mods.EAC2.Common.Config_XP_Catchup_Tooltip")]
        [DefaultValue(true)]
        public bool AllowXPCatchup;


        //DEBUG
        [Header("$Mods.EAC2.Common.Config_Header_Debug")]

        [Label("$Mods.EAC2.Common.Config_Trace_Label")]
        [Tooltip("$Mods.EAC2.Common.Config_Trace_Tooltip")]
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
        [Header("$Mods.EAC2.Common.Config_Header_UI")]

        [Label("$Mods.EAC2.Common.Config_XPOverlay_Show_Label")]
        [Tooltip("$Mods.EAC2.Common.Config_XPOverlay_Show_Tooltip")]
        [DefaultValue(true)]
        public bool XPOverlay_Show;

        [Label("$Mods.EAC2.Common.Config_XPOverlay_Vertical_Label")]
        [Tooltip("$Mods.EAC2.Common.Config_XPOverlay_Vertical_Tooltip")]
        [DefaultValue(false)]
        public bool XPOverlay_Vertical;

        [Label("$Mods.EAC2.Common.Config_XPOverlay_Dims_Label")]
        [Tooltip("$Mods.EAC2.Common.Config_XPOverlay_Dims_Tooltip")]
        public UIDims XPOverlay_Dims = new UIDims(250, 10);


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

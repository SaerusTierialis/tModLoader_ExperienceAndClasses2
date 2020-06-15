﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;

namespace EAC2.Utilities
{
    public static class Logger
    {
        public static void Trace(string message)
        {
            if (LocalData.IS_SERVER)
            {
                message = "TRACE from Server: " + message;
                Console.WriteLine(message);
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(message), UI.Constants.COLOUR_MESSAGE_TRACE);
            }
            else
            {
                if (LocalData.IS_CLIENT)
                {
                    message = "TRACE from Client#" + Main.LocalPlayer.whoAmI + ": " + message;
                    Main.NewText("Sending " + message, UI.Constants.COLOUR_MESSAGE_TRACE);
                    PacketHandler.ClientBroadcast.Send(-1, (byte)Main.LocalPlayer.whoAmI, message, PacketHandler.ClientBroadcast.BROADCAST_TYPE.TRACE);
                }
                else
                {
                    Main.NewText("TRACE: " + message, UI.Constants.COLOUR_MESSAGE_TRACE);
                }
            }
            EAC2.MOD.Logger.Debug(message);
        }

        public static void Error(string message)
        {
            message = message + " (please report)";
            if (LocalData.IS_SERVER)
            {
                message = "EAC2-ERROR from Server: " + message;
                Console.WriteLine(message);
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(message), UI.Constants.COLOUR_MESSAGE_ERROR);
            }
            else
            {
                if (LocalData.IS_CLIENT)
                {
                    message = "EAC2-ERROR from Client#" + Main.LocalPlayer.whoAmI + ": " + message;
                    Main.NewText("Sending " + message, UI.Constants.COLOUR_MESSAGE_ERROR);
                    PacketHandler.ClientBroadcast.Send(-1, (byte)Main.LocalPlayer.whoAmI, message, PacketHandler.ClientBroadcast.BROADCAST_TYPE.ERROR);
                }
                else
                {
                    Main.NewText("ExperienceAndClasses-ERROR: " + message, UI.Constants.COLOUR_MESSAGE_TRACE);
                }
            }
            EAC2.MOD.Logger.Error(message);
        }
    }
}

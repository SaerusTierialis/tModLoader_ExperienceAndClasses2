using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;

namespace ACE.Utilities
{
    public static class LocalizedText
    {
        public static string Get(string key) { return Get(key, new object[0]); }
        public static string Get(string key, object arg1) { return Get(key, new object[] { arg1 }); }
        public static string Get(string key, object arg1, object arg2) { return Get(key, new object[] { arg1, arg2 }); }
        public static string Get(string key, object arg1, object arg2, object arg3) { return Get(key, new object[] { arg1, arg2, arg3 }); }
        public static string Get(string key, object[] args)
        {
            return Language.GetTextValue("Mods.ACE." + key, args);
        }
    }
}

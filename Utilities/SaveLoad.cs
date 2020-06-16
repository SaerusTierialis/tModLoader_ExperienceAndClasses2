using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace EAC2.Utilities
{
    class SaveLoad
    {
        public static class TAG_NAMES
        {
            public const string PREFIX = "eac2_";

            //Character
            public const string CHARACTER_LEVEL = PREFIX + "character_level";
        }

        /// <summary>
        /// Try to get from tag, else default to specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tag"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T TagTryGet<T>(TagCompound tag, string key, T defaultValue)
        {
            //new method does not detect if type is wrong
            if ((tag != null) && (tag.ContainsKey(key)))
            {
                try
                {
                    return tag.Get<T>(key);
                }
                catch
                {
                    return defaultValue;
                }
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Set target to read key value if valid and not null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tag"></param>
        /// <param name="key"></param>
        /// <param name="target"></param>
        public static void TagTrySet<T>(TagCompound tag, string key, ref T target)
        {
            if ((tag != null) && (tag.ContainsKey(key)))
            {
                try
                {
                    T value = tag.Get<T>(key);
                    if (value != null)
                    {
                        target = value;
                    }
                }
                catch {}
            }
        }

        public static TagCompound TagAddArrayAsList<T>(TagCompound tag, string name, T[] array)
        {

            //convert to list
            List<T> list = new List<T>();
            foreach (T value in array)
            {
                list.Add(value);
            }

            //add list
            tag.Add(name, list);

            //return
            return tag;
        }

        public static T[] TagLoadListAsArray<T>(TagCompound tag, string name, int length)
        {
            //load list
            List<T> list = TagTryGet<List<T>>(tag, name, new List<T>());

            //warn if list is too long
            if (length < list.Count)
            {
                Utilities.Logger.Error("Error loading " + name + ". Loaded array is too long. Excess entries will be lost.");
            }

            //create array (if list was too long, produce a larger array in case this helps prevent data loss)
            T[] array = new T[Math.Max(length, list.Count)];

            //populate array
            for (int i = 0; i < list.Count; i++)
            {
                array[i] = list[i];
            }

            //return
            return array;
        }
    }
}

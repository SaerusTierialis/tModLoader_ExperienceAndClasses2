using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAC2.Containers
{
    /// <summary>
    /// Contains EAC-related ModPlayer data
    /// </summary>
    public class PlayerData
    {
        /// <summary>
        /// The local player will have this set TRUE during OnEnterWorld
        /// </summary>
        public bool Is_Local = false;

        /// <summary>
        /// (LOCAL) TODO currently always false
        /// </summary>
        public bool In_Combat = false;

    }
}

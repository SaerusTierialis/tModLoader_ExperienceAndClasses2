using EAC2.Utilities;
using Terraria.ModLoader;

namespace EAC2
{
	public class EAC2 : Mod
	{
		public EAC2(){}

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Load/Unload ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public override void Load()
        {
            LocalData.ResetLocalData();
        }

        public override void Unload()
        {
            LocalData.ResetLocalData(); //should help with garbage collection
        }

    }
}
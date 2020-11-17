using ACE.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.IO;

namespace ACE.Containers
{
    public class UIData
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Fields ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public enum UIs : byte
        {
            XPOverlay,
        }

        private bool initialized = false;
        public readonly Preferences PersitentData = new Preferences(Path.Combine(Main.SavePath, "Mod Configs", $"{ACE.MOD_NAME}_UI.json"));
        private bool _prior_inventory_state = false;

        private readonly Dictionary<UIs, UIModule> _modules = new Dictionary<UIs, UIModule>();
        public XPOverlay XPOverlay => (XPOverlay)_modules[UIs.XPOverlay];

        public UIData()
        {
            //load any peristent data that isn't a ModConfig
            PersitentData.Load();

            //init modules
            PopulateModules();

            //complete
            initialized = true;

            void PopulateModules()
            {
                //populate modules...
                _modules[UIs.XPOverlay] = new XPOverlay(this);
                //ADD FUTURE MODULES HERE <------------------------------------

                //warn if any module not set
                foreach (UIs ui in (UIs[])Enum.GetValues(typeof(UIs)))
                {
                    if (_modules[ui] == null)
                        Utilities.Logger.Error($"Did not initialize UIModule {ui}");
                }
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Actions ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public void Update(GameTime time)
        {
            if (initialized)
            {
                if (Main.playerInventory != _prior_inventory_state)
                {
                    _prior_inventory_state = Main.playerInventory;
                    InventoryStateChange();
                }

                foreach (UIModule module in _modules.Values)
                {
                    module?.DoUpdate(time);
                }
            }
        }

        public void Draw()
        {
            if (initialized)
            {
                foreach (UIModule module in _modules.Values)
                {
                    module?.DoDraw();
                }
            }
        }

        public void ApplyModConfig(ConfigClient config)
        {
            if (initialized)
            {
                foreach (UIModule module in _modules.Values)
                {
                    if (module?.Initialized == true)
                        module?.ApplyModConfig(config);
                }
            }
        }

        public void Save()
        {
            if (initialized)
            {
                PersitentData.Clear();
                foreach (UIModule module in _modules.Values)
                {
                    module?.Save();
                }
                PersitentData.Save();
            }
        }

        private void InventoryStateChange()
        {
            if (initialized)
            {
                foreach (UIModule module in _modules.Values)
                    module.OnInventoryStateChange();
            }
        }

    }
}

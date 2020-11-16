using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.IO;
using Terraria.UI;

namespace ACE.Containers
{
    public abstract class UIModule : UIState
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Fields ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public bool Visible { get; private set; } = false;

        protected UserInterface UI;

        private GameTime _time_last_update;
        public bool Initialized { get; private set; } = false;
        protected readonly UIData ParentUIData;

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Constructor ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public UIModule(UIData parent, bool visible = false)
        {
            ParentUIData = parent;

            UI = new UserInterface();
            UI.SetState(this); //set state immediately so that it can init, will be hidden after if not meant to be visible
        }

        public override void OnInitialize()
        {
            RemoveAllChildren();
            DoInitialize();
            ApplyModConfig(ConfigClient.Instance);
            Load();
            UpdateVisibility(Visible);
            Initialized = true;
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Actions ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public void Show()
        {
            Visible = true;
            UI?.SetState(this);
        }

        public void Hide()
        {
            Visible = false;
            UI?.SetState(null);
        }

        public void DoUpdate(GameTime time)
        {
            UI?.Update(time);
            _time_last_update = time;
        }

        public void DoDraw()
        {
            UI?.Draw(Main.spriteBatch, _time_last_update);
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Internal Actions ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        protected void UpdateVisibility(bool visible)
        {
            if (Visible != visible)
            {
                if (visible)
                    Show();
                else
                    Hide();
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Overrides ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        public virtual void ApplyModConfig(ConfigClient config) { }
        public virtual void DoInitialize() { }
        public virtual void Save() { }
        protected virtual void Load() { }

    }
}

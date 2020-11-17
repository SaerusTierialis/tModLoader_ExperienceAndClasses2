using ACE.Containers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using static ACE.UI.Elements;

namespace ACE.UI
{
    public class XPOverlay : UIModule
    {
        private ProgressBarBundle _bar;

        public XPOverlay(UIData parent, bool visible = false) : base(parent, visible) { }

        public override void DoInitialize()
        {
            _bar = new ProgressBarBundle(3);
            Append(_bar);

            _bar.SetColourForeground(0, new Color(0f, 255f, 0f, 0f));
            _bar.SetProgress(1, 25, 100);
            _bar.SetColourForeground(1, new Color(255f, 255f, 0f, 0f));
            _bar.SetProgress(2, 50, 100);
            _bar.SetColourForeground(2, new Color(255f, 0f, 0f, 0f));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _bar?.SetProgress(0, LocalData.LOCAL_PLAYER?.PlayerData.Character.local_XPLevel);
        }

        public override void ApplyModConfig(ConfigClient config)
        {
            UpdateVisibility(config.XPOverlay_Show);

            _bar.Resize(config.XPOverlay_Dims.width, config.XPOverlay_Dims.height);
            _bar.SetColourBackground(config.XPOverlay_Background_Colour);
            _bar.SetTransparency(config.XPOverlay_Transparency);
        }

        protected override void Load()
        {
            (float Left, float Top) location = (500, 0);
            ParentUIData.PersitentData.Get(Tags.Get(Tags.ID.UI_XPOverlay_Location), ref location);
            _bar.Move(location);
        }

        public override void Save()
        {
            ParentUIData.PersitentData.Put(Tags.Get(Tags.ID.UI_XPOverlay_Location), _bar.Location);
        }
    }
}

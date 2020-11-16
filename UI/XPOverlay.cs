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
        private ProgressBar _bar;

        public XPOverlay(UIData parent, bool visible = false) : base(parent, visible) { }

        public override void DoInitialize()
        {
            _bar = new ProgressBar();
            Append(_bar);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _bar?.SetProgressXP(LocalData.LOCAL_PLAYER?.PlayerData.Character.local_XPLevel);
        }

        public override void ApplyModConfig(ConfigClient config)
        {
            UpdateVisibility(config.XPOverlay_Show);

            _bar.Resize(config.XPOverlay_Dims.width, config.XPOverlay_Dims.height);
            _bar.SetVerticalModee(config.XPOverlay_Vertical);
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

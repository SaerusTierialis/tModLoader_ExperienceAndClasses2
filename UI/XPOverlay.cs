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

        private enum ID : byte
        {
            Character_XP,
            Primary_XP,
            Secondary_XP,
            COUNT,
        }

        public XPOverlay(UIData parent, bool visible = false) : base(parent, visible) { }

        public override void DoInitialize()
        {
            _bar = new ProgressBarBundle((uint)ID.COUNT);
            Append(_bar);

            _bar.SetVisibility((uint)ID.Primary_XP, false);
            _bar.SetVisibility((uint)ID.Secondary_XP, false);
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

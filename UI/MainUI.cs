using ACE.Containers;
using ACE.UI.Elements;
using ACE.Utilities;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace ACE.UI
{
    public class MainUI : UIModule
    {
        private const float WIDTH = 600f;
        private const float HEIGHT = 600f;
        private const float HEIGHT_MAIN_TITLE = 30f;
        private const float PADDING = 5f;

        private Draggable _main;
        private TitledPanel _main_panel;

        public MainUI(UIData parent) : base(parent) { }

        public override void DoInitialize()
        {
            _main = new Draggable();
            _main.Resize(WIDTH, HEIGHT);
            Append(_main);

            _main_panel = new TitledPanel(HEIGHT_MAIN_TITLE);
            _main_panel.Resize(WIDTH, HEIGHT);
            _main.Append(_main_panel);
        }

        public override void OnInventoryStateChange()
        {
            Hide();
        }

        protected override void Load()
        {
            (float Left, float Top) location = (80, 100);
            ParentUIData.PersitentData.Get(Tags.Get(Tags.ID.UI_MainUI_Location), ref location);
            _main.Move(location);
        }

        public override void Save()
        {
            ParentUIData.PersitentData.Put(Tags.Get(Tags.ID.UI_MainUI_Location), _main.Location);
        }
    }
}

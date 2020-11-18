using ACE.Containers;
using ACE.UI.Elements;
using ACE.Utilities;

namespace ACE.UI
{
    public class MainUI : UIModule
    {
        private const float WIDTH = 600f;
        private const float HEIGHT = 600f;

        private Draggable _main;

        public MainUI(UIData parent) : base(parent) { }

        public override void DoInitialize()
        {
            _main = new Draggable();
            _main.Resize(WIDTH, HEIGHT);
            Append(_main);

            ACEPanel panel = new ACEPanel();
            panel.Resize(WIDTH, HEIGHT);
            _main.Append(panel);
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

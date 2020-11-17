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
    }
}

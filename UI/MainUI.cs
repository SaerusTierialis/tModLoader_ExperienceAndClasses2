using ACE.Containers;
using ACE.UI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;

namespace ACE.UI
{
    public class MainUI : UIModule
    {
        private const float WIDTH = 600f;
        private const float HEIGHT = 600f;

        private Draggable _main;

        public MainUI(UIData parent) : base(parent)
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

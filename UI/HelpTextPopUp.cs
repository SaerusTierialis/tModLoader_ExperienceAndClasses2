using ACE.Containers;
using ACE.UI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.UI
{
    public class HelpTextPopUp : UIModule
    {
        private const float WIDTH = 200f;
        private const float HEIGHT_TITLE = 30f;

        private HelpPanel _main;

        public HelpTextPopUp(UIData parent) : base(parent) { }

        public override void DoInitialize()
        {
            _main = new HelpPanel(HEIGHT_TITLE, WIDTH);
            Append(_main);

            Visible = true;
        }

        public void Display(ACEElement target, string body, string title = "Help")
        {
            _main.Show(target, body, title);
        }

        public void Stop(ACEElement target)
        {
            _main.Hide(target);
        }
    }
}

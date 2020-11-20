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
        private const float HEIGHT = 500f;
        private const float HEIGHT_MAIN_TITLE = 30f;
        private const float WIDTH_COLUMN = (WIDTH - (PADDING * 3)) / 2;
        private const float HEIGHT_LEVEL = 80f;
        private const float HEIGHT_ATTRIBUTES = HEIGHT - HEIGHT_MAIN_TITLE - (HEIGHT_LEVEL*3) - (PADDING * 3);
        private const float HEIGHT_ABILITIES = 180f;
        private const float HEIGHT_PASSVIES = HEIGHT - HEIGHT_MAIN_TITLE - HEIGHT_ABILITIES - (PADDING * 3);

        private const float PADDING = 5f;

        private const float FONT_SCALE_TITLE = 0.5f;
        private const float FONT_SCALE_SUBTITLE = 0.4f;

        private Draggable _main;
        private TitledPanel _main_panel;

        //placeholders
        private TitledPanel _char_level, _class_level, _subclass_level, _attributes, _abilities, _passives;

        public MainUI(UIData parent) : base(parent) { }

        public override void DoInitialize()
        {
            _main = new Draggable();
            _main.Resize(WIDTH, HEIGHT);
            Append(_main);

            _main_panel = new TitledPanel(HEIGHT_MAIN_TITLE, "Character Sheet", FONT_SCALE_TITLE, true);
            _main_panel.Resize(WIDTH, HEIGHT);
            _main.Append(_main_panel);

            _char_level = new TitledPanel(HEIGHT_MAIN_TITLE, "Character", FONT_SCALE_SUBTITLE, true);
            _char_level.Resize(WIDTH_COLUMN, HEIGHT_LEVEL);
            _char_level.Move(PADDING, PADDING);
            _main_panel.Panel_Body.Append(_char_level);

            _class_level = new TitledPanel(HEIGHT_MAIN_TITLE, "Main Class", FONT_SCALE_SUBTITLE, true);
            _class_level.Resize(WIDTH_COLUMN, HEIGHT_LEVEL);
            _class_level.Move(_char_level.L, _char_level.B);
            _main_panel.Panel_Body.Append(_class_level);

            _subclass_level = new TitledPanel(HEIGHT_MAIN_TITLE, "Subclass", FONT_SCALE_SUBTITLE, true);
            _subclass_level.Resize(WIDTH_COLUMN, HEIGHT_LEVEL);
            _subclass_level.Move(_char_level.L, _class_level.B);
            _main_panel.Panel_Body.Append(_subclass_level);

            _attributes = new TitledPanel(HEIGHT_MAIN_TITLE, "Attributes", FONT_SCALE_SUBTITLE, true);
            _attributes.Resize(WIDTH_COLUMN, HEIGHT_ATTRIBUTES);
            _attributes.Move(_char_level.L, _subclass_level.B + PADDING);
            _main_panel.Panel_Body.Append(_attributes);

            _abilities = new TitledPanel(HEIGHT_MAIN_TITLE, "Abilities", FONT_SCALE_SUBTITLE, true);
            _abilities.Resize(WIDTH_COLUMN, HEIGHT_ABILITIES);
            _abilities.Move(_char_level.R + PADDING, PADDING);
            _main_panel.Panel_Body.Append(_abilities);

            _passives = new TitledPanel(HEIGHT_MAIN_TITLE, "Passives", FONT_SCALE_SUBTITLE, true);
            _passives.Resize(WIDTH_COLUMN, HEIGHT_PASSVIES);
            _passives.Move(_char_level.R + PADDING, _abilities.B + PADDING);
            _main_panel.Panel_Body.Append(_passives);
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

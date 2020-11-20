using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Containers;
using ACE.Textures;
using ACE.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace ACE.UI.Elements
{
    public class ACEElement : UIElement
    {
        public bool visible = true;

        public float W => Width.Pixels;
        public float H => Height.Pixels;
        public float T => Top.Pixels;
        public float L => Left.Pixels;

        public float B => T + H;
        public float R => L + W;

        public (float Left, float Top) Center => (L+(W/2f), T+(H/2f));
        public (float Left, float Top) Location => (L, T);
        public (float Width, float Height) Size => (W, H);

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                base.Draw(spriteBatch);
                OnDraw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            OnUpdate();
        }

        public void Resize((float Width, float Height) size)
        {
            Resize(size.Item1, size.Item2);
        }

        public void Resize(float? width = null, float? height = null)
        {
            if (width != null) Width.Set((float)width, 0);
            if (height != null) Height.Set((float)height, 0);
            OnResize();
            Recalculate();
        }

        public void Move((float Left, float Top) location)
        {
            Move(location.Item1, location.Item2);
        }

        public void Move(float? left = null, float? top = null)
        {
            if (left != null) Left.Set((float)left, 0);
            if (top != null) Top.Set((float)top, 0);
            OnMove();
            Recalculate();
        }

        protected virtual void OnResize() { }
        protected virtual void OnMove() { }
        protected virtual void OnDraw(SpriteBatch spriteBatch) { }
        protected virtual void OnUpdate() { }
    }

    public class RestrictWithinParent : ACEElement
    {
        protected override void OnUpdate()
        {
            Restrict();
        }

        public void Restrict()
        {
            Parent.Recalculate();

            float screen_width = Parent.GetDimensions().Width;
            float screen_height = Parent.GetDimensions().Height;

            float left = L;
            float top = T;

            //check screen right
            if ((left + W) > screen_width)
                left = screen_width - W;

            //check screen left
            if (left < 0f)
                left = 0f;

            //check screen bottom
            if ((top + H) > screen_height)
                top = screen_height - H;

            //check screen top
            if (top < 0f)
                top = 0f;

            //move only if needed
            if ((L != left) || (T != top))
                Move(left, top);
        }
    }

    public class Draggable : RestrictWithinParent
    {
        private bool dragging = false;
        private Vector2 offset;

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            DragStart(evt);
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            DragEnd(evt);
        }

        protected override void OnUpdate()
        {
            //prevent use of hold items
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (dragging)
            {
                Move(Main.mouseX - offset.X, Main.mouseY - offset.Y);
            }
        }

        private void DragStart(UIMouseEvent evt)
        {
            offset = new Vector2(evt.MousePosition.X - L, evt.MousePosition.Y - T);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt)
        {
            if (dragging)
            {
                Vector2 mouse = evt.MousePosition;
                Move(mouse.X - offset.X, mouse.Y - offset.Y);
            }
            dragging = false;
        }

        protected override void OnMove()
        {
            Restrict();
        }
    }

    public class ProgressBar : ACEElement
    {
        private float _value_current;
        private float _value_max;
        private float _progress;
        private Rectangle _rect_bgd = new Rectangle();
        private Rectangle _rect_fgd = new Rectangle();
        private bool _vertical_mode = false;

        public string label = "Experience";
        public Color colour_background = Color.Gray;
        public Color colour_foreground = Color.Green;

        public ProgressBar()
        {
            SetProgress(0, 1);
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.Textures.Get(TextureHandler.ID.Solid), _rect_bgd, colour_background);
            spriteBatch.Draw(Assets.Textures.Get(TextureHandler.ID.Solid), _rect_fgd, colour_foreground);

            if (IsMouseHovering)
                Main.hoverItemName = $"{label}\n{_value_current} / {_value_max} ({_progress * 100}%)";
        }

        public void UpdateRectangles()
        {
            if (Parent != null)
            {
                int l = (int)(Parent.Left.Pixels + L);
                int t = (int)(Parent.Top.Pixels + T);

                if (_vertical_mode)
                {
                    _rect_bgd = new Rectangle(l, t, (int)W, (int)(H * (1 - _progress)));
                    _rect_fgd = new Rectangle(l, t + _rect_bgd.Height, (int)W, (int)(H - _rect_bgd.Height));
                }
                else
                {
                    _rect_fgd = new Rectangle(l, t, (int)(W * _progress), (int)H);
                    _rect_bgd = new Rectangle(l + _rect_fgd.Width, t, (int)(W - _rect_fgd.Width), (int)H);
                }
            }
        }

        public void SetProgress(XPLevel xp)
        {
            SetProgress(xp.XP, xp.XP_Needed);
        }

        public void SetProgress(float current, float max)
        {
            _value_current = current;
            _value_max = max;
            _progress = _value_current / _value_max;
            UpdateRectangles();
        }

        protected override void OnResize()
        {
            _vertical_mode = H > W;
            UpdateRectangles();
        }

        protected override void OnMove()
        {
            UpdateRectangles();
        }
    }

    public class ProgressBarBundle : Draggable
    {
        private readonly ProgressBar[] _bars;
        private readonly uint _number_bars;
        private bool _vertical_mode = false;
        private float _transparency = 0f;

        public ProgressBarBundle(uint number_bars)
        {
            _number_bars = number_bars;
            _bars = new ProgressBar[_number_bars];
            for (uint i = 0; i < _number_bars; i++)
            {
                _bars[i] = new ProgressBar();
                Append(_bars[i]);
            }
        }

        public void SetProgress(uint bar_index, float current, float max)
        {
            if (bar_index < _number_bars)
                _bars[bar_index].SetProgress(current, max);
        }

        public void SetProgress(uint bar_index, XPLevel xp)
        {
            if (bar_index < _number_bars)
                _bars[bar_index].SetProgress(xp.XP, xp.XP_Needed);
        }

        public void SetLabel(uint bar_index, string label)
        {
            if (bar_index < _number_bars)
                _bars[bar_index].label = label;
        }

        public void SetColourBackground(uint bar_index, Color colour)
        {
            if (bar_index < _number_bars)
                _bars[bar_index].colour_background = colour;
            UpdateTransparency();
        }

        public void SetColourForeground(uint bar_index, Color colour)
        {
            if (bar_index < _number_bars)
                _bars[bar_index].colour_foreground = colour;
            UpdateTransparency();
        }

        public void SetColourBackground(Color colour)
        {
            foreach (var bar in _bars)
                bar.colour_background = colour;
            UpdateTransparency();
        }

        public void SetColourForeground(Color colour)
        {
            foreach (var bar in _bars)
                bar.colour_foreground = colour;
            UpdateTransparency();
        }

        public void SetTransparency(float transparency)
        {
            _transparency = transparency;
            UpdateTransparency();
        }

        private void UpdateTransparency()
        {
            foreach (var bar in _bars)
            {
                bar.colour_background.A = (byte)(255 * (1 - _transparency));
                bar.colour_foreground.A = (byte)(255 * (1 - _transparency));
            }
        }

        public void SetVisibility(uint bar_index, bool is_visible)
        {
            if (bar_index < _number_bars)
                _bars[bar_index].visible = is_visible;
        }

        protected override void OnMove()
        {
            RecalculateBars();
        }

        protected override void OnResize()
        {
            _vertical_mode = H > W;
            RecalculateBars();
        }

        private void RecalculateBars()
        {
            float w = W;
            float h = H;
            if (_vertical_mode)
                w /= _number_bars;
            else
                h /= _number_bars;

            float t = h * (_number_bars - 1);
            float l = 0;
            foreach (var bar in _bars)
            {
                bar.Left.Set(l, 0);
                bar.Top.Set(t, 0);

                bar.Resize(w, h);

                if (_vertical_mode)
                    l += w;
                else
                    t -= h;
            }
        }
    }

    /// <summary>
    /// Wrapper for UIPanel that adds ACEElement functionality
    /// </summary>
    public class ACEPanel : ACEElement
    {
        public UIPanel Panel { get; private set; }
        public Color BackgroundColor { get { return Panel.BackgroundColor; } set { Panel.BackgroundColor = value; } }
        public Color BorderColor { get { return Panel.BorderColor; } set { Panel.BorderColor = value; } }

        public ACEPanel()
        {
            Panel = new UIPanel();
            Panel.SetPadding(0);
            base.Append(Panel);
        }

        protected override void OnResize()
        {
            Panel.Width.Set(W, 0f);
            Panel.Height.Set(H, 0f);
            RecalculateChildren();
        }

        public new void Append(UIElement element)
        {
            //Mouse functions break when appending directly to this, append to wrapped panel instead
            Panel.Append(element);
        }
    }

    public class ACEPanelWithHelpText : ACEPanel
    {
        public string text_help_title;
        public string text_help_body;

        /// <summary>
        /// Defaults to transparent background if left null.
        /// </summary>
        /// <param name="colour_background"></param>
        public ACEPanelWithHelpText(string body = "Unknown", string title = "Unknown", Color? colour_background = null)
        {
            text_help_body = body;
            text_help_title = title;

            if (colour_background == null)
                colour_background = Color.Transparent;
            BackgroundColor = (Color)colour_background;
        }

        private void StartHelpText()
        {
            LocalData.UIData.HelpTextPopUp.Display(this, text_help_body, text_help_title);
        }

        public void EndHelpText()
        {
            LocalData.UIData.HelpTextPopUp.Stop(this);
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            StartHelpText();
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
            EndHelpText();
        }
    }

    public class TitledPanel : RestrictWithinParent
    {
        public readonly ACEPanel Panel_Background;
        public readonly ACEPanel Panel_Body;
        public readonly ACEPanelWithHelpText Panel_Title;

        private float _title_height;

        public TitledPanel(float title_height)
        {
            _title_height = title_height;

            Panel_Background = new ACEPanel();
            Append(Panel_Background);

            Panel_Title = new ACEPanelWithHelpText();
            Panel_Background.Append(Panel_Title);

            Panel_Body = new ACEPanel();
            Panel_Body.Panel.BorderColor = Color.Transparent;
            Panel_Body.Panel.BackgroundColor = Color.Transparent;
            Panel_Background.Append(Panel_Body);
        }

        protected override void OnResize()
        {
            Panel_Background.Resize(W, H);
            Panel_Title.Resize(W, _title_height);
            Panel_Body.Resize(W, H - _title_height);
            Panel_Body.Move(0, _title_height);
        }
    }

    public class HelpPanel : TitledPanel
    {
        private float _width;
        private string _title = "Help";
        private string _body = "Default";
        private ACEElement _target = null;

        public HelpPanel(float title_height, float width) : base(title_height)
        {
            visible = false;
            _width = width;
        }

        protected override void OnDraw(SpriteBatch spriteBatch)
        {
            if (_target != null)
            {
                Rectangle r = _target.GetClippingRectangle(spriteBatch);
                Move(r.Left + r.Width, r.Top);
            }
        }

        public void Show(ACEElement target, string body, string title)
        {
            if (target != null)
            {
                _target = target;

                _body = body;
                _title = title;
                UpdateText();

                visible = true;
            }
        }

        public void Hide(ACEElement target)
        {
            if (target == _target)
                Hide();
        }

        public void Hide()
        {
            visible = false;
        }

        private void UpdateText()
        {
            Resize(_width, 100f);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAC2.Containers;
using EAC2.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace EAC2.UI
{
    class Elements
    {
        public class EACElement : UIElement
        {
            public float W => Width.Pixels;
            public float H => Height.Pixels;
            public float T => Top.Pixels;
            public float L => Left.Pixels;

            public (float Left, float Top) Location => (L, T);
            public (float Width, float Height) Size => (W, H);

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
        }

        public class RestrictWithinParent : EACElement
        {
            public override void Update(GameTime gameTime)
            {
                base.Update(gameTime); // don't remove.
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

            public override void Update(GameTime gameTime)
            {
                base.Update(gameTime); // don't remove.

                //prevent use of hold items
                if (ContainsPoint(Main.MouseScreen))
                {
                    Main.LocalPlayer.mouseInterface = true;
                }

                if (dragging)
                {
                    Move(Main.mouseX - offset.X, Main.mouseY - offset.Y);
                    Restrict();
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
        }

        public class ProgressBar : Draggable
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

            protected override void DrawSelf(SpriteBatch spriteBatch)
            {
                if (_vertical_mode)
                {
                    spriteBatch.Draw(Assets.Textures.Get(TextureHandler.ID.Solid), _rect_fgd, colour_foreground);
                    spriteBatch.Draw(Assets.Textures.Get(TextureHandler.ID.Solid), _rect_bgd, colour_background);
                }
                else
                {
                    spriteBatch.Draw(Assets.Textures.Get(TextureHandler.ID.Solid), _rect_bgd, colour_background);
                    spriteBatch.Draw(Assets.Textures.Get(TextureHandler.ID.Solid), _rect_fgd, colour_foreground);
                }

                if (IsMouseHovering)
                    Main.hoverItemName = $"{label}\n{_value_current} / {_value_max} ({_progress * 100}%)";
            }

            private void UpdateRectangles()
            {
                if (_vertical_mode)
                {
                    _rect_bgd = new Rectangle((int)L, (int)T, (int)W, (int)(H * (1 - _progress)));
                    _rect_fgd = new Rectangle((int)L, (int)T, (int)W, (int)H);
                }
                else
                {
                    _rect_bgd = new Rectangle((int)L, (int)T, (int)W, (int)H);
                    _rect_fgd = new Rectangle((int)L, (int)T, (int)(W * _progress), (int)H);
                }
            }

            public void SetProgressXP(XPLevel xp)
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

            public void SetVerticalModee(bool is_vertical)
            {
                _vertical_mode = is_vertical;
                UpdateRectangles();
            }

            protected override void OnResize()
            {
                UpdateRectangles();
            }

            protected override void OnMove()
            {
                UpdateRectangles();
            }
        }
    }
}

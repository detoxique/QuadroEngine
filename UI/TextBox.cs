using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace QuadroEngine.UI
{
    public class TextBox : UI_Element, Drawable
    {
        private RectangleShape Background = new RectangleShape();
        public Text Label = new Text();

        private bool MouseChecked = false, MouseClicked = false;
        public string text = "", TextEnter = "";

        public override void Update(float DeltaTime)
        {
            Label.DisplayedString = text;
            Label.Position = new Vector2f(Position.X + 3, Position.Y + Size.Y / 2 - Label.GetGlobalBounds().Height / 2 - 4);
            Background.Size = Size;
            Background.FillColor = new Color(0, 0, 0, 120);
            Background.Position = Position;
        }

        public override void MouseCheck(MouseMoveEventArgs e)
        {
            if (e.X >= Background.Position.X && e.X <= Background.Position.X + Background.Size.X &&
                e.Y >= Background.Position.Y && e.Y <= Background.Position.Y + Background.Size.Y)
            {
                MouseChecked = true;
            }
            else
            {
                MouseChecked = false;
            }
        }

        public override void MouseClick(MouseButtonEventArgs e)
        {
            if (MouseChecked && e.Button == Mouse.Button.Left)
            {
                MouseClicked = true;
                IsFocused = true;
                Background.OutlineColor = Color.White;
                Background.OutlineThickness = 1;
            }
            else
            {
                MouseClicked = false;
            }

            if (!MouseChecked && e.Button == Mouse.Button.Left)
            {
                IsFocused = false;
                Background.OutlineThickness = 0;
            }
        }

        public override void TextEntered(TextEventArgs e)
        {
            if (IsFocused)
            {
                text += e.Unicode;
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (IsDraw)
            {
                target.Draw(Background);
                target.Draw(Label);
            }
        }
    }
}

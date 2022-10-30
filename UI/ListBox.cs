using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace QuadroEngine.UI
{
    class ListBox : UI_Element, Drawable
    {
        public int CurrentItem = 0;

        public List<Item> Items = new List<Item>();

        private RectangleShape Background = new RectangleShape();
        private RectangleShape ListBackground = new RectangleShape();

        public bool Changed = false, MouseChecked = false, MouseClicked = false;

        private Animator Fade;
        private Button DeployButton;

        public ListBox()
        {
            DeployButton = new Button();
            Fade = new Animator();
            Background.FillColor = new Color(0, 0, 0, 120);
            DeployButton = new Button();
            DeployButton.Size = new Vector2f(25, 25);
        }

        public override void Update(float DeltaTime)
        {
            Background.Position = Position;
            Background.Size = new Vector2f(Size.X - 25, Size.Y);
            DeployButton.Position = new Vector2f(Position.X + Size.X - 25, Position.Y);
            DeployButton.Update(DeltaTime);

            foreach (Item it in Items)
            {
                if (it.Selected)
                {
                    it.DisplayedText.Position = new Vector2f(Position.X + (Size.X - 25) / 2 - it.DisplayedText.GetGlobalBounds().Width / 2,
                        Position.Y + Size.Y / 2 - it.DisplayedText.GetGlobalBounds().Height / 2 - 2);
                }
                if (IsFocused && !it.Selected)
                {

                }
            }

            if (IsFocused)
            {
                Deploy(DeltaTime);
            }
        }

        private void Deploy(float DeltaTime)
        {
            Size = new Vector2f(Size.X - 25, Fade.Lerp(Size.Y, Items.Capacity * 25, DeltaTime));
        }

        public override void MouseCheck(MouseMoveEventArgs e)
        {
            if (e.X >= Position.X && e.X <= Position.X + Size.X &&
                e.Y >= Position.Y && e.Y <= Position.Y + Size.Y)
            {
                MouseChecked = true;
            }
            DeployButton.MouseCheck(e);
        }

        public override void MouseClick(MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left && MouseChecked)
                IsFocused = true;
            else
                IsFocused = false;
            DeployButton.MouseClick(e);
        }

        public override void MouseRelease(MouseButtonEventArgs e)
        {
            DeployButton.MouseRelease(e);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Background);
            target.Draw(ListBackground);
            target.Draw(DeployButton);

            foreach (Item it in Items)
            {
                target.Draw(it.DisplayedText);
            }
        }
    }

    public class Item
    {
        public string Label;
        public Text DisplayedText;
        public bool Selected;
    }
}

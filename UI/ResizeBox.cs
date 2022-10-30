using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;

namespace QuadroEngine.UI
{
    class ResizeBox : UI_Element, Drawable
    {
        private RectangleShape ResizeRect = new RectangleShape();
        private List<RectangleShape> Boxes = new List<RectangleShape>();
        private bool[] MouseHover = new bool[8];
        private bool[] MouseClicked = new bool[8];
        private bool[] GoToMouse = new bool[8];
        private Vector2f MouseDelta = new Vector2f();

        public ResizeBox()
        {
            for (int i = 0; i < 8; i++)
            {
                Boxes.Add(new RectangleShape(new Vector2f(10, 10)));
            }
        }

        public override void Update(float DeltaTime)
        {
            Boxes[0].Position = new Vector2f(Position.X - 5, Position.Y - 5);
            Boxes[1].Position = new Vector2f(Position.X + Size.X / 2 - 5, Position.Y - 5);
            Boxes[2].Position = new Vector2f(Position.X + Size.X - 5, Position.Y - 5);
            Boxes[3].Position = new Vector2f(Position.X + Size.X - 5, Position.Y + Size.Y / 2 - 5);
            Boxes[4].Position = new Vector2f(Position.X + Size.X - 5, Position.Y + Size.Y - 5);
            Boxes[5].Position = new Vector2f(Position.X + Size.X / 2 - 5, Position.Y + Size.Y - 5);
            Boxes[6].Position = new Vector2f(Position.X - 5, Position.Y + Size.Y - 5);
            Boxes[7].Position = new Vector2f(Position.X - 5, Position.Y + Size.Y / 2 - 5);

            foreach (RectangleShape box in Boxes)
            {
                box.OutlineColor = new Color(0, 0, 0, 200);
                box.OutlineThickness = 1;
            }

            ResizeRect.Position = Position;
            ResizeRect.Size = Size;
            ResizeRect.FillColor = Color.Transparent;
            ResizeRect.OutlineColor = new Color(45, 45, 45);
            ResizeRect.OutlineThickness = 2;
        }

        public override void MouseRelease(MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                for (int i = 0; i < MouseClicked.Length; i++)
                {
                    MouseClicked[i] = false;
                    GoToMouse[i] = false;
                    MouseDelta = new Vector2f();
                }
            }
        }

        public override void MouseCheck(MouseMoveEventArgs e)
        {
            for (int i = 0; i < Boxes.Count; i++)
            {
                if (e.X >= Boxes[i].Position.X && e.X <= Boxes[i].Position.X + Boxes[i].Size.X &&
                    e.Y >= Boxes[i].Position.Y && e.Y <= Boxes[i].Position.Y + Boxes[i].Size.Y)
                {
                    MouseHover[i] = true;
                }
                else
                {
                    MouseHover[i] = false;
                }
            }
            if (GoToMouse[0])
            {
                Vector2f deltaSize = new Vector2f(e.X - MouseDelta.X - Position.X, e.Y - MouseDelta.Y - Position.Y);
                Size = new Vector2f(Size.X - deltaSize.X, Size.Y - deltaSize.Y);
                Position = new Vector2f(e.X - MouseDelta.X, e.Y - MouseDelta.Y);
            }
            if (GoToMouse[1])
            {
                Vector2f deltaSize = new Vector2f(0, e.Y - MouseDelta.Y - Position.Y);
                Size = new Vector2f(Size.X, Size.Y - deltaSize.Y);
                Position = new Vector2f(Position.X, e.Y - MouseDelta.Y);
            }
            if (GoToMouse[2])
            {
                Size = new Vector2f(Size.X - Position.X + Size.X - e.X - MouseDelta.X, Size.Y - Position.Y + Size.Y - e.Y - MouseDelta.Y);
            }
        }

        public override void MouseClick(MouseButtonEventArgs e)
        {
            for (int i = 0; i < Boxes.Count; i++)
            {
                if (MouseHover[i] && e.Button == Mouse.Button.Left)
                {
                    MouseClicked[i] = true;
                    GoToMouse[i] = true;
                    MouseDelta = new Vector2f(e.X - Boxes[i].Position.X, e.Y - Boxes[i].Position.Y);
                }
                else
                {
                    MouseClicked[i] = false;
                    GoToMouse[i] = false;
                }
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(ResizeRect);
            foreach (RectangleShape box in Boxes)
            {
                target.Draw(box);
            }
        }
    }
}

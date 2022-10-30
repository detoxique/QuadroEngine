using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace QuadroEngine.UI
{
    public enum Dock
    {
        Top,
        Bottom,
        Left,
        Right,
        None
    };

    public class Window : UI_Element, Drawable
    {
        public new Vector2f Position;

        public Text Name = new Text();

        public List<TextW> Labels = new List<TextW>();
        public List<UI_Element> Elements = new List<UI_Element>();

        public RectangleShape WindowSpace = new RectangleShape();
        public RectangleShape WindowNameRect = new RectangleShape();

        public RectangleShape RollUpIcon = new RectangleShape();
        public Button RollUp = new Button();

        public bool GoToMouse = false;
        public bool RolledUp, Dragable = true, RollUpable = true;
        public Vector2f MouseDelta;

        private Dock Dock = Dock.None;

        public Window()
        {
            IsDraw = true;
            WindowSpace.FillColor = new Color(0, 0, 0, 150);
        }

        public override void Update(float DeltaTime)
        {
            if (!RolledUp)
            {
                WindowSpace.Size = new Vector2f(Size.X, Size.Y + 20);
            }
            else
            {
                WindowSpace.Size = new Vector2f(Size.X, 20);
            }

            WindowNameRect.FillColor = new Color(0, 0, 0, 120);
            WindowNameRect.Size = new Vector2f(Size.X, 20);

            RollUpIcon.Position = new Vector2f(Position.X + Size.X - 16, Position.Y + 8);
            RollUpIcon.Size = new Vector2f(13, 3);

            RollUp.Position = new Vector2f(Position.X + Size.X - 20, Position.Y);
            RollUp.Size = new Vector2f(20, 20);

            RollUp.Update(DeltaTime);

            foreach (TextW tx in Labels)
            {
                if (tx.Attribute == Attribute.None)
                {
                    tx.Position = new Vector2f(Position.X + tx.SurfacePos.X, Position.Y + tx.SurfacePos.Y);
                }
                else if (tx.Attribute == Attribute.CenterHorizontal)
                {
                    tx.Position = new Vector2f(Position.X + Size.X / 2 - tx.GetLocalBounds().Width / 2, Position.Y + tx.SurfacePos.Y);
                }
            }

            foreach (UI_Element el in Elements)
            {
                el.Update(DeltaTime);
                if (el.Attribute == Attribute.None)
                {
                    el.Position = new Vector2f(Position.X + el.SurfacePos.X, Position.Y + el.SurfacePos.Y);
                }
                else if (el.Attribute == Attribute.CenterHorizontal)
                {
                    el.Position = new Vector2f(Position.X + Size.X / 2 - el.Size.X / 2, Position.Y + el.SurfacePos.Y);
                }
            }
            WindowSpace.Position = Position;
            WindowNameRect.Position = Position;
            Name.Position = new Vector2f(Position.X + Size.X / 2 - Name.GetLocalBounds().Width / 2, Position.Y + 2);
        }

        public void SetDock(Dock dock, IntRect dockableArea)
        {
            Dock = dock;
            Dragable = false;
            if (Dock == Dock.Top)
            {
                Position = new Vector2f(dockableArea.Left, dockableArea.Top);
                Size = new Vector2f(dockableArea.Width, Size.Y);
            }
            else if (Dock == Dock.Bottom)
            {
                Position = new Vector2f(dockableArea.Left, dockableArea.Top + dockableArea.Height - Size.Y);
                Size = new Vector2f(dockableArea.Width, Size.Y);
            }
            else if (Dock == Dock.Left)
            {
                Position = new Vector2f(dockableArea.Left, dockableArea.Top);
                Size = new Vector2f(Size.X, dockableArea.Height);
            }
            else if (Dock == Dock.Right)
            {
                Position = new Vector2f(dockableArea.Left + dockableArea.Width - Size.X, dockableArea.Top);
                Size = new Vector2f(Size.X, dockableArea.Height);
            }
        }

        public override void MouseClick(MouseButtonEventArgs e)
        {
            foreach (UI_Element el in Elements)
            {
                el.MouseClick(e);
            }

            RollUp.MouseClick(e);

            if (RollUp.Click && RollUpable)
            {
                RolledUp = !RolledUp;

                foreach (UI_Element el in Elements)
                {
                    el.IsDraw = !el.IsDraw;
                }
            }

            if (e.Button == Mouse.Button.Left && e.X >= WindowNameRect.Position.X && e.X <= WindowNameRect.Position.X + WindowNameRect.Size.X &&
                e.Y >= WindowNameRect.Position.Y && e.Y <= WindowNameRect.Position.Y + WindowNameRect.Size.Y && Dragable)
            {
                GoToMouse = true;
                MouseDelta = new Vector2f(e.X - Position.X, e.Y - Position.Y);
            }

            if (e.Button == Mouse.Button.Left && e.X >= Position.X && e.X <= Position.X + Size.X 
                && e.Y >= Position.Y && e.Y <= Position.Y + Size.Y)
            {
                IsFocused = true;
            }
            else if (e.Button == Mouse.Button.Left && e.X <= Position.X && e.X >= Position.X + Size.X
                && e.Y <= Position.Y && e.Y >= Position.Y + Size.Y)
            {
                IsFocused = false;
            }
        }

        public override void MouseCheck(MouseMoveEventArgs e)
        {
            foreach (UI_Element el in Elements)
            {
                el.MouseCheck(e);
            }

            if (RollUpable)
                RollUp.MouseCheck(e);

            if (GoToMouse)
            {
                Position = new Vector2f(e.X - MouseDelta.X, e.Y - MouseDelta.Y);
            }
        }

        public override void MouseRelease(MouseButtonEventArgs e)
        {
            foreach (UI_Element el in Elements)
            {
                el.MouseRelease(e);
            }

            if (e.Button == Mouse.Button.Left)
            {
                GoToMouse = false;
                RollUp.Click = false;
            }
        }

        public override void MouseWheel(float Delta)
        {
            foreach (UI_Element el in Elements)
            {
                el.MouseWheel(Delta);
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (IsDraw)
            {
                target.Draw(WindowSpace);
                target.Draw(WindowNameRect);
                if (RollUpable)
                {
                    if (RollUp.IsDraw)
                        target.Draw(RollUp);
                    target.Draw(RollUpIcon);
                }
                target.Draw(Name);

                if (!RolledUp)
                {
                    foreach (Text tx in Labels)
                    {
                        target.Draw(tx);
                    }

                    foreach (UI_Element el in Elements)
                    {
                        target.Draw(el);
                    }
                }
            }
        }
    }
}
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace QuadroEngine.UI
{
    public class Button : UI_Element, Drawable
    {
        public RectangleShape Background = new RectangleShape();

        public Text Label = new Text();

        public string Tag = "";
        public delegate void Action();
        public Action action;

        private Animator Fade = new Animator();

        public Sprite Icon;

        public bool MouseHover = false, Click = false, ActionDone = false;
        public bool Rounded = false;

        /// <summary>
        /// Creates a button
        /// </summary>
        public Button()
        {
            Fade.To = 0.1f;
            IsDraw = true;
        }

        /// <summary>
        /// Creates a button
        /// </summary>
        /// <param name="Position">Button position</param>
        /// <param name="Label">Button text</param>
        /// <param name="Size">Button size</param>
        public Button(Vector2f Position, Text Label, Vector2f Size)
        {
            this.Position = Position;
            this.Label = Label;
            this.Size = Size;
            Fade.To = 0.1f;
            IsDraw = true;
        }

        /// <summary>
        /// Creates a button
        /// </summary>
        /// <param name="Label">Button text</param>
        /// <param name="Size">Button size</param>
        /// <param name="SurfacePosition">Button position on the surface(window)</param>
        public Button(Text Label, Vector2f Size, Vector2f SurfacePosition)
        {
            Position = new Vector2f();
            this.Label = Label;
            this.Size = Size;
            SurfacePos = SurfacePosition;
            Fade.To = 0.1f;
            IsDraw = true;
        }

        /// <summary>
        /// Creates a button
        /// </summary>
        /// <param name="Position">Button position</param>
        /// <param name="Label">Button text</param>
        /// <param name="Size">Button size</param>
        /// <param name="SurfacePosition">Button position on the surface(window)</param>
        /// <param name="action">The function that the button performs when it is clicked</param>
        public Button(Vector2f Position, Text Label, Vector2f Size, Vector2f SurfacePosition, Action action)
        {
            this.Position = Position;
            this.Label = Label;
            this.Size = Size;
            SurfacePos = SurfacePosition;
            this.action = action;
            Fade.To = 0.1f;
            IsDraw = true;
        }

        /// <summary>
        /// Updates button parameters
        /// </summary>
        public override void Update(float DeltaTime)
        {
            Background.Position = new Vector2f(Position.X, Position.Y);
            Background.Size = Size;
            if (Icon != null)
                Icon.Position = new Vector2f(Position.X + Size.X / 2 - Icon.TextureRect.Width / 2, Position.Y + Size.Y / 2 - Icon.TextureRect.Height / 2);

            Label.Position = new Vector2f(Background.Position.X + Size.X / 2 - Label.GetLocalBounds().Width / 2,
                Background.Position.Y + Size.Y / 2 - Label.GetLocalBounds().Height / 2);

            if (MouseHover && !Click)
            {
                Background.FillColor = new Color((byte)Fade.Lerp(Background.FillColor.R, 255, DeltaTime), 
                    (byte)Fade.Lerp(Background.FillColor.G, 255, DeltaTime),
                    (byte)Fade.Lerp(Background.FillColor.B, 255, DeltaTime), 
                    (byte)Fade.Lerp(Background.FillColor.A, 100, DeltaTime)); // 100
                Label.FillColor = new Color(20, 20, 20, 255);
            }
            else if (!MouseHover && !Click)
            {
                Background.FillColor = new Color((byte)Fade.Lerp(Background.FillColor.R, 0, DeltaTime),
                    (byte)Fade.Lerp(Background.FillColor.G, 0, DeltaTime),
                    (byte)Fade.Lerp(Background.FillColor.B, 0, DeltaTime),
                    (byte)Fade.Lerp(Background.FillColor.A, 200, DeltaTime)); // 0 0 0 200
                Label.FillColor = Color.White;
            }
            if (Click && MouseHover)
            {
                Background.FillColor = new Color(255, 255, 255, 200);
                Label.FillColor = Color.Black;

                if (action != null && !ActionDone && IsDraw)
                {
                    action.Invoke();
                    ActionDone = true;
                }
            }
        }

        /// <summary>
        /// Updating a cursor position
        /// </summary>
        /// <param name="e"></param>
        public override void MouseCheck(MouseMoveEventArgs e)
        {
            if (e.X >= Position.X && e.X <= Position.X + Size.X &&
                    e.Y >= Position.Y && e.Y <= Position.Y + Size.Y)
            {
                MouseHover = true;
            }
            else
            {
                MouseHover = false;
                Click = false;
            }
        }

        /// <summary>
        /// Mouse click event
        /// </summary>
        /// <param name="e"></param>
        public override void MouseClick(MouseButtonEventArgs e)
        {
            if (MouseHover && e.Button == Mouse.Button.Left)
            {
                Click = true;
            }
            else
            {
                Click = false;
            }
        }

        /// <summary>
        /// Mouse release event
        /// </summary>
        /// <param name="e"></param>
        public override void MouseRelease(MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                Click = false;
                ActionDone = false;
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (IsDraw)
            {
                target.Draw(Background);
                target.Draw(Label);
                if (Icon != null)
                    target.Draw(Icon);
            }
        }
    }
}

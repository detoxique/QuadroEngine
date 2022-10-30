using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace QuadroEngine.UI
{
    public enum TextPosCheckBox
    {
        Right,
        Left
    };

    public class CheckBox : UI_Element, Drawable
    {
        public bool Checked = false, MouseChecked, MouseClicked;

        public RectangleShape BackRect = new RectangleShape();
        public RectangleShape CheckRect = new RectangleShape();
        public Color BackColor = new Color(0, 0, 0, 200);
        public Color ForeColor = Color.White;

        public TextPosCheckBox TextPos = TextPosCheckBox.Right;

        public Text Label = new Text();

        public delegate void Action();
        Action OnCheckBoxChecked;
        Action OnCheckBoxNotChecked;

        private Animator Fade = new Animator();

        private bool OnCheckBoxCheckedDid = false, OnCheckBoxNotCheckedDid = false;

        public CheckBox()
        {
            Fade.To = 0.25f;
        }

        public CheckBox(Vector2f Position, Text Label)
        {
            this.Position = Position;
            this.Label = Label;
            Fade.To = 0.25f;
        }

        public CheckBox(Vector2f Position, Text Label, Vector2f SurfacePos)
        {
            this.Position = Position;
            this.Label = Label;
            this.SurfacePos = SurfacePos;
            Fade.To = 0.1f;
        }

        public CheckBox(Vector2f Position, Text Label, Vector2f SurfacePos, Action OnCheckBoxChecked, Action OnCheckBoxNotChecked)
        {
            this.Position = Position;
            this.Label = Label;
            this.SurfacePos = SurfacePos;
            this.OnCheckBoxChecked = OnCheckBoxChecked;
            this.OnCheckBoxNotChecked = OnCheckBoxNotChecked;
            Fade.To = 0.1f;
        }

        public override void Update(float DeltaTime)
        {
            CheckRect.Origin = new Vector2f(CheckRect.Size.X / 2, CheckRect.Size.Y / 2);
            BackRect.Size = new Vector2f(20, 20);
            if (!MouseChecked)
            {
                BackRect.FillColor = new Color((byte)Fade.Lerp(BackRect.FillColor.R, 0, DeltaTime),
                    (byte)Fade.Lerp(BackRect.FillColor.G, 0, DeltaTime),
                    (byte)Fade.Lerp(BackRect.FillColor.B, 0, DeltaTime),
                    (byte)Fade.Lerp(BackRect.FillColor.A, 150, DeltaTime)); // 0 0 0 200
            }
            else
            {
                BackRect.FillColor = new Color((byte)Fade.Lerp(BackRect.FillColor.R, 255, DeltaTime),
                    (byte)Fade.Lerp(BackRect.FillColor.G, 255, DeltaTime),
                    (byte)Fade.Lerp(BackRect.FillColor.B, 255, DeltaTime),
                    (byte)Fade.Lerp(BackRect.FillColor.A, 120, DeltaTime)); // 255 255 255 120
            }

            if (TextPos == TextPosCheckBox.Right)
            {
                Label.Position = new Vector2f(Position.X + 25, Position.Y + 10 - Label.GetLocalBounds().Height / 2); 
                BackRect.Position = new Vector2f(Position.X, Position.Y);
                CheckRect.Position = new Vector2f(Position.X + BackRect.Size.X / 2, Position.Y + BackRect.Size.Y / 2);
            }
            else
            {
                Label.Position = new Vector2f(Position.X, Position.Y);
                BackRect.Position = new Vector2f(Position.X + Label.GetLocalBounds().Width + 5, Position.Y);
                CheckRect.Position = new Vector2f(Position.X + BackRect.Size.X / 2, Position.Y + BackRect.Size.Y / 2);
            }

            if (Checked)
            {
                CheckRect.Size = Fade.Lerp(CheckRect.Size, new Vector2f(12, 12), DeltaTime);
                CheckRect.FillColor = ForeColor;
                if (!OnCheckBoxCheckedDid)
                {
                    OnCheckBoxChecked.Invoke();
                    OnCheckBoxCheckedDid = true;
                }
            }
            else
            {
                CheckRect.Size = Fade.Lerp(CheckRect.Size, new Vector2f(0, 0), DeltaTime);
                CheckRect.FillColor = ForeColor;
                if (!OnCheckBoxNotCheckedDid)
                {
                    OnCheckBoxNotChecked.Invoke();
                    OnCheckBoxNotCheckedDid = true;
                }
            }
            Size = new Vector2f(25 + Label.GetLocalBounds().Width, 20);
        }

        public override void MouseCheck(MouseMoveEventArgs e)
        {
            if (e.X >= BackRect.Position.X && e.X <= BackRect.Position.X + BackRect.Size.X &&
                e.Y >= BackRect.Position.Y && e.Y <= BackRect.Position.Y + BackRect.Size.Y)
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
                Checked = !Checked;
                OnCheckBoxCheckedDid = false;
                OnCheckBoxNotCheckedDid = false;
            }
        }

        public override void MouseRelease(MouseButtonEventArgs e)
        {
            MouseClicked = false;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(BackRect);
            target.Draw(Label);
            target.Draw(CheckRect);
        }
    }
}

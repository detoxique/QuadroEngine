using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace QuadroEngine.UI
{
    public class Notification : UI_Element, Drawable
    {
        private Window notice = new Window();
        public Vector2f StartPosition = new Vector2f();
        public Vector2f EndPosition = new Vector2f();

        private RectangleShape Background = new RectangleShape();

        private Animator Fade = new Animator();
        private Animator FadeBack = new Animator();

        public Font CurrentFont;
        public TextW NoticeText = new TextW();

        public bool Closing = false, Opened = false, Starting = true, Closed = false;


        public Notification(string message, Font font, Vector2f size)
        {
            Background.Size = size;
            Background.FillColor = new Color(0, 0, 0, 0);
            notice.Dragable = false;
            NoticeText.Attribute = Attribute.CenterHorizontal;
            NoticeText.CharacterSize = 14;
            NoticeText.DisplayedString = message;
            NoticeText.Font = font;
            NoticeText.SurfacePos = new Vector2f(0, 25);
            notice.Labels.Add(NoticeText);
            notice.Size = new Vector2f(250, 100);
            notice.Elements.Add(new Button(new Vector2f(), 
                new Text("OK", font, 14), 
                new Vector2f(20, 20), 
                new Vector2f(), Close));
            Fade.To = 0.5f;
            FadeBack.To = 0.25f;
            notice.Size = new Vector2f(NoticeText.GetLocalBounds().Width + 40, NoticeText.GetLocalBounds().Height + 40);
            EndPosition = new Vector2f(size.X / 2 - notice.Size.X / 2, size.Y / 2 - notice.Size.Y / 2);
            StartPosition = new Vector2f(size.X / 2 - notice.Size.X / 2, size.Y + 700);
            notice.Position = StartPosition;
        }

        public Notification(string message, string buttonMessage, Font font, Vector2f size)
        {
            Background.Size = size;
            Background.FillColor = new Color(0, 0, 0, 0);
            notice.Dragable = false;
            NoticeText.Attribute = Attribute.CenterHorizontal;
            NoticeText.CharacterSize = 14;
            NoticeText.DisplayedString = message;
            NoticeText.Font = font;
            NoticeText.SurfacePos = new Vector2f(0, 25);
            notice.Labels.Add(NoticeText);
            notice.Size = new Vector2f(250, 100);
            notice.Elements.Add(new Button(new Vector2f(),
                new Text(buttonMessage, font, 14),
                new Vector2f(new Text(buttonMessage, font, 14).GetLocalBounds().Width, 20),
                new Vector2f(), Close));
            Fade.To = 0.5f;
            FadeBack.To = 0.25f;
            notice.Size = new Vector2f(NoticeText.GetLocalBounds().Width + 40, NoticeText.GetLocalBounds().Height + 40);
            EndPosition = new Vector2f(size.X / 2 - notice.Size.X / 2, size.Y / 2 - notice.Size.Y / 2);
            StartPosition = new Vector2f(size.X / 2 - notice.Size.X / 2, size.Y + 700);
            notice.Position = StartPosition;
        }

        private void Close()
        {
            Closing = true;
        }

        public override void Update(float dtime)
        {
            notice.Update(dtime);
            if (!Opened && !Closed)
            {
                Background.FillColor = new Color(0, 0, 0, (byte)FadeBack.Lerp(Background.FillColor.A, 150, dtime));
                notice.Position = Fade.Lerp(notice.Position, EndPosition, dtime);
            }
            if (notice.Position == EndPosition)
            {
                Opened = true;
            }
            if (Closing && !Closed)
            {
                notice.Position = Fade.Lerp(notice.Position, StartPosition, dtime);
                Background.FillColor = new Color(0, 0, 0, (byte)FadeBack.Lerp(Background.FillColor.A, -110, dtime));
            }
            if (Closing && Background.FillColor.A == 0)
            {
                Closed = true;
                Closing = false;
            }
        }

        public override void MouseCheck(MouseMoveEventArgs e)
        {
            notice.MouseCheck(e);
        }

        public override void MouseClick(MouseButtonEventArgs e)
        {
            notice.MouseClick(e);
        }

        public override void MouseRelease(MouseButtonEventArgs e)
        {
            notice.MouseRelease(e);
        }

        public override void Resized(SizeEventArgs e)
        {
            notice.Resized(e);
            if (Closed)
                notice.Position = new Vector2f(notice.Position.X, e.Height + 100);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Background);
            target.Draw(notice);
        }
    }
}

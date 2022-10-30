using SFML.Graphics;
using SFML.System;

namespace QuadroEngine.UI
{
    public class RoundedRectangleShape : Transformable, Drawable
    {
        public Vector2f Size;
        public float Round;

        private RectangleShape vertShape = new RectangleShape();
        private RectangleShape horizShape = new RectangleShape();

        private CircleShape circLeftUp = new CircleShape();
        private CircleShape circLeftDown = new CircleShape();
        private CircleShape circRightUp = new CircleShape();
        private CircleShape circRightDown = new CircleShape();

        public Color FillColor;

        public RoundedRectangleShape(Vector2f PositionRect, Vector2f Size, float Roundness, Color FillColor)
        {
            Position = PositionRect;
            this.Size = Size;
            Round = Roundness;
            this.FillColor = FillColor;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            circLeftUp.Position = Position;
            circLeftUp.Radius = Round; // Left Upper Corner
            circLeftUp.FillColor = FillColor;

            circLeftDown.Position = new Vector2f(Position.X, Position.Y + Size.Y - Round * 2);
            circLeftDown.Radius = Round; // Left Down Corner
            circLeftDown.FillColor = FillColor;

            circRightUp.Position = new Vector2f(Position.X + Size.X - Round * 2, Position.Y);
            circRightUp.Radius = Round; // Right Upper Corner
            circRightUp.FillColor = FillColor;

            circRightDown.Position = new Vector2f(Position.X + Size.X - Round * 2, Position.Y + Size.Y - Round * 2);
            circRightDown.Radius = Round; // Right Down Corner
            circRightDown.FillColor = FillColor;

            vertShape.Position = new Vector2f(Position.X + Round, Position.Y);
            vertShape.Size = new Vector2f(Size.X - Round * 2, Size.Y);
            vertShape.FillColor = FillColor;

            horizShape.Position = new Vector2f(Position.X, Position.Y + Round);
            horizShape.Size = new Vector2f(Size.X, Size.Y - Round * 2);
            horizShape.FillColor = FillColor;

            target.Draw(circLeftUp);
            target.Draw(circLeftDown);
            target.Draw(circRightDown);
            target.Draw(circRightUp);

            target.Draw(vertShape);
            target.Draw(horizShape);
        }
    }
}

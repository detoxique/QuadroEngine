using SFML.Graphics;
using SFML.System;
using SFML.Window;
using VelcroPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VelcroPhysics.Utilities;
using VelcroPhysics.Factories;

namespace QuadroEngine
{
    public enum ShapeType
    {
        Rectangle,
        Circle,
        Polygon
    };

    public class GameObject : Drawable
    {
        public Vector2f Position { get; set; }
        public Vector2f WorldPosition { get; set; }
        public Vector2f Velocity { get; set; }
        public Vector2f Size { get; set; }
        public Vector2f Gravity { get; set; }
        public Sprite sprite { get; set; }
        public Shape shape { get; set; }
        public Body body { get; set; }

        public float Restiution, Radius, Mass;

        public bool IsStatic { get; set; }
        public bool IsOnGround { get; set; }

        public delegate void IsCollides();

        public event IsCollides OnCollision;

        public bool IsEditable { get; set; }

        public bool Clicked { get; set; }

        public GameObject()
        {
            
        }

        public virtual void Update(float DeltaTime, Entity Camera)
        {
            WorldPosition = new Vector2f(ConvertUnits.ToDisplayUnits(body.Position).X, ConvertUnits.ToDisplayUnits(body.Position).Y);

            Position = WorldPosition - Camera.Position;

            if (sprite != null)
            {
                sprite.Position = Position;
                sprite.Rotation = MathHelper.ToDegrees(body.Rotation);
            }

            if (shape != null)
            {
                shape.Position = Position;
            }
        }

        public void CallOnCollision()
        {
            OnCollision?.Invoke();
        }

        public void MoveToMouse(MouseMoveEventArgs e, Entity Camera)
        {
            if (IsEditable && Clicked)
            {
                body.ApplyLinearImpulse(new Vector2(e.X - body.Position.X, e.Y - body.Position.Y));
            }
        }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            if (sprite != null)
            {
                target.Draw(sprite, states);
            }
            if (shape != null)
            {
                target.Draw(shape, states);
            }
        }
    }
}

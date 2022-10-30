using SFML.Graphics;
using SFML.System;

namespace QuadroEngine
{
    public class Entity : Transformable, Drawable
    {
        public Vector2f Size = new Vector2f();
        public Vector2f Velocity = new Vector2f();

        public Entity()
        {

        }
        public Entity(float X, float Y)
        {
            Position = new Vector2f(X, Y);
        }

        public virtual void Update(float DeltaTime)
        {

        }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            
        }
    }
}

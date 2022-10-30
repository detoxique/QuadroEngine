using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace QuadroEngine.UI
{
    public class UI_Element : Transformable, Drawable
    {
        public bool IsFocused { get; internal set; }
        public bool IsDraw { get; internal set; }
        public Vector2f SurfacePos { get; internal set; }
        public Attribute Attribute { get; internal set; }
        public Vector2f Size { get; internal set; }

        public virtual void Update(float DeltaTime)
        {

        }

        public virtual void MouseCheck(MouseMoveEventArgs e)
        {

        }

        public virtual void MouseClick(MouseButtonEventArgs e)
        {

        }

        public virtual void MouseRelease(MouseButtonEventArgs e)
        {

        }

        public virtual void TextEntered(TextEventArgs e)
        {

        }

        public virtual void KeyPressed(KeyEventArgs e)
        {

        }

        public virtual void MouseWheel(float Delta)
        {

        }

        public virtual void Resized(SizeEventArgs e)
        {

        }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            
        }
    }
}

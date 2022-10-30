using System.Collections.Generic;
using SFML.Graphics;

namespace QuadroEngine
{
    class Drawing_layers : Transformable, Drawable
    {
        public List<Layer> Layers = new List<Layer>();

        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (Layer ly in Layers)
            {
                target.Draw(ly);
            }
        }
    }
}

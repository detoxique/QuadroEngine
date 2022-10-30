using System.Collections.Generic;
using SFML.Graphics;

namespace QuadroEngine
{
    public class Layer : Transformable, Drawable
    {
        public string Name;

        public int Priority = 0;

        public List<Drawable> Objects = new List<Drawable>();

        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (Drawable obj in Objects)
            {
                target.Draw(obj);
            }
        }
    }
}

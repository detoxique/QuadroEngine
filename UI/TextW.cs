using SFML.Graphics;
using SFML.System;

namespace QuadroEngine.UI
{
    public enum Attribute
    {
        None,
        CenterHorizontal
    };
    public class TextW : Text
    {
        public Vector2f SurfacePos;
        public Attribute Attribute;
    }
}

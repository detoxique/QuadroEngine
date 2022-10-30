using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;

namespace QuadroEngine.UI
{
    public class ItemList : UI_Element
    {
        private RectangleShape Background;
        private Text Label;
        public List<ImageItem> Items = new List<ImageItem>();

        public void Add(Sprite image, Text label)
        {
            Items.Add(new ImageItem
            {
                img = image,
                label = label
            });
        }
        public override void Update(float DeltaTime)
        {
            
        }
        public override void MouseCheck(MouseMoveEventArgs e)
        {
            
        }
        public override void MouseClick(MouseButtonEventArgs e)
        {
            
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            
        }
    }
    public class ImageItem
    {
        public Sprite img;
        public Text label;

        
    }
}

using SFML.Graphics;
using System.Collections.Generic;
using SFML.System;

namespace QuadroEngine.UI
{
    public enum TextEffect
    {
        None,
        Shake,
        AnimatedGradient
    };

    public struct Gradient
    {
        public Color Color1;
        public Color Color2;

        public Gradient(Color a, Color b)
        {
            Color1 = a;
            Color2 = b;
        }
    }

    public class TextAnimated : Transformable, Drawable
    {
        private Text[] Symbols;

        public float SymbolSpace = 2f;
        public string DisplayedString = "";
        private Font myFont;
        private float myCharacterSize = 0;

        private Gradient grad = new Gradient();

        public TextAnimated()
        {

        }

        public void SetGradient(Gradient gradient)
        {
            grad = gradient;
        }

        /// <summary>
        /// Creates an animated text
        /// </summary>
        /// <param name="text"></param>
        public TextAnimated(string text, Font font, float CharacterSize)
        {
            myFont = font;
            myCharacterSize = CharacterSize;
            char[] s;
            s = text.ToCharArray();
            Symbols = new Text[s.Length];
            for (int i = 0; i < text.Length; i++)
            {
                Symbols[i] = new Text(s[i].ToString(), font, (uint)CharacterSize);
            }
        }

        public FloatRect GetGlobalBounds()
        {
            Text sample = new Text(DisplayedString, myFont, (uint)myCharacterSize);
            return sample.GetGlobalBounds();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            for (int i = 0; i < DisplayedString.Length; i++)
            {
                Symbols[i].DisplayedString = DisplayedString[i].ToString();
            }

            for (int i = 0; i < Symbols.Length; i++)
            {
                Symbols[i].Position = Position;

                Symbols[i].FillColor = Color.White;

                target.Draw(Symbols[i]);
            }
        }

        private int Map(int value, int From1, int From2, int To1, int To2)
        {
            return (value - From1) / (From2 - From1) * (To2 - To1) + To1;
        }
    }
}

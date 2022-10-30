using SFML.System;

namespace QuadroEngine
{
    public class Animator
    {
        public float From, To, Speed, Time;

        public float Value;

        public Animator()
        {

        }

        public Animator(int from, int to, int time)
        {
            From = from;
            To = to;
            Time = time;
        }

        private int Map(int value, int From1, int From2, int To1, int To2)
        {
            return (value - From1) / (From2 - From1) * (To2 - To1) + To1;
        }

        private float Mapf(float value, float From1, float From2, float To1, float To2)
        {
            return (value - From1) / (From2 - From1) * (To2 - To1) + To1;
        }

        public float Map01(float a)
        {
            return Mapf(a, 0, To, 0, 1);
        }

        public Vector2f Lerp(Vector2f a, Vector2f b, float t)
        {
            t = Map01(t);
            if (t >= 1)
            {
                return b;
            }
            else
                return new Vector2f(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);
        }

        public float Lerp(float a, float b, float t)
        {
            t = Map01(t);
            if (t >= 1)
            {
                return b;
            }
            else
                return (a + (b - a) * t);
        }
    }
}

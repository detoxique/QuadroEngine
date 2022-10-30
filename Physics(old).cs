using System;
using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;

namespace QuadroEngine
{
    public struct AABB_old
    {
        public Vector2f Min;
        public Vector2f Max;

        public AABB_old(Vector2f min, Vector2f max)
        {
            Min = min;
            Max = max;
        }
    };

    public struct Circle_old
    {
        public float Radius;
        public Vector2f Position;
    };

    public struct Manifold_old
    {
        public GameObject A;
        public GameObject B;
        public float penetration;
        public Vector2f normal;
    };

    public enum ColliderType_old
    {
        AABB,
        Circle,
    };

    public static class Physics_old
    {
        public static bool AABBandAABBSimple(AABB_old a, AABB_old b)
        {
            // Выходим без пересечения, потому что найдена разделяющая ось
            if (a.Max.X < b.Min.X || a.Min.X > b.Max.X)
                return false;
            if (a.Max.Y < b.Min.Y || a.Min.Y > b.Max.Y)
                return false;

            // Разделяющая ось не найдена, поэтому существует по крайней мере одна пересекающая ось
            return true;
        }

        public static Manifold_old AABBvsAABB(Manifold_old m)
        {
            Vector2f n = m.B.WorldPosition - m.A.WorldPosition;

            AABB_old abox = m.A.aabb;
            AABB_old bbox = m.B.aabb;

            // Вычисление половины ширины вдоль оси x для каждого объекта
            float a_extent = (abox.Max.X - abox.Min.X) / 2;
            float b_extent = (bbox.Max.X - bbox.Min.X) / 2;

            // Вычисление наложения по оси x
            float x_overlap = a_extent + b_extent - Math.Abs(n.X);

            float a_extenty = (abox.Max.Y - abox.Min.Y) / 2;
            float b_extenty = (bbox.Max.Y - bbox.Min.Y) / 2;

            // Вычисление наложения по оси y
            float y_overlap = a_extenty + b_extenty - Math.Abs(n.Y);

            Manifold_old res = new Manifold_old
            {
                A = m.A,
                B = m.B
            };
            // Проверка SAT по оси x
            if (x_overlap > 0 && y_overlap > 0)
            {
                // Определяем, по какой из осей проникновение наименьшее
                if (x_overlap < y_overlap)
                {
                    // Указываем в направлении B, зная, что n указывает в направлении от A к B
                    if (n.X < 0)
                        res.normal = new Vector2f(-1, 0);
                    else
                        res.normal = new Vector2f(0, 0);
                    res.penetration = x_overlap;
                    return res;
                }
                else
                {
                    // Указываем в направлении B, зная, что n указывает в направлении от A к B
                    if (n.Y < 0)
                        res.normal = new Vector2f(0, -1);
                    else
                        res.normal = new Vector2f(0, 1);
                    res.penetration = y_overlap;
                    return res;
                }
            }
            else
            {
                res.penetration = 0;
                return res;
            }
        }

        public static bool CirclevsCircleSimple(Circle_old a, Circle_old b)
        {
            float r = a.Radius + b.Radius;
            r *= r;
            float res = (a.Position.X + b.Position.X) * (a.Position.X + b.Position.X) + (a.Position.Y + b.Position.Y) * (a.Position.Y + b.Position.Y);
            return r < res;
        }

        public static Manifold_old CirclevsCircle(Manifold_old m)
        {
            Manifold_old res = new Manifold_old();
            res.A = m.A;
            res.B = m.B;

            Vector2f n = m.B.WorldPosition - m.A.WorldPosition;

            float r = m.A.Radius + m.B.Radius;
            r *= r;

            if (DotProduct(n, n) > r)
            {
                res.penetration = 0;
                return res;
            }

            float d = (float)Math.Sqrt(n.X * (double)n.Y);

            if (d != 0)
            {
                res.penetration = r - d;

                res.normal = m.B.Velocity - m.A.Velocity;
                return res;
            }
            else
            {
                res.penetration = m.A.Radius;
                res.normal = new Vector2f(1, 0);
                return res;
            }
        }

        public static Manifold_old AABBvsCircle(Manifold_old m)
        {
            Manifold_old res = new Manifold_old();
            res.A = m.A;
            res.B = m.B;

            Vector2f n = res.B.WorldPosition - res.A.WorldPosition;
            Vector2f closest = n;

            float x_extent = (res.A.aabb.Max.X - res.A.aabb.Min.X) / 2;
            float Y_extent = (res.A.aabb.Max.Y - res.A.aabb.Min.Y) / 2;

            closest.X = (-x_extent).Clamp(x_extent, closest.X);
            closest.Y = (-Y_extent).Clamp(Y_extent, closest.Y);

            bool inside = false;

            // Окружность внутри AABB, поэтому нам нужно ограничить центр окружности
            // до ближайшего ребра
            if (n == closest)
            {
                inside = true;

                if (Math.Abs(n.X) > Math.Abs(n.Y))
                {
                    // Отсекаем до ближайшей ширины
                    if (closest.X > 0)
                        closest.X = x_extent;
                    else
                        closest.X = -x_extent;
                }
                else
                {
                    // Отсекаем до ближайшей ширины
                    if (closest.Y > 0)
                        closest.Y = Y_extent;
                    else
                        closest.Y = -Y_extent;
                }
            }

            Vector2f normal = n - closest;
            float d = (float)Math.Sqrt(normal.X * normal.X + normal.Y * normal.Y);
            float r = res.B.Radius;

            if (d > r * r && !inside)
            {
                res.penetration = 0;
                return res;
            }

            d = (float)Math.Sqrt(d);

            if (inside)
            {
                res.normal = -n;
                res.penetration = r - d;
            }
            else
            {
                res.normal = n;
                res.penetration = r - d;
            }

            return res;
        }

        public static void ResolveCollision(Manifold_old m)
        {
            Vector2f relativeVel = m.B.Velocity - m.A.Velocity;

            float VelAlongNormal = DotProduct(relativeVel, m.normal);

            if (VelAlongNormal > 0)
                return;

            float e;
            if (m.A.Restiution > m.B.Restiution)
                e = m.B.Restiution;
            else
                e = m.A.Restiution;

            float j = -(1 + e) * VelAlongNormal;
            j /= 1 / m.A.Mass + 1 / m.B.Mass;

            Vector2f impulse = j * m.normal;

            float mass_sum = m.A.Mass + m.B.Mass;
            float ratio = m.A.Mass / mass_sum;
            if (!m.A.IsStatic)
                m.A.Velocity -= ratio * impulse;
            if (!m.B.IsStatic)
            {
                ratio = m.B.Mass / mass_sum;
                m.B.Velocity += ratio * impulse;
            }
        }

        public static void PositionalCorrection(Manifold_old m)
        {
            const float percent = 0.6f;
            const float slop = 0.001f;

            Vector2f correction;
            if (m.penetration - slop > 0.0f)
                correction = (m.penetration - slop) / (GetInversedMass(m.A) + GetInversedMass(m.B)) * percent * m.normal;
            else
                correction = 0.0f / (GetInversedMass(m.A) + GetInversedMass(m.B)) * percent * m.normal;

            m.A.WorldPosition -= GetInversedMass(m.A) * correction;
            m.B.WorldPosition += GetInversedMass(m.B) * correction;
        }

        private static float DotProduct(Vector2f a, Vector2f b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        private static float GetInversedMass(GameObject a)
        {
            if (a.Mass == 0)
                return 0;
            else
                return 1 / a.Mass;
        }

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
    }
}

using System;
using Game.SimpleGeometry;
using OpenTK;

namespace Game.Physics
{
    internal static class PenetrationDepths
    {
        public static Vector2 AabbAndAabb(Rect rect1, Rect rect2)
        {
            Vector2 n = rect2.Center - rect1.Center;
            Vector2 normal;
            float xOverlap = (rect1.Size / 2).X + (rect2.Size / 2).X - MathF.Abs(n.X);
            float yOverlap = (rect1.Size / 2).Y + (rect2.Size / 2).Y - MathF.Abs(n.Y);

            if (xOverlap < yOverlap)
            {
                if (n.X < 0)
                {
                    normal = -Vector2.UnitX;
                }
                else
                {
                    normal = Vector2.UnitX;
                }

                return xOverlap * normal;
            }
            else
            {
                if (n.Y < 0)
                {
                    normal = -Vector2.UnitY;
                }
                else
                {
                    normal = Vector2.UnitY;
                }

                return yOverlap * normal;
            }
        }

        public static Vector2 CircleAndCircle(Circle circle1, Circle circle2)
        {
            Vector2 distance = circle1.Center - circle2.Center;
            float penDepth = circle1.Radius + circle2.Radius - distance.Length;
            Vector2 penRes = distance.Normalized() * (penDepth / 2);
            return penRes;
        }

        public static Vector2 AabbAndCircle(Rect rect, Circle circle)
        {
            Vector2 n = circle.Center - rect.Center;
            Vector2 closest = n;
            Vector2 rectHalfDistances = rect.Size / 2f;
            closest = Vector2.Clamp(closest, -rectHalfDistances, rectHalfDistances);

            Vector2 normal = n - closest;

            float dist = normal.Length;
            float penDepth = circle.Radius - dist;
            Vector2 penRes = normal.Normalized() * penDepth;

            return penRes;
        }
    }
}
using System;
using Game.Interfaces;
using OpenTK;

namespace Game.Physics
{
    internal static class PenetrationDepths
    {
        internal static Vector2 HandlePenDepth(IReadonlySimpleGeometry geometry1, IReadonlySimpleGeometry geometry2)
        {
            if (geometry1 is IReadonlyRect)
            {
                if (geometry2 is IReadonlyRect)
                {
                    return AabbAndAabb((IReadonlyRect)geometry1, (IReadonlyRect)geometry2);
                }
                else if (geometry2 is IReadonlyCircle)
                {
                    return AabbAndCircle((IReadonlyRect)geometry1, (IReadonlyCircle)geometry2);
                }
            }
            else if (geometry1 is IReadonlyCircle)
            {
                if (geometry2 is IReadonlyCircle)
                {
                    return CircleAndCircle((IReadonlyCircle)geometry1, (IReadonlyCircle)geometry2);
                }
                else if (geometry2 is IReadonlyRect)
                {
                    return AabbAndCircle((IReadonlyRect)geometry2, (IReadonlyCircle)geometry1);
                }
            }

            throw new ArgumentException("PenDepth got geometry of unknown type!");
        }

        private static Vector2 AabbAndAabb(IReadonlyRect rect1, IReadonlyRect rect2)
        {
            Vector2 n = rect2.NextCenter - rect1.NextCenter;
            Vector2 normal;
            float aExtent = (rect1.MaxX - rect1.MinX) / 2;
            float bExtent = (rect2.MaxX - rect2.MinX) / 2;
            float xOverlap = aExtent + bExtent - MathF.Abs(n.X);

            if (xOverlap > 0f)
            {
                aExtent = (rect1.MaxY - rect1.MinY) / 2;
                bExtent = (rect2.MaxY - rect2.MinY) / 2;
                float yOverlap = aExtent + bExtent - MathF.Abs(n.Y);

                if (yOverlap > 0f)
                {
                    if (xOverlap < yOverlap)
                    {
                        if (n.X < 0f)
                        {
                            normal = Vector2.UnitX;
                        }
                        else
                        {
                            normal = -Vector2.UnitX;
                        }

                        return xOverlap * normal;
                    }
                    else
                    {
                        if (n.Y < 0f)
                        {
                            normal = Vector2.UnitY;
                        }
                        else
                        {
                            normal = -Vector2.UnitY;
                        }

                        return yOverlap * normal;
                    }
                }
            }

            return Vector2.Zero;
        }

        private static Vector2 CircleAndCircle(IReadonlyCircle circle1, IReadonlyCircle circle2)
        {
            Vector2 distance = circle1.NextCenter - circle2.NextCenter;
            float penDepth = circle1.Radius + circle2.Radius - distance.Length;
            Vector2 penRes = distance.Normalized() * (penDepth / 2);
            return penRes;
        }

        private static Vector2 AabbAndCircle(IReadonlyRect rect, IReadonlyCircle circle)
        {
            Vector2 n = circle.NextCenter - rect.NextCenter;
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
using System;
using System.Runtime.CompilerServices;
using Game.Interfaces;
using OpenTK;

[assembly: InternalsVisibleTo("UnitTests")]

namespace Game.Physics
{
    internal static class CollisionCheck
    {
        public static bool HandelCollision(IReadonlySimpleGeometry geometry1, IReadonlySimpleGeometry geometry2)
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

        internal static bool IsPointInAabb(IReadonlyRect rect, Vector2 point)
        {
            if (point.X > rect.MinX && point.X < rect.MaxX &&
                point.Y > rect.MinY && point.Y < rect.MaxY)
            {
                return true;
            }

            return false;
        }

        private static bool CircleAndCircle(IReadonlyCircle circle1, IReadonlyCircle circle2)
        {
            float squaredDistanceCircles = Vector2.DistanceSquared(circle1.NextCenter, circle2.NextCenter);
            float squaredRadii = (circle1.Radius + circle2.Radius) * (circle1.Radius + circle2.Radius);

            return squaredDistanceCircles <= squaredRadii;
        }

        private static bool AabbAndAabb(IReadonlyRect rect1, IReadonlyRect rect2)
        {
            bool collisionX = rect1.MaxX + rect1.PhysicOffset.X >= rect2.MinX + rect2.PhysicOffset.X && rect2.MaxX + rect2.PhysicOffset.X >= rect1.MinX + rect1.PhysicOffset.X;
            bool collisionY = rect1.MaxY + rect1.PhysicOffset.Y >= rect2.MinY + rect2.PhysicOffset.Y && rect2.MaxY + rect2.PhysicOffset.Y >= rect1.MinY + rect1.PhysicOffset.X;

            return collisionX && collisionY;
        }

        private static bool AabbAndCircle(IReadonlyRect rect, IReadonlyCircle circle)
        {
            Vector2 distance = circle.NextCenter - rect.NextCenter;
            Vector2 rectHalfDistances = rect.Size / 2f;
            Vector2 clampedDistance = Vector2.Clamp(distance, -rectHalfDistances, rectHalfDistances);
            Vector2 closest = rect.NextCenter + clampedDistance;
            distance = closest - circle.NextCenter;

            return distance.LengthSquared <= circle.Radius * circle.Radius;
        }
    }
}
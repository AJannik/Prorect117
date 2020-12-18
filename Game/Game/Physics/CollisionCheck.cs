using System.Runtime.CompilerServices;
using Game.SimpleGeometry;
using OpenTK;

[assembly: InternalsVisibleTo("UnitTests")]

namespace Game.Physics
{
    internal static class CollisionCheck
    {
        internal static bool CircleAndCircle(Circle circle1, Circle circle2)
        {
            float squaredDistanceCircles = Vector2.DistanceSquared(circle1.Center, circle2.Center);
            float squaredRadii = (circle1.Radius + circle2.Radius) * (circle1.Radius + circle2.Radius);

            if (squaredDistanceCircles <= squaredRadii)
            {
                return true;
            }

            return false;
        }

        internal static bool AabbAndAabb(Rect rect1, Rect rect2)
        {
            bool collisionX = rect1.MaxX >= rect2.MinX && rect2.MaxX >= rect1.MinX;
            bool collisionY = rect1.MaxY >= rect2.MinY && rect2.MaxY >= rect1.MinY;

            return collisionX && collisionY;
        }

        internal static bool AabbAndCircle(Rect activeRect, Circle passiveCircle)
        {
            Vector2 distance = passiveCircle.Center - activeRect.Center;
            Vector2 rectHalfDistances = activeRect.Size / 2f;
            Vector2 clampedDistance = Vector2.Clamp(distance, -rectHalfDistances, rectHalfDistances);
            Vector2 closest = activeRect.Center + clampedDistance;
            distance = closest - passiveCircle.Center;

            return distance.LengthSquared <= passiveCircle.Radius * passiveCircle.Radius;
        }

        internal static bool IsPointInAabb(Rect rect, Vector2 point)
        {
            if (point.X > rect.MinX && point.X < rect.MaxX &&
                point.Y > rect.MinY && point.Y < rect.MaxY)
            {
                return true;
            }

            return false;
        }
    }
}
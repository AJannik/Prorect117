using System;
using System.Collections.Generic;
using System.Text;
using Game.SimpleGeometry;
using OpenTK;

namespace Game.Tools
{
    public static class CollisionCheck
    {
        // algorithm from https://stackoverflow.com/questions/1073336/circle-line-segment-collision-detection-algorithm
        public static bool CircleAndLine(Circle circle, Ray ray)
        {
            Vector2 circleToRayStart = ray.StartPos - circle.Center;
            float a = Vector2.Dot(ray.Direction * ray.Length, ray.Direction * ray.Length);
            float b = 2 * Vector2.Dot(circleToRayStart, ray.Direction * ray.Length);
            float c = Vector2.Dot(circleToRayStart, circleToRayStart) - (circle.Radius * circle.Radius);

            float discriminant = (b * b) - (4 * a * c);

            // no collision
            if (discriminant < 0)
            {
                return false;
            }

            // 3x HIT cases:
            //          -o->             --|-->  |            |  --|->
            // Impale(t1 hit, t2 hit), Poke(t1 hit, t2 > 1), ExitWound(t1 < 0, t2 hit),

            // 3x MISS cases:
            //       ->  o                     o ->              | -> |
            // FallShort(t1 > 1, t2 > 1), Past(t1 < 0,t2 < 0), CompletelyInside(t1 < 0, t2 > 1)
            discriminant = MathF.Sqrt(discriminant);
            float t1 = (-b - discriminant) / (2 * a);
            float t2 = (-b + discriminant) / (2 * a);

            if (t1 >= 0 && t1 <= 1)
            {
                // t1 is the intersection, and it's closer than t2
                // (since t1 uses -b - discriminant)
                // Impale, Poke
                return true;
            }

            // here t1 didn't intersect so we are either started
            // inside the sphere or completely past it
            if (t2 >= 0 && t2 <= 1)
            {
                // ExitWound
                return true;
            }

            // no intn: FallShort, Past, CompletelyInside
            return false;
        }

        public static bool CircleAndCircle(Circle circle1, Circle circle2)
        {
            float squaredDistanceCircles = Vector2.DistanceSquared(circle1.Center, circle2.Center);
            float squaredRadii = (circle1.Radius + circle2.Radius) * (circle1.Radius + circle2.Radius);

            if (squaredDistanceCircles <= squaredRadii)
            {
                return true;
            }

            return false;
        }

        public static bool AabbAndLine(Rect rect, Ray ray)
        {
            throw new NotImplementedException();
        }

        public static bool AabbAndAabb(Rect rect1, Rect rect2)
        {
            throw new NotImplementedException();
        }

        public static bool AabbAndCircle(Rect rect, Circle circle)
        {
            throw new NotImplementedException();
        }
    }
}
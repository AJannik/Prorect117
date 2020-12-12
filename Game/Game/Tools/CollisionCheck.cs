﻿using System;
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
            //          -o->                    --|-->  |                   |  --|->
            // Impale(tmin hit, tmax hit), Poke(tmin hit, tmax > 1), ExitWound(tmin < 0, tmax hit),

            // 3x MISS cases:
            //             ->  o                     o ->              | -> |
            // FallShort(tmin > 1, tmax > 1), Past(tmin < 0,tmax < 0), CompletelyInside(tmin < 0, tmax > 1)
            discriminant = MathF.Sqrt(discriminant);
            float tmin = (-b - discriminant) / (2 * a);
            float tmax = (-b + discriminant) / (2 * a);

            if (tmin >= 0 && tmin <= 1)
            {
                // tmin is the intersection, and it's closer than tmax
                // (since tmin uses -b - discriminant)
                // Impale, Poke
                return true;
            }

            // here tmin didn't intersect so we are either started
            // inside the sphere or completely past it
            if (tmax >= 0 && tmax <= 1)
            {
                // ExitWound
                return true;
            }

            // no intn: FallShort, Past, CompletelyInside
            return false;
        }

        public static bool CircleAndCircle(Circle circle1, Circle circle2, Vector2 movement)
        {
            float squaredDistanceCircles = Vector2.DistanceSquared(circle1.Center + movement, circle2.Center);
            float squaredRadii = (circle1.Radius + circle2.Radius) * (circle1.Radius + circle2.Radius);

            if (squaredDistanceCircles <= squaredRadii)
            {
                return true;
            }

            return false;
        }

        public static bool AabbAndLine(Rect rect, Ray ray)
        {
            float t1 = (rect.MinX - ray.StartPos.X) / ray.Direction.X;
            float t2 = (rect.MaxX - ray.StartPos.X) / ray.Direction.X;
            float t3 = (rect.MinY - ray.StartPos.Y) / ray.Direction.Y;
            float t4 = (rect.MaxY - ray.StartPos.Y) / ray.Direction.Y;

            float tmin = MathF.Max(MathF.Min(t1, t2), MathF.Min(t3, t4));
            float tmax = MathF.Min(MathF.Max(t1, t2), MathF.Max(t3, t4));

            // if tmax < 0, ray (line) is intersecting AABB, but whole AABB is behind us
            if (tmax < 0)
            {
                return false;
            }

            // if tmin > tmax, ray doesn't intersect AABB
            if (tmin > tmax)
            {
                return false;
            }

            // ray stops before reaching the aabb
            if (tmax > ray.Length)
            {
                return false;
            }

            if (tmin < 0f)
            {
                return true; // tmax
            }

            return true; // tmin
        }

        public static bool AabbAndAabb(Rect rect1, Rect rect2, Vector2 movement)
        {
            bool collisionX = rect1.MaxX + movement.X >= rect2.MinX && rect2.MaxX >= rect1.MinX + movement.X;
            bool collisionY = rect1.MaxY + movement.Y >= rect2.MinY && rect2.MaxY >= rect1.MinY + movement.Y;

            return collisionX && collisionY;
        }

        public static bool AabbAndCircle(Rect activeRect, Circle passiveCircle, Vector2 movement)
        {
            Vector2 distance = passiveCircle.Center - (activeRect.Center + movement);
            Vector2 rectHalfDistances = activeRect.Size / 2f;
            Vector2 clampedDistance = Vector2.Clamp(distance, -rectHalfDistances, rectHalfDistances);
            Vector2 closest = activeRect.Center + clampedDistance;
            distance = (closest + movement) - passiveCircle.Center;

            return distance.LengthSquared <= passiveCircle.Radius * passiveCircle.Radius;
        }

        public static bool AabbAndCircle(Circle activeCircle, Rect passiveRect, Vector2 movement)
        {
            Vector2 distance = (activeCircle.Center + movement) - passiveRect.Center;
            Vector2 rectHalfDistances = passiveRect.Size / 2f;
            Vector2 clampedDistance = Vector2.Clamp(distance, -rectHalfDistances, rectHalfDistances);
            Vector2 closest = passiveRect.Center + clampedDistance;
            distance = closest - (activeCircle.Center + movement);

            return distance.LengthSquared <= activeCircle.Radius * activeCircle.Radius;
        }

        public static bool IsPointInAabb(Rect rect, Vector2 point)
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
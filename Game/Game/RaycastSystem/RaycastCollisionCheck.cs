using System;
using Game.SimpleGeometry;
using OpenTK;

namespace Game.RaycastSystem
{
    public static class RaycastCollisionCheck
    {
        public static bool CircleAndLine(Circle circle, Ray ray, RaycastHit hit)
        {
            Vector2 circleToRayStart = circle.Center - ray.StartPos;
            float radiusSquared = circle.Radius * circle.Radius;

            float a = Vector2.Dot(circleToRayStart, ray.Direction);
            float bSq = circleToRayStart.LengthSquared - (a * a);

            if (radiusSquared - bSq < 0f)
            {
                return false;
            }

            float f = MathF.Sqrt(radiusSquared - bSq);
            float t = 0f;

            if (circleToRayStart.LengthSquared < radiusSquared)
            {
                // Ray starts inside the circle
                t = a + f;
            }
            else
            {
                t = a - f;
            }

            if (t > ray.Length || t < 0f)
            {
                return false;
            }

            hit.HitPoint = ray.StartPos + (ray.Direction * t);
            hit.ObjectNormal = hit.HitPoint - circle.Center;

            return true;
        }

        public static bool AabbAndLine(Rect rect, Ray ray, RaycastHit hit)
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

            // TODO: Calculate hit normal
            if (tmin < 0f)
            {
                hit.HitPoint = ray.StartPos + (ray.Direction * tmax);
                hit.ObjectNormal = CalculateNormal(hit.HitPoint, rect);
                return true; // tmax
            }

            hit.HitPoint = ray.StartPos + (ray.Direction * tmin);
            hit.ObjectNormal = CalculateNormal(hit.HitPoint, rect);
            return true; // tmin
        }

        private static Vector2 CalculateNormal(Vector2 hitPos, Rect rect)
        {
            Vector2 normal = Vector2.Zero;
            if (hitPos.X - rect.MinX == 0)
            {
                normal += -Vector2.UnitX;
            }

            if (hitPos.X - rect.MaxX == 0)
            {
                normal += Vector2.UnitX;
            }

            if (hitPos.Y - rect.MinY == 0)
            {
                normal += -Vector2.UnitY;
            }

            if (hitPos.Y - rect.MaxY == 0)
            {
                normal += Vector2.UnitY;
            }

            return normal;
        }
    }
}
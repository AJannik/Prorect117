using System;
using System.Collections.Generic;
using System.Text;
using Game.SimpleGeometry;
using OpenTK;

namespace Game.Tools
{
    public static class CollisionCheck
    {
        public static bool CircleAndLine(Circle circle, Ray ray)
        {
            Vector2 circleToRayStart = ray.StartPos - circle.Center;
            float a = Vector2.Dot(ray.Direction * ray.Length, ray.Direction * ray.Length);
            float b = 2 * Vector2.Dot(circleToRayStart, ray.Direction * ray.Length);
            float c = Vector2.Dot(circleToRayStart, circleToRayStart) - (circle.Radius * circle.Radius);

            float discriminant = (b * b) - (4 * a * c);
            if (discriminant < 0)
            {
                return false;
            }

            return true;
        }
    }
}

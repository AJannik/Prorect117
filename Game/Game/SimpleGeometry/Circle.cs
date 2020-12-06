using OpenTK;

namespace Game.SimpleGeometry
{
    public class Circle
    {
        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public Vector2 Center { get; set; }

        public float Radius { get; set; }
    }
}
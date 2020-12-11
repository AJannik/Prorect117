using Game.Interfaces;
using OpenTK;

namespace Game.SimpleGeometry
{
    public class Circle : ISimpleGeometry
    {
        public Circle(Vector2 center, Vector2 radius)
        {
            Center = center;
            Size = radius;
        }

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Size = new Vector2(radius, 0f);
        }

        public Vector2 Center { get; set; }

        public Vector2 Size { get; set; }

        public float Radius => Size.X;
    }
}
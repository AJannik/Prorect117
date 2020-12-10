using OpenTK;

namespace Game.SimpleGeometry
{
    public class Rect
    {
        public Rect(float minX, float minY, float sizeX, float sizeY)
        {
            Center = new Vector2(minX + (sizeX / 2), minY + (sizeY / 2));
            Size = new Vector2(sizeX, sizeY);
        }

        public Rect(Vector2 center, Vector2 size)
        {
            Center = center;
            Size = size;
        }

        public Vector2 Center { get; set; }

        public Vector2 Size { get; set; }

        public float MinX => Center.X - (Size.X / 2);

        public float MinY => Center.Y - (Size.Y / 2);

        public float MaxX => Center.X + (Size.X / 2);

        public float MaxY => Center.Y + (Size.Y / 2);
    }
}
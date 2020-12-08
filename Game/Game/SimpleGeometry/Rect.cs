namespace Game.SimpleGeometry
{
    public class Rect
    {
        public Rect(float minX, float minY, float sizeX, float sizeY)
        {
            MinX = minX;
            MinY = minY;
            SizeX = sizeX;
            SizeY = sizeY;
        }

        public float MinX { get; set; }

        public float MinY { get; set; }

        public float MaxX => MinX + SizeX;

        public float MaxY => MinY + SizeY;

        public float SizeX { get; set; }

        public float SizeY { get; set; }

        public float CenterX => MinX + (SizeX / 2f);

        public float CenterY => MinY + (SizeY / 2f);
    }
}
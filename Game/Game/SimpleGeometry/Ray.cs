using OpenTK;

namespace Game.SimpleGeometry
{
    public class Ray
    {
        private Vector2 direction = new Vector2(0f, 0f);

        public Ray(Vector2 startPos, Vector2 direction, float length)
        {
            StartPos = startPos;
            Direction = direction;
            Length = length;
        }

        public Vector2 StartPos { get; set; }

        public Vector2 Direction
        {
            get { return direction; }
            set { direction = value.Normalized(); }
        }

        public float Length { get; set; }
    }
}
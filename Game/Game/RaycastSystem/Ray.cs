using OpenTK;

namespace Game.RaycastSystem
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

        public Vector2 EndPos
        {
            get
            {
                return StartPos + (Direction * Length);
            }
        }

        public float Length { get; set; }

        public Color Color { get; set; } = Color.White;
    }
}
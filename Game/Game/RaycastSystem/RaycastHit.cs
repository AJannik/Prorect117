using Game.Interfaces;
using OpenTK;

namespace Game.RaycastSystem
{
    public class RaycastHit
    {
        private Vector2 normal = Vector2.Zero;

        public ICollider HitObject { get; set; }

        public Vector2 HitPoint { get; set; }

        public Vector2 HitNormal
        {
            get { return normal; }
            set { normal = value.Normalized(); }
        }
    }
}
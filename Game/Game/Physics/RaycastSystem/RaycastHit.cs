using OpenTK;

namespace Game.Physics.RaycastSystem
{
    public class RaycastHit
    {
        private Vector2 normal = Vector2.Zero;

        public GameObject HitObject { get; set; }

        public Vector2 HitPoint { get; set; }

        /// <summary>
        /// Gets or Sets the normal vector of the collided objects surface.
        /// </summary>
        public Vector2 ObjectNormal
        {
            get { return normal; }
            set { normal = value.Normalized(); }
        }
    }
}
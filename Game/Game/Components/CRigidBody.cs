using Game.Interfaces;
using Game.SimpleGeometry;
using Game.Tools;
using OpenTK;

namespace Game.Components
{
    public class CRigidBody : IComponent
    {
        public GameObject MyGameObject { get; set; } = null;

        public ICollider Collider { get; set; }

        public float GravityScale { get; set; } = 1f;

        public bool Static { get; set; } = false;

        public bool Simulated { get; set; } = true;

        public bool UseGravity { get; set; } = true;

        public float Mass { get; set; } = 1f;

        public Vector2 Velocity { get; set; } = Vector2.Zero;

        private Vector2 Force { get; set; } = Vector2.Zero;

        public void Update(float deltaTime)
        {
            if (Collider == null)
            {
                SetCollider();
            }

            if (!Simulated || Static)
            {
                return;
            }

            Vector2 s = (Velocity + (Force / Mass * deltaTime)) * deltaTime;
            if (UseGravity)
            {
                s += PhysicConstants.Gravity * GravityScale * deltaTime;
            }

            foreach (CBoxCollider boxCollider in MyGameObject.Scene.GetCBoxColliders())
            {
                if (boxCollider != Collider && !boxCollider.IsTrigger)
                {
                    if (Collider.GetType() == typeof(CBoxCollider))
                    {
                        // TODO: Use predicted postition instead of current
                        if (!CollisionCheck.AabbAndAabb((Rect)Collider.Geometry, (Rect)boxCollider.Geometry, s))
                        {
                            MyGameObject.Transform.Position += s;
                        }
                    }

                    // TODO: CircleCollider
                }
            }

            // TODO: CircleCollider
        }

        public void AddForce(Vector2 force)
        {
            if (Static)
            {
                return;
            }

            Force += force;
        }

        public void ClearForce()
        {
            Force = Vector2.Zero;
        }

        private void SetCollider()
        {
            CBoxCollider[] colliders = MyGameObject.GetComponents<CBoxCollider>();
            if (colliders.Length > 0)
            {
                foreach (CBoxCollider boxCollider in colliders)
                {
                    if (!boxCollider.IsTrigger)
                    {
                        Collider = boxCollider;
                        return;
                    }
                }
            }

            // TODO: CircleCollider
        }
    }
}
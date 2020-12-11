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

        public Vector2 Gravitiy { get; private set; } = new Vector2(0f, -10f);

        public bool Static { get; set; } = false;

        public bool Simulated { get; set; } = true;

        public bool UseGravity { get; set; } = true;

        public float Mass { get; set; } = 1f;

        public Vector2 Force { get; private set; } = Vector2.Zero;

        public Vector2 Velocity { get; set; } = Vector2.Zero;

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

            foreach (CBoxCollider boxCollider in MyGameObject.Scene.GetCBoxColliders())
            {
                if (boxCollider != Collider && !boxCollider.IsTrigger)
                {
                    if (Collider.GetType() == typeof(CBoxCollider))
                    {
                        // TODO: Mybe use predicted postition instead of current
                        if (!CollisionCheck.AabbAndAabb((Rect)boxCollider.Geometry, (Rect)Collider.Geometry))
                        {
                            MyGameObject.Transform.Position += Force * deltaTime;
                        }
                    }

                    // TODO: CircleCollider
                }
            }

            // TODO: CircleCollider
        }

        public void AddForce(Vector2 direction, float thrust)
        {
            if (Static)
            {
                return;
            }

            direction = direction.Normalized();
            Vector2 acceleration = direction * thrust;
            if (UseGravity)
            {
                acceleration += Gravitiy;
            }

            Force += Mass * acceleration;
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
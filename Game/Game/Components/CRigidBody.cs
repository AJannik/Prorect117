using Game.Interfaces;
using Game.SimpleGeometry;
using Game.Tools;
using OpenTK;

namespace Game.Components
{
    public class CRigidBody : IComponent
    {
        public GameObject MyGameObject { get; set; } = null;

        public ICollider Collider { get; set; } // TODO: use List

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
                        if (!CollisionCheck.AabbAndAabb((Rect)Collider.Geometry, (Rect)boxCollider.Geometry, s))
                        {
                            MyGameObject.Transform.Position += s;
                        }
                    }

                    if (Collider.GetType() == typeof(CCircleCollider))
                    {
                        if (!CollisionCheck.AabbAndCircle((Rect)boxCollider.Geometry, (Circle)Collider.Geometry, s))
                        {
                            MyGameObject.Transform.Position += s;
                        }
                    }
                }
            }

            // TODO: CircleColliders
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
            CBoxCollider[] boxColliders = MyGameObject.GetComponents<CBoxCollider>();
            CCircleCollider[] circleColliders = MyGameObject.GetComponents<CCircleCollider>();
            if (boxColliders != null && boxColliders.Length > 0)
            {
                foreach (CBoxCollider boxCollider in boxColliders)
                {
                    if (!boxCollider.IsTrigger)
                    {
                        Collider = boxCollider;
                        return;
                    }
                }
            }

            if (circleColliders != null && circleColliders.Length > 0)
            {
                foreach (CCircleCollider circleCollider in circleColliders)
                {
                    if (!circleCollider.IsTrigger)
                    {
                        Collider = circleCollider;
                        return;
                    }
                }
            }
        }
    }
}
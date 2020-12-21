using System.Collections.Generic;
using Game.Interfaces;
using Game.Physics;
using Game.SimpleGeometry;
using OpenTK;

namespace Game.Components
{
    public class CRigidBody : IComponent
    {
        public GameObject MyGameObject { get; set; } = null;

        public float GravityScale { get; set; } = 1f;

        public bool Static { get; set; } = false;

        public bool Simulated { get; set; } = true;

        public bool UseGravity { get; set; } = true;

        public float Mass { get; set; } = 1f;

        public Vector2 Velocity { get; set; } = Vector2.Zero;

        private List<ICollider> Colliders { get; set; } = new List<ICollider>();

        private Vector2 Force { get; set; } = Vector2.Zero;

        public void Update(float deltaTime)
        {
            if (Colliders.Count == 0)
            {
                SetColliders();
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

            MyGameObject.Transform.Position += s;

            if (IsColliding())
            {
            }
            else
            {
            }
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

        private bool IsColliding()
        {
            foreach (ICollider myCollider in Colliders)
            {
                if (myCollider.GetType() == typeof(CBoxCollider))
                {
                    if (CheckBoxColliderCollisions((CBoxCollider)myCollider))
                    {
                        return true;
                    }
                }
                else if (myCollider.GetType() == typeof(CCircleCollider))
                {
                    if (CheckCircleColliderCollisions((CCircleCollider)myCollider))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckBoxColliderCollisions(CBoxCollider myCollider)
        {
            // Going through every non owned and non trigger CBoxCollider in the Scene
            foreach (CBoxCollider boxCollider in MyGameObject.Scene.GetCBoxColliders())
            {
                if (!Colliders.Contains(boxCollider) && !boxCollider.IsTrigger)
                {
                    if (CollisionCheck.AabbAndAabb((Rect)myCollider.Geometry, (Rect)boxCollider.Geometry))
                    {
                        MyGameObject.Transform.Position += PenetrationDepths.AabbAndAabb((Rect)myCollider.Geometry, (Rect)boxCollider.Geometry) * -1f;
                        return true;
                    }
                }
            }

            // Going through every non owned and non trigger CCircleCollider in the Scene
            foreach (CCircleCollider circelCollider in MyGameObject.Scene.GetCCircleColliders())
            {
                if (!Colliders.Contains(circelCollider) && !circelCollider.IsTrigger)
                {
                    if (CollisionCheck.AabbAndCircle((Rect)myCollider.Geometry, (Circle)circelCollider.Geometry))
                    {
                        MyGameObject.Transform.Position += PenetrationDepths.AabbAndCircle((Rect)myCollider.Geometry, (Circle)circelCollider.Geometry) * -1f;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckCircleColliderCollisions(CCircleCollider myCollider)
        {
            // Going through every non owned and non trigger CBoxCollider in the Scene
            foreach (CBoxCollider boxCollider in MyGameObject.Scene.GetCBoxColliders())
            {
                if (!Colliders.Contains(boxCollider) && !boxCollider.IsTrigger)
                {
                    if (CollisionCheck.AabbAndCircle((Rect)boxCollider.Geometry, (Circle)myCollider.Geometry))
                    {
                        MyGameObject.Transform.Position += PenetrationDepths.AabbAndCircle((Rect)boxCollider.Geometry, (Circle)myCollider.Geometry) * 1f;
                        return true;
                    }
                }
            }

            // Going through every non owned and non trigger CCircleCollider in the Scene
            foreach (CCircleCollider circelCollider in MyGameObject.Scene.GetCCircleColliders())
            {
                if (!Colliders.Contains(circelCollider) && !circelCollider.IsTrigger)
                {
                    if (CollisionCheck.CircleAndCircle((Circle)myCollider.Geometry, (Circle)circelCollider.Geometry))
                    {
                        MyGameObject.Transform.Position += PenetrationDepths.CircleAndCircle((Circle)myCollider.Geometry, (Circle)circelCollider.Geometry) * 1f;
                        return true;
                    }
                }
            }

            return false;
        }

        private void SetColliders()
        {
            CBoxCollider[] boxColliders = MyGameObject.GetComponents<CBoxCollider>();
            CCircleCollider[] circleColliders = MyGameObject.GetComponents<CCircleCollider>();
            if (boxColliders != null && boxColliders.Length > 0)
            {
                foreach (CBoxCollider boxCollider in boxColliders)
                {
                    if (!boxCollider.IsTrigger && !Colliders.Contains(boxCollider))
                    {
                        Colliders.Add(boxCollider);
                    }
                }
            }

            if (circleColliders != null && circleColliders.Length > 0)
            {
                foreach (CCircleCollider circleCollider in circleColliders)
                {
                    if (!circleCollider.IsTrigger && !Colliders.Contains(circleCollider))
                    {
                        Colliders.Add(circleCollider);
                    }
                }
            }
        }
    }
}
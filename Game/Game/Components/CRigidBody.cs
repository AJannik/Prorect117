using System;
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

        private Vector2 PenRes { get; set; } = Vector2.Zero;

        public void Update(float deltaTime)
        {
            if (!MyGameObject.Active)
            {
                return;
            }

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

            CheckCollision(s);
            s += PenRes;

            if (s.X < 0.0001f && s.X > -0.0001f)
            {
                s.X = 0f;
            }

            if (s.Y < 0.0001f && s.Y > -0.0001f)
            {
                s.Y = 0f;
            }

            MyGameObject.Transform.Position += s;
            PenRes = Vector2.Zero;
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

        private void CheckCollision(Vector2 s)
        {
            foreach (ICollider myCollider in Colliders)
            {
                myCollider.Geometry.PhysicOffset = s;
                if (myCollider.GetType() == typeof(CBoxCollider))
                {
                    CheckBoxColliderCollisions((CBoxCollider)myCollider);
                }
                else if (myCollider.GetType() == typeof(CCircleCollider))
                {
                    CheckCircleColliderCollisions((CCircleCollider)myCollider);
                }

                myCollider.Geometry.PhysicOffset = Vector2.Zero;
            }
        }

        private void CheckBoxColliderCollisions(CBoxCollider myCollider)
        {
            // Going through every non owned and non trigger CBoxCollider in the Scene
            foreach (var item in MyGameObject.Scene.GetCBoxColliders())
            {
                if (!Colliders.Contains(item) && !item.IsTrigger)
                {
                    PenRes += PenetrationDepths.AabbAndAabb((Rect)myCollider.Geometry, (Rect)item.Geometry);
                }
            }

            // Going through every non owned and non trigger CCircleCollider in the Scene
            foreach (CCircleCollider circelCollider in MyGameObject.Scene.GetCCircleColliders())
            {
                if (!Colliders.Contains(circelCollider) && !circelCollider.IsTrigger)
                {
                    if (CollisionCheck.AabbAndCircle((Rect)myCollider.Geometry, (Circle)circelCollider.Geometry))
                    {
                        PenRes += PenetrationDepths.AabbAndCircle((Rect)myCollider.Geometry, (Circle)circelCollider.Geometry) * -1f;
                    }
                }
            }
        }

        private void CheckCircleColliderCollisions(CCircleCollider myCollider)
        {
            // Going through every non owned and non trigger CBoxCollider in the Scene
            foreach (CBoxCollider boxCollider in MyGameObject.Scene.GetCBoxColliders())
            {
                if (!Colliders.Contains(boxCollider) && !boxCollider.IsTrigger)
                {
                    if (CollisionCheck.AabbAndCircle((Rect)boxCollider.Geometry, (Circle)myCollider.Geometry))
                    {
                        PenRes += PenetrationDepths.AabbAndCircle((Rect)boxCollider.Geometry, (Circle)myCollider.Geometry) * 1f;
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
                        PenRes += PenetrationDepths.CircleAndCircle((Circle)myCollider.Geometry, (Circle)circelCollider.Geometry) * 1f;
                    }
                }
            }
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
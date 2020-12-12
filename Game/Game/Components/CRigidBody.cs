using System;
using System.Collections.Generic;
using Game.Interfaces;
using Game.SimpleGeometry;
using Game.Tools;
using OpenTK;

namespace Game.Components
{
    public class CRigidBody : IComponent
    {
        public GameObject MyGameObject { get; set; } = null;

        public List<ICollider> Colliders { get; set; } = new List<ICollider>();

        public float GravityScale { get; set; } = 1f;

        public bool Static { get; set; } = false;

        public bool Simulated { get; set; } = true;

        public bool UseGravity { get; set; } = true;

        public float Mass { get; set; } = 1f;

        public Vector2 Velocity { get; set; } = Vector2.Zero;

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

            if (!IsColliding(3f * s))
            {
                MyGameObject.Transform.Position += s;
            }
            else
            {
                Velocity = Vector2.Zero;
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

        private bool IsColliding(Vector2 movement)
        {
            foreach (ICollider myCollider in Colliders)
            {
                if (myCollider.GetType() == typeof(CBoxCollider))
                {
                    if (CheckBoxColliderCollisions((CBoxCollider)myCollider, movement))
                    {
                        return true;
                    }
                }
                else if (myCollider.GetType() == typeof(CCircleCollider))
                {
                    if (CheckCircleColliderCollisions((CCircleCollider)myCollider, movement))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckBoxColliderCollisions(CBoxCollider myCollider, Vector2 movement)
        {
            foreach (CBoxCollider boxCollider in MyGameObject.Scene.GetCBoxColliders())
            {
                if (boxCollider != myCollider && !boxCollider.IsTrigger)
                {
                    if (CollisionCheck.AabbAndAabb((Rect)myCollider.Geometry, (Rect)boxCollider.Geometry, movement))
                    {
                        return true;
                    }
                }
            }

            foreach (CCircleCollider circelCollider in MyGameObject.Scene.GetCCircleColliders())
            {
                if (!circelCollider.IsTrigger)
                {
                    if (CollisionCheck.AabbAndCircle((Rect)myCollider.Geometry, (Circle)circelCollider.Geometry, movement))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckCircleColliderCollisions(CCircleCollider myCollider, Vector2 movement)
        {
            foreach (CBoxCollider boxCollider in MyGameObject.Scene.GetCBoxColliders())
            {
                if (!boxCollider.IsTrigger)
                {
                    if (CollisionCheck.AabbAndCircle((Circle)myCollider.Geometry, (Rect)boxCollider.Geometry, movement))
                    {
                        return true;
                    }
                }
            }

            foreach (CCircleCollider circelCollider in MyGameObject.Scene.GetCCircleColliders())
            {
                if (myCollider != circelCollider && !circelCollider.IsTrigger)
                {
                    if (CollisionCheck.CircleAndCircle((Circle)myCollider.Geometry, (Circle)circelCollider.Geometry, movement))
                    {
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
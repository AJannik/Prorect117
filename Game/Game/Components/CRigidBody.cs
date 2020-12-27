﻿using System;
using System.Collections.Generic;
using Game.Interfaces;
using Game.Physics;
using Game.SimpleGeometry;
using OpenTK;

namespace Game.Components
{
    public class CRigidBody : IComponent, IPhysicsComponent
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

        private Vector2 Acceleration { get; set; } = Vector2.Zero;

        public void Update(float deltaTime)
        {
        }

        public void FixedUpdate()
        {
            float deltaTime = PhysicConstants.FixedDeltaTime;
            if (!MyGameObject.Active)
            {
                return;
            }

            deltaTime = PhysicConstants.FixedUpdate;

            if (Colliders.Count == 0)
            {
                SetColliders();
            }

            if (!Simulated || Static)
            {
                return;
            }

            if (UseGravity)
            {
                AddForce(PhysicConstants.Gravity * GravityScale * Mass);
            }

            // Verlet Integral
            Vector2 s = deltaTime * (Velocity + (deltaTime * Acceleration / 2f));
            Vector2 newAcceleration = Force / Mass;
            Velocity += deltaTime * (Acceleration + newAcceleration) / 2f;
            Acceleration = newAcceleration;

            CheckCollision(s);
            s += PenRes;
            s = CorrectRoundingErrors(s);

            // Subtract velocity of PenDepth resolution
            // TODO: Only add if opposite direction
            if ((PenRes.X > 0f && Velocity.X > 0f) || (PenRes.X < 0f && Velocity.X < 0f) || (PenRes.Y > 0f && Velocity.Y > 0f) || (PenRes.Y < 0f && Velocity.Y < 0f))
            {
            }
            else
            {
                Velocity += (PenRes / deltaTime) - (deltaTime * Acceleration / 2f);
            }

            Velocity = CorrectRoundingErrors(Velocity);
            Acceleration = CorrectRoundingErrors(Acceleration);

            MyGameObject.Transform.Position += s;
            if (MyGameObject.Name == "Player")
            {
                Console.WriteLine($"{Velocity} {(Acceleration + newAcceleration) / 2f} {PenRes}");
            }

            PenRes = Vector2.Zero;
            ClearForce();
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

        private Vector2 CorrectRoundingErrors(Vector2 vector)
        {
            Vector2 s = new Vector2(vector.X, vector.Y);
            if (s.X < 0.0001f && s.X > -0.0001f)
            {
                s.X = 0f;
            }

            if (s.Y < 0.0001f && s.Y > -0.0001f)
            {
                s.Y = 0f;
            }

            return s;
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
            foreach (CBoxCollider boxCollider in MyGameObject.Scene.GetCBoxColliders())
            {
                if (!Colliders.Contains(boxCollider) && !boxCollider.IsTrigger)
                {
                    Vector2 x = PenetrationDepths.AabbAndAabb((Rect)myCollider.Geometry, (Rect)boxCollider.Geometry);
                    if (CorrectRoundingErrors(PenRes - x) != Vector2.Zero)
                    {
                        PenRes += x;
                    }
                }
            }

            // Going through every non owned and non trigger CCircleCollider in the Scene
            foreach (CCircleCollider circelCollider in MyGameObject.Scene.GetCCircleColliders())
            {
                if (!Colliders.Contains(circelCollider) && !circelCollider.IsTrigger)
                {
                    Vector2 x = PenetrationDepths.AabbAndCircle((Rect)myCollider.Geometry, (Circle)circelCollider.Geometry) * -1f;
                    if (CorrectRoundingErrors(PenRes - x) != Vector2.Zero)
                    {
                        PenRes += x;
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
                    Vector2 x = PenetrationDepths.AabbAndCircle((Rect)boxCollider.Geometry, (Circle)myCollider.Geometry);
                    if (CorrectRoundingErrors(PenRes - x) != Vector2.Zero)
                    {
                        PenRes += x;
                    }
                }
            }

            // Going through every non owned and non trigger CCircleCollider in the Scene
            foreach (CCircleCollider circelCollider in MyGameObject.Scene.GetCCircleColliders())
            {
                if (!Colliders.Contains(circelCollider) && !circelCollider.IsTrigger)
                {
                    Vector2 x = PenetrationDepths.CircleAndCircle((Circle)myCollider.Geometry, (Circle)circelCollider.Geometry);
                    if (CorrectRoundingErrors(PenRes - x) != Vector2.Zero)
                    {
                        PenRes += x;
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
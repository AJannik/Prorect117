﻿using System;
using System.Collections.Generic;
using Game.Components.Collision;
using Game.Entity;
using Game.Interfaces;
using Game.Interfaces.ActorInterfaces;
using Game.Physics;
using OpenTK;

namespace Game.Components
{
    public class CRigidBody : IComponent, IPhysicsComponent, IOnStart
    {
        public GameObject MyGameObject { get; set; } = null;

        public float GravityScale { get; set; } = 1f;

        public bool Static { get; set; } = false;

        public bool Simulated { get; set; } = true;

        public bool UseGravity { get; set; } = true;

        public float Mass { get; set; } = 1f;

        public Vector2 Velocity { get; set; } = Vector2.Zero;

        public bool IgnoreCollision { get; set; } = false;

        private List<ICollider> Colliders { get; set; } = new List<ICollider>();

        private Vector2 Force { get; set; } = Vector2.Zero;

        private Vector2 PenRes { get; set; } = Vector2.Zero;

        private Vector2 Acceleration { get; set; } = Vector2.Zero;

        public void Start()
        {
            SetColliders();
            if (MyGameObject.GetComponent<IActor>() != null)
            {
                IgnoreCollision = true;
            }
        }

        public void FixedUpdate(float deltaTime)
        {
            if (!Simulated)
            {
                Velocity = Vector2.Zero;
                Force = Vector2.Zero;
                PenRes = Vector2.Zero;
                return;
            }

            if (Static)
            {
                return;
            }

            if (UseGravity)
            {
                AddForce(PhysicConstants.Gravity * GravityScale * Mass);
            }

            // Verlet Integral
            Vector2 s, newAcceleration;
            Integrate(deltaTime, out s, out newAcceleration);

            // Check y-axis for collision
            CheckCollision(new Vector2(0f, s.Y));
            s.Y += PenRes.Y;
            s = CorrectRoundingErrors(s);

            if ((Velocity.Y > 0f && PenRes.Y < 0f) || (Velocity.Y < 0f && PenRes.Y > 0f))
            {
                float yRes = (PenRes.Y / deltaTime) - (deltaTime * Acceleration.Y / 2f);
                if (MathF.Abs(Velocity.Y) < MathF.Abs(yRes))
                {
                    yRes = -Velocity.Y;
                }

                Velocity += new Vector2(0f, yRes);
            }

            // Check x-axis for collision
            CheckCollision(new Vector2(s.X, 0f));
            s.X += PenRes.X;
            s = CorrectRoundingErrors(s);

            // reduce velocity by PenRes amount
            if ((Velocity.X > 0f && PenRes.X < 0f) || (Velocity.X < 0f && PenRes.X > 0f))
            {
                float xRes = (PenRes.X / deltaTime) - (deltaTime * Acceleration.X / 2f);
                if (MathF.Abs(Velocity.X) < MathF.Abs(xRes))
                {
                    xRes = -Velocity.X;
                }

                Velocity += new Vector2(xRes, 0f);
            }

            Velocity = CorrectRoundingErrors(Velocity);
            Acceleration = CorrectRoundingErrors(Acceleration);

            MyGameObject.Transform.Position += s;

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

        private void Integrate(float deltaTime, out Vector2 s, out Vector2 newAcceleration)
        {
            s = deltaTime * (Velocity + (deltaTime * Acceleration / 2f));
            newAcceleration = Force / Mass;
            Velocity += deltaTime * (Acceleration + newAcceleration) / 2f;
            Acceleration = newAcceleration;
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
                CheckColliderCollisions(myCollider);

                myCollider.Geometry.PhysicOffset = Vector2.Zero;
            }
        }

        private void CheckColliderCollisions(ICollider myCollider)
        {
            foreach (ICollider collider in MyGameObject.Scene.GetColliders())
            {
                if (collider.MyGameObject.Active && !Colliders.Contains(collider) && collider.MyGameObject.GetComponent<CRigidBody>() != null && !(IgnoreCollision && collider.MyGameObject.GetComponent<CRigidBody>().IgnoreCollision))
                {
                    Vector2 x = PenetrationDepths.HandlePenDepth((IReadonlySimpleGeometry)myCollider.Geometry, (IReadonlySimpleGeometry)collider.Geometry);
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
                    if (!Colliders.Contains(boxCollider))
                    {
                        Colliders.Add(boxCollider);
                    }
                }
            }

            if (circleColliders != null && circleColliders.Length > 0)
            {
                foreach (CCircleCollider circleCollider in circleColliders)
                {
                    if (!Colliders.Contains(circleCollider))
                    {
                        Colliders.Add(circleCollider);
                    }
                }
            }
        }
    }
}
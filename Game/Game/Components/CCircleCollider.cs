using System;
using System.Collections.Generic;
using Game.Interfaces;
using Game.Physics;
using Game.SimpleGeometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CCircleCollider : IComponent, ICollider
    {
        public event EventHandler<IComponent> TriggerEntered;

        public event EventHandler<IComponent> TriggerExited;

        public GameObject MyGameObject { get; set; } = null;

        public bool IsTrigger { get; set; } = false;

        public Vector2 Offset { get; set; } = Vector2.Zero;

        public ISimpleGeometry Geometry { get; set; } = new Circle(Vector2.Zero, 1f);

        private List<IComponent> TriggerHits { get; set; } = new List<IComponent>();

        private int NumVerticies { get; set; } = 18;

        public void Update(float deltaTime)
        {
            Geometry.Center = MyGameObject.Transform.WorldPosition + Offset;
            if (IsTrigger)
            {
                CheckForBoxCollider();
                CheckForCircleCollider();
            }
        }

        public void DebugDraw()
        {
            Circle circle = (Circle)Geometry;
            float delta = 2f * MathF.PI / NumVerticies;

            if (IsTrigger)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.LimeGreen);
            }

            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < NumVerticies; i++)
            {
                float alpha = i * delta;
                float x = MathF.Cos(alpha);
                float y = MathF.Sin(alpha);
                Vector2 point = new Vector2(x, y);
                GL.Vertex2(circle.Center + (circle.Radius * point));
            }

            GL.End();
            GL.Color4(Color.White);
        }

        public IReadOnlyList<IComponent> GetTriggerHits()
        {
            return TriggerHits;
        }

        private bool IsValidCollider(CBoxCollider boxCollider)
        {
            return boxCollider.MyGameObject.Active && !boxCollider.IsTrigger && boxCollider.MyGameObject != MyGameObject && boxCollider.MyGameObject.GetComponent<CRigidBody>() != null;
        }

        private bool IsValidCollider(CCircleCollider circleCollider)
        {
            return circleCollider != this && circleCollider.MyGameObject.Active && !circleCollider.IsTrigger && circleCollider.MyGameObject != MyGameObject && circleCollider.MyGameObject.GetComponent<CRigidBody>() != null;
        }

        private void CheckForBoxCollider()
        {
            foreach (CBoxCollider boxCollider in MyGameObject.Scene.GetCBoxColliders())
            {
                if (IsValidCollider(boxCollider))
                {
                    if (CollisionCheck.AabbAndCircle((Rect)boxCollider.Geometry, (Circle)Geometry))
                    {
                        if (!TriggerHits.Contains(boxCollider))
                        {
                            // OnTriggerEnter event
                            TriggerEntered?.Invoke(this, boxCollider);
                            TriggerHits.Add(boxCollider);
                        }
                    }
                    else if (TriggerHits.Contains(boxCollider))
                    {
                        // OnTriggerExit event
                        TriggerExited?.Invoke(this, boxCollider);
                        TriggerHits.Remove(boxCollider);
                    }
                }
            }
        }

        private void CheckForCircleCollider()
        {
            foreach (CCircleCollider circleCollider in MyGameObject.Scene.GetCCircleColliders())
            {
                if (IsValidCollider(circleCollider))
                {
                    if (CollisionCheck.CircleAndCircle((Circle)Geometry, (Circle)circleCollider.Geometry))
                    {
                        if (!TriggerHits.Contains(circleCollider))
                        {
                            // OnTriggerEnter event
                            TriggerEntered?.Invoke(this, circleCollider);
                            TriggerHits.Add(circleCollider);
                        }
                    }
                    else if (TriggerHits.Contains(circleCollider))
                    {
                        // OnTriggerExit event
                        TriggerExited?.Invoke(this, circleCollider);
                        TriggerHits.Remove(circleCollider);
                    }
                }
            }
        }
    }
}
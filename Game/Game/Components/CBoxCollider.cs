using System;
using System.Collections.Generic;
using Game.Interfaces;
using Game.Physics;
using Game.SimpleGeometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CBoxCollider : IComponent, ICollider, IUpdateable, IDebugDrawable
    {
        public event EventHandler<IComponent> TriggerEntered;

        public event EventHandler<IComponent> TriggerExited;

        public GameObject MyGameObject { get; set; } = null;

        public ISimpleGeometry Geometry { get; set; } = new Rect(Vector2.Zero, new Vector2(1f, 1f));

        public bool IsTrigger { get; set; } = false;

        public Vector2 Offset { get; set; } = Vector2.Zero;

        private List<IComponent> TriggerHits { get; set; } = new List<IComponent>();

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
            if (IsTrigger)
            {
                GL.Color4(Color.Yellow);
            }
            else
            {
                GL.Color4(Color.LimeGreen);
            }

            Rect x = (Rect)Geometry;
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(x.MinX, x.MinY);
            GL.Vertex2(x.MaxX, x.MinY);
            GL.Vertex2(x.MaxX, x.MaxY);
            GL.Vertex2(x.MinX, x.MaxY);
            GL.End();
            GL.Color4(Color.White);
        }

        public IReadOnlyList<IComponent> GetTriggerHits()
        {
            return TriggerHits;
        }

        private bool IsValidCollider(CBoxCollider boxCollider)
        {
            return boxCollider != this && boxCollider.MyGameObject.Active && !boxCollider.IsTrigger && boxCollider.MyGameObject != MyGameObject && boxCollider.MyGameObject.GetComponent<CRigidBody>() != null;
        }

        private bool IsValidCollider(CCircleCollider circleCollider)
        {
            return circleCollider.MyGameObject.Active && !circleCollider.IsTrigger && circleCollider.MyGameObject != MyGameObject && circleCollider.MyGameObject.GetComponent<CRigidBody>() != null;
        }

        private void CheckForBoxCollider()
        {
            foreach (CBoxCollider boxCollider in MyGameObject.Scene.GetCBoxColliders())
            {
                if (IsValidCollider(boxCollider))
                {
                    if (CollisionCheck.AabbAndAabb((Rect)Geometry, (Rect)boxCollider.Geometry))
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
                    if (CollisionCheck.AabbAndCircle((Rect)Geometry, (Circle)circleCollider.Geometry))
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
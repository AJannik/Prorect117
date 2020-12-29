using System;
using System.Collections.Generic;
using Game.Interfaces;
using Game.Physics;
using Game.SimpleGeometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CBoxCollider : ICollider, IUpdateable, IDebugDrawable
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
                CheckForColliders();
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

        private bool IsValidCollider(ICollider collider)
        {
            return collider != this && collider.MyGameObject.Active && !collider.IsTrigger && collider.MyGameObject != MyGameObject && collider.MyGameObject.GetComponent<CRigidBody>() != null;
        }

        private void CheckForColliders()
        {
            foreach (ICollider collider in MyGameObject.Scene.GetColliders())
            {
                if (IsValidCollider(collider))
                {
                    IComponent component = collider as IComponent;
                    if (CollisionCheck.HandelCollision((IReadonlySimpleGeometry)collider.Geometry, (IReadonlySimpleGeometry)Geometry))
                    {
                        if (!TriggerHits.Contains(component))
                        {
                            // OnTriggerEnter event
                            TriggerEntered?.Invoke(this, component);
                            TriggerHits.Add(component);
                        }
                    }
                    else if (TriggerHits.Contains(component))
                    {
                        // OnTriggerExit event
                        TriggerExited?.Invoke(this, component);
                        TriggerHits.Remove(component);
                    }
                }
            }
        }
    }
}
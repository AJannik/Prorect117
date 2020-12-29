using System;
using System.Collections.Generic;
using Game.Interfaces;
using Game.Physics;
using Game.SimpleGeometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CBoxTrigger : ITrigger, IUpdateable, IDebugDrawable
    {
        public event EventHandler<IComponent> TriggerEntered;

        public event EventHandler<IComponent> TriggerExited;

        public Vector2 Offset { get; set; } = Vector2.Zero;

        public ISimpleGeometry Geometry { get; set; } = new Rect(Vector2.Zero, new Vector2(1f, 1f));

        public GameObject MyGameObject { get; set; } = null;

        private List<IComponent> TriggerHits { get; set; } = new List<IComponent>();

        public void Update(float deltaTime)
        {
            Geometry.Center = MyGameObject.Transform.WorldPosition + Offset;
            CheckForColliders();
        }

        public void DebugDraw()
        {
            Rect x = (Rect)Geometry;
            GL.Color4(Color.Yellow);
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
            return collider.MyGameObject.Active && collider.MyGameObject != MyGameObject && collider.MyGameObject.GetComponent<CRigidBody>() != null;
        }

        private void CheckForColliders()
        {
            foreach (ICollider collider in MyGameObject.Scene.GetColliders())
            {
                if (IsValidCollider(collider))
                {
                    if (CollisionCheck.HandelCollision((IReadonlySimpleGeometry)collider.Geometry, (IReadonlySimpleGeometry)Geometry))
                    {
                        if (!TriggerHits.Contains(collider))
                        {
                            // OnTriggerEnter event
                            TriggerEntered?.Invoke(this, collider);
                            TriggerHits.Add(collider);
                        }
                    }
                    else if (TriggerHits.Contains(collider))
                    {
                        // OnTriggerExit event
                        TriggerExited?.Invoke(this, collider);
                        TriggerHits.Remove(collider);
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Game.Interfaces;
using Game.Physics;
using Game.SimpleGeometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components.Collision
{
    public class CCircleTrigger : ITrigger, IUpdateable, IDebugDrawable
    {
        public event EventHandler<IComponent> TriggerEntered;

        public event EventHandler<IComponent> TriggerExited;

        public Vector2 Offset { get; set; } = Vector2.Zero;

        public ISimpleGeometry Geometry { get; set; } = new Circle(Vector2.Zero, 1f);

        public GameObject MyGameObject { get; set; } = null;

        private List<IComponent> TriggerHits { get; set; } = new List<IComponent>();

        private int NumVerticies { get; set; } = 18;

        public void Update(float deltaTime)
        {
            Geometry.Center = MyGameObject.Transform.WorldPosition + Offset;
            CheckForColliders();
        }

        public void DebugDraw()
        {
            Circle circle = (Circle)Geometry;
            float delta = 2f * MathF.PI / NumVerticies;

            GL.Color4(Color.Yellow);
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
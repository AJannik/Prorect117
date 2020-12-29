using System;
using System.Collections.Generic;
using Game.Interfaces;
using Game.Physics;
using Game.SimpleGeometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CCircleCollider : ICollider, IUpdateable, IDebugDrawable
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
                CheckForColliders();
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
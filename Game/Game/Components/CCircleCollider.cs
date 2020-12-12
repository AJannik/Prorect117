using System;
using Game.Interfaces;
using Game.SimpleGeometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CCircleCollider : IComponent, ICollider
    {
        public GameObject MyGameObject { get; set; } = null;

        public bool IsTrigger { get; set; } = false;

        public ISimpleGeometry Geometry { get; set; } = new Circle(Vector2.Zero, 0.2f);

        private int NumVerticies { get; set; } = 18;

        public void DebugDraw()
        {
            Circle circle = (Circle)Geometry;
            float delta = 2f * MathF.PI / NumVerticies;

            GL.Color4(Color.LimeGreen);
            GL.LineWidth(1.5f);
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

        public void Update(float deltaTime)
        {
            Geometry.Center = MyGameObject.Transform.WorldPosition;
        }
    }
}
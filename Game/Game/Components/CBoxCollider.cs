using System;
using Game.Interfaces;
using Game.SimpleGeometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CBoxCollider : IComponent, ICollider
    {
        public GameObject MyGameObject { get; set; } = null;

        public ISimpleGeometry Geometry { get; set; } = new Rect(0f, 0f, 0.4f, 0.2f);

        public bool IsTrigger { get; set; } = false;

        public void Update(float deltaTime)
        {
            Geometry.Center = MyGameObject.Transform.WorldPosition;
        }

        public void DebugDraw()
        {
            Rect x = (Rect)Geometry;
            GL.Color4(Color.LimeGreen);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(x.MinX, x.MinY);
            GL.Vertex2(x.MaxX, x.MinY);
            GL.Vertex2(x.MaxX, x.MaxY);
            GL.Vertex2(x.MinX, x.MaxY);
            GL.End();
            GL.Color4(Color.White);
        }
    }
}
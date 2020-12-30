using Game.Interfaces;
using Game.SimpleGeometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components.Colision
{
    public class CBoxCollider : ICollider, IUpdateable, IDebugDrawable
    {
        public GameObject MyGameObject { get; set; } = null;

        public ISimpleGeometry Geometry { get; set; } = new Rect(Vector2.Zero, new Vector2(1f, 1f));

        public Vector2 Offset { get; set; } = Vector2.Zero;

        public void Update(float deltaTime)
        {
            Geometry.Center = MyGameObject.Transform.WorldPosition + Offset;
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
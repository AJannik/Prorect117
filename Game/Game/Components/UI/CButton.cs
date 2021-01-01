using System;
using Game.Interfaces;
using Game.SimpleGeometry;
using Game.Tools;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Game.Components.UI
{
    public class CButton : IComponent, IMouseListener, IGuiElement, IDebugDrawable, IUpdateable
    {
        private Matrix4 matrix;

        private bool transformSize = true;

        public GameObject MyGameObject { get; set; } = null;

        public CCanvas Canvas { get; set; }

        public Vector2 Offset { get; set; } = Vector2.Zero;

        private ISimpleGeometry Geometry { get; set; } = new Rect(Vector2.Zero, new Vector2(1f, 1f));

        public void Update(float deltaTime)
        {
            if (transformSize)
            {
                TransformSize();
            }
        }

        public void MouseEvent(MouseButtonEventArgs args)
        {
            Geometry.Center = MyGameObject.Transform.Position;

            Vector2 mouseCoords = new Vector2(args.X, args.Y);
            mouseCoords = Transformation.Transform(mouseCoords, Canvas.CanvasMouseMatrix);
            Console.WriteLine(mouseCoords);

            bool hit = false;
            if (Geometry is Rect)
            {
                hit = IsPointInAabb((IReadonlyRect)Geometry, mouseCoords);
            }
            else
            {
                hit = IsPointInCircle((IReadonlyCircle)Geometry, mouseCoords);
            }

            if (hit)
            {
                Console.WriteLine("Button click");
            }

            // TODO: Check IsPointInCollider() for coords
            // TODO: if true then throw ButtonClicked event
        }

        public void DebugDraw()
        {
            Geometry.Center = MyGameObject.Transform.Position;
            matrix = Matrix4.Identity;
            GL.LoadMatrix(ref matrix);

            Rect x = (Rect)Geometry;
            GL.Color4(Color.Aqua);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(x.MinX, x.MinY);
            GL.Vertex2(x.MaxX, x.MinY);
            GL.Vertex2(x.MaxX, x.MaxY);
            GL.Vertex2(x.MinX, x.MaxY);
            GL.End();

            matrix = Canvas.Camera.CameraMatrix;
            GL.LoadMatrix(ref matrix);
            GL.Color4(Color.White);
        }

        public void SetSize(Vector2 size)
        {
            Geometry.Size = size;
            transformSize = true;
        }

        private void TransformSize()
        {
            Geometry.Size = Transformation.Transform(Geometry.Size, Canvas.CanvasDrawMatrix);
            transformSize = false;
        }

        private bool IsPointInCircle(IReadonlyCircle circle, Vector2 point)
        {
            Vector2 dist = circle.Center - point;
            if (dist.LengthSquared < circle.Radius * circle.Radius)
            {
                return true;
            }

            return false;
        }

        private bool IsPointInAabb(IReadonlyRect rect, Vector2 point)
        {
            if (point.X > rect.MinX && point.X < rect.MaxX &&
                point.Y > rect.MinY && point.Y < rect.MaxY)
            {
                return true;
            }

            return false;
        }
    }
}
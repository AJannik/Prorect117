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

        public event EventHandler<int> ButtonClicked;

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

            if (IsPointInAabb((IReadonlyRect)Geometry, mouseCoords))
            {
                ButtonClicked?.Invoke(this, 0);

                // TODO: Remove
                MyGameObject.Scene.GameManager.Coins++;
            }
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
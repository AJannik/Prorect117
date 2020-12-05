﻿using System;
using Game.Tools;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CRender : IComponent
    {
        public CRender()
        {
            // Texture = TextureTools.LoadFromResource("Content.default.png");
            TexCoords = new Rect(0f, 0f, 1f, 1f);
            Offset = new Vector2(0, 0);
        }

        public GameObject MyGameObject { get; set; } = null;

        public int Layer { get; set; }

        private int Texture { get; set; }

        private Rect Boundary { get; set; } = new Rect(0.1f, 0.1f, 0.2f, 0.2f);

        private Rect TexCoords { get; set; }

        private CTransform Transform { get; set; }

        private Matrix3 RotationMatrix { get; set; }

        private Vector2 Offset { get; set; }

        public void Update(float deltaTime)
        {
            // pull Transform from object
            Transform = MyGameObject.Transform;

            UpdateRotationMatrix(Transform.WorldRotation);
        }

        public void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, this.Texture);

            Vector2 pos1 = new Vector2(Boundary.MinX + Offset.X, Boundary.MinY + Offset.Y);
            Vector2 pos2 = new Vector2(Boundary.MaxX + Offset.X, Boundary.MinY + Offset.Y);
            Vector2 pos3 = new Vector2(Boundary.MaxX + Offset.X, Boundary.MaxY + Offset.Y);
            Vector2 pos4 = new Vector2(Boundary.MinX + Offset.X, Boundary.MaxY + Offset.Y);

            if (Transform != null)
            {
                pos1.Transform(Transform.TransformMatrix);
                pos2.Transform(Transform.TransformMatrix);
                pos3.Transform(Transform.TransformMatrix);
                pos4.Transform(Transform.TransformMatrix);
            }

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(TexCoords.MinX, TexCoords.MinY);
            GL.Vertex2(pos1);
            GL.TexCoord2(TexCoords.MaxX, TexCoords.MinY);
            GL.Vertex2(pos2);
            GL.TexCoord2(TexCoords.MaxX, TexCoords.MaxY);
            GL.Vertex2(pos3);
            GL.TexCoord2(TexCoords.MinX, TexCoords.MaxY);
            GL.Vertex2(pos4);
            GL.End();
        }

        public void SetTexture(string name)
        {
            Texture = TextureTools.LoadFromResource(name);
        }

        public void SetTexCoords(Rect newTexCoords)
        {
            TexCoords = newTexCoords;
        }

        public void SetOffset(float x, float y)
        {
            Offset = new Vector2(x, y);
        }

        private void UpdateRotationMatrix(float rotationAngle)
        {
            RotationMatrix = new Matrix3(
                new Vector3(MathF.Cos(rotationAngle), -MathF.Sin(rotationAngle), 0),
                new Vector3(MathF.Sin(rotationAngle), -MathF.Cos(rotationAngle), 0),
                new Vector3(0, 0, 1));
        }
    }
}
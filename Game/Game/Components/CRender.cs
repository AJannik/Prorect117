﻿using System;
using Game.SimpleGeometry;
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
            Transform = new CTransform();
        }

        public GameObject MyGameObject { get; set; } = null;

        public int Layer { get; set; }

        private int Texture { get; set; }

        private float SizeX { get; set; } = 1f;

        private float SizeY { get; set; } = 1f;

        private Rect TexCoords { get; set; }

        private CTransform Transform { get; set; }

        private Vector2 Offset { get; set; }

        public void Update(float deltaTime)
        {
            // pull Transform from object
            Transform = MyGameObject.Transform;
        }

        public void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, this.Texture);

            Vector2 pos1 = new Vector2((-SizeX / 2) + Offset.X, (-SizeY / 2) + Offset.Y);
            Vector2 pos2 = new Vector2((SizeX / 2) + Offset.X, (-SizeY / 2) + Offset.Y);
            Vector2 pos3 = new Vector2((SizeX / 2) + Offset.X, (SizeY / 2) + Offset.Y);
            Vector2 pos4 = new Vector2((-SizeX / 2) + Offset.X, (SizeY / 2) + Offset.Y);

            pos1 = Transformation.Transform(pos1, Transform.TransformMatrix);
            pos2 = Transformation.Transform(pos2, Transform.TransformMatrix);
            pos3 = Transformation.Transform(pos3, Transform.TransformMatrix);
            pos4 = Transformation.Transform(pos4, Transform.TransformMatrix);

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

        public void SetSize(float sizeX, float sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
        }
    }
}
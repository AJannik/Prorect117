﻿using Game.Components.Renderer;
using Game.Entity;
using Game.Interfaces;
using Game.SimpleGeometry;
using Game.Tools;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components.UI.BaseComponents
{
    public class CImageRender : IComponent, IDrawable, IGuiElement
    {
        private Matrix4 matrix = Matrix4.Identity;

        private bool transformSize = true;

        public CImageRender()
        {
            Texture = TextureTools.LoadFromResource("Content.default.png");
            GL.BindTexture(TextureTarget.Texture2D, Texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public GameObject MyGameObject { get; set; } = null;

        public int Layer { get; set; } = 30;

        public CCanvas Canvas { get; set; } = null;

        public bool Visible { get; set; } = true;

        public int Texture { get; private set; }

        public Color TintColor { get; set; } = Color.White;

        private float SizeX { get; set; } = 1f;

        private float SizeY { get; set; } = 1f;

        private Rect TexCoords { get; set; } = new Rect(0f, 0f, 1f, 1f);

        public void Draw(float alpha)
        {
            if (!Visible)
            {
                return;
            }

            matrix = Matrix4.Identity;
            GL.LoadMatrix(ref matrix);
            GL.BindTexture(TextureTarget.Texture2D, Texture);
            GL.Color3(TintColor);

            if (transformSize)
            {
                TransformSize();
            }

            Vector2 pos1 = new Vector2(-SizeX / 2f, -SizeY / 2f);
            Vector2 pos2 = new Vector2(SizeX / 2f, -SizeY / 2f);
            Vector2 pos3 = new Vector2(SizeX / 2f, SizeY / 2f);
            Vector2 pos4 = new Vector2(-SizeX / 2f, SizeY / 2f);

            // transform the corners
            pos1 = Transformation.Transform(pos1, MyGameObject.Transform.WorldTransformMatrix);
            pos2 = Transformation.Transform(pos2, MyGameObject.Transform.WorldTransformMatrix);
            pos3 = Transformation.Transform(pos3, MyGameObject.Transform.WorldTransformMatrix);
            pos4 = Transformation.Transform(pos4, MyGameObject.Transform.WorldTransformMatrix);

            // Draw
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
            matrix = Canvas.Camera.CameraMatrix;
            GL.LoadMatrix(ref matrix);
            GL.Color3(Color.White);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void LoadAndSetTexture(string name)
        {
            Texture = TextureTools.LoadFromResource(name);

            // set options
            GL.BindTexture(TextureTarget.Texture2D, Texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void SetTexture(int tex)
        {
            Texture = tex;
        }

        public void SetTexCoords(Rect newTexCoords)
        {
            TexCoords = newTexCoords;
        }

        public void SetSize(float sizeX, float sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            transformSize = true;
        }

        private void TransformSize()
        {
            Vector2 size = new Vector2(SizeX, SizeY);
            size = Transformation.Transform(size, Canvas.CanvasDrawMatrix);
            SizeX = size.X;
            SizeY = size.Y;
            transformSize = false;
        }
    }
}
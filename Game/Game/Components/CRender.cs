using System;
using Game.Interfaces;
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
            Texture = TextureTools.LoadFromResource("Content.default.png");
            TexCoords = new Rect(0f, 0f, 1f, 1f);
            Offset = new Vector2(0, 0);
        }

        public GameObject MyGameObject { get; set; } = null;

        public int Layer { get; set; } = 10;

        public bool Flipped { get; set; } = false;

        public bool Visible { get; set; } = true;

        public int Texture { get; private set; }

        public Color TintColor { get; set; } = Color.White;

        private float SizeX { get; set; } = 1f;

        private float SizeY { get; set; } = 1f;

        private Rect TexCoords { get; set; }

        private Vector2 Offset { get; set; }

        public void Update(float deltaTime)
        {
        }

        public void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, this.Texture);
            GL.Color3(TintColor);

            Vector2 pos1;
            Vector2 pos2;
            Vector2 pos3;
            Vector2 pos4;

            // calculate the corners
            if (Flipped)
            {
                pos1 = new Vector2((-SizeX / 2) - Offset.X, (-SizeY / 2) + Offset.Y);
                pos2 = new Vector2((SizeX / 2) - Offset.X, (-SizeY / 2) + Offset.Y);
                pos3 = new Vector2((SizeX / 2) - Offset.X, (SizeY / 2) + Offset.Y);
                pos4 = new Vector2((-SizeX / 2) - Offset.X, (SizeY / 2) + Offset.Y);
            }
            else
            {
                pos1 = new Vector2((-SizeX / 2) + Offset.X, (-SizeY / 2) + Offset.Y);
                pos2 = new Vector2((SizeX / 2) + Offset.X, (-SizeY / 2) + Offset.Y);
                pos3 = new Vector2((SizeX / 2) + Offset.X, (SizeY / 2) + Offset.Y);
                pos4 = new Vector2((-SizeX / 2) + Offset.X, (SizeY / 2) + Offset.Y);
            }

            // transform the corners
            pos1 = Transformation.Transform(pos1, MyGameObject.Transform.WorldTransformMatrix);
            pos2 = Transformation.Transform(pos2, MyGameObject.Transform.WorldTransformMatrix);
            pos3 = Transformation.Transform(pos3, MyGameObject.Transform.WorldTransformMatrix);
            pos4 = Transformation.Transform(pos4, MyGameObject.Transform.WorldTransformMatrix);

            // Draw (flipped)
            GL.Begin(PrimitiveType.Quads);
            if (Flipped)
            {
                GL.TexCoord2(TexCoords.MaxX, TexCoords.MinY);
                GL.Vertex2(pos1);
                GL.TexCoord2(TexCoords.MinX, TexCoords.MinY);
                GL.Vertex2(pos2);
                GL.TexCoord2(TexCoords.MinX, TexCoords.MaxY);
                GL.Vertex2(pos3);
                GL.TexCoord2(TexCoords.MaxX, TexCoords.MaxY);
                GL.Vertex2(pos4);
            }
            else
            {
                GL.TexCoord2(TexCoords.MinX, TexCoords.MinY);
                GL.Vertex2(pos1);
                GL.TexCoord2(TexCoords.MaxX, TexCoords.MinY);
                GL.Vertex2(pos2);
                GL.TexCoord2(TexCoords.MaxX, TexCoords.MaxY);
                GL.Vertex2(pos3);
                GL.TexCoord2(TexCoords.MinX, TexCoords.MaxY);
                GL.Vertex2(pos4);
            }

            GL.End();
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
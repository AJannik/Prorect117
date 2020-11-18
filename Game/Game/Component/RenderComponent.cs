using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;
using Game.Tools;
using OpenTK.Graphics.OpenGL;

namespace Game
{
    internal class RenderComponent : IComponent
    {
        public RenderComponent()
        {
            // texture = TextureTools.LoadFromResource("Content.default.png");
            TexCoords = new Rect(0f, 0f, 1f, 1f);
        }

        private int Texture { get; set; }

        private Rect Boundary { get; set; }

        private Rect TexCoords { get; set; }

        public GameObject MyGameObject { get; set; } = null;

        // private Transform transform { get; }
        public void Update(float deltaTime)
        {
            // update logic
            // Boundary.MinX = Transform.X;
            // Boundary.MinY = Transform.Y;
            Draw();
        }

        public void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, this.Texture);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(this.TexCoords.MinX, this.TexCoords.MinY);
            GL.Vertex2(this.Boundary.MinX, this.Boundary.MinY);
            GL.TexCoord2(this.TexCoords.MaxX, this.TexCoords.MinY);
            GL.Vertex2(this.Boundary.MaxX, this.Boundary.MinY);
            GL.TexCoord2(this.TexCoords.MaxX, this.TexCoords.MaxY);
            GL.Vertex2(this.Boundary.MaxX, this.Boundary.MaxY);
            GL.TexCoord2(this.TexCoords.MinX, this.TexCoords.MaxY);
            GL.Vertex2(this.Boundary.MinX, this.Boundary.MaxY);
            GL.End();
        }

        public void SetTexture(string name)
        {
            Texture = TextureTools.LoadFromResource(name);
        }
    }
}
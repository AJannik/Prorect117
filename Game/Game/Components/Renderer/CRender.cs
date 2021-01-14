using Game.Interfaces;
using Game.SimpleGeometry;
using Game.Tools;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components.Renderer
{
    public class CRender : IComponent, IDrawable
    {
        public CRender()
        {
            Texture = TextureTools.LoadFromResource("Content.default.png");
            GL.BindTexture(TextureTarget.Texture2D, Texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public GameObject MyGameObject { get; set; } = null;

        public int Layer { get; set; } = 10;

        public bool Flipped { get; set; } = false;

        public bool Visible { get; set; } = true;

        public int Texture { get; private set; }

        public Color TintColor { get; set; } = Color.White;

        private float SizeX { get; set; } = 1f;

        private float SizeY { get; set; } = 1f;

        private Rect TexCoords { get; set; } = new Rect(0f, 0f, 1f, 1f);

        private Vector2 Offset { get; set; } = new Vector2(0, 0);

        // Used for RenderBlending
        private Vector2 Oldpos1 { get; set; } = Vector2.Zero;

        private Vector2 Oldpos2 { get; set; } = Vector2.Zero;

        private Vector2 Oldpos3 { get; set; } = Vector2.Zero;

        private Vector2 Oldpos4 { get; set; } = Vector2.Zero;

        private bool OldFlipped { get; set; } = false;

        public void Draw(float alpha)
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture);
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

            // Blend between old frame and alpha towards current frame
            Vector2 newpos1 = pos1;
            Vector2 newpos2 = pos2;
            Vector2 newpos3 = pos3;
            Vector2 newpos4 = pos4;
            if (OldFlipped == Flipped)
            {
                newpos1 = (pos1 * alpha) + (Oldpos1 * (1f - alpha));
                newpos2 = (pos2 * alpha) + (Oldpos2 * (1f - alpha));
                newpos3 = (pos3 * alpha) + (Oldpos3 * (1f - alpha));
                newpos4 = (pos4 * alpha) + (Oldpos4 * (1f - alpha));
            }

            // Draw (flipped)
            GL.Begin(PrimitiveType.Quads);
            if (Flipped)
            {
                GL.TexCoord2(TexCoords.MaxX, TexCoords.MinY);
                GL.Vertex2(newpos1);
                GL.TexCoord2(TexCoords.MinX, TexCoords.MinY);
                GL.Vertex2(newpos2);
                GL.TexCoord2(TexCoords.MinX, TexCoords.MaxY);
                GL.Vertex2(newpos3);
                GL.TexCoord2(TexCoords.MaxX, TexCoords.MaxY);
                GL.Vertex2(newpos4);
            }
            else
            {
                GL.TexCoord2(TexCoords.MinX, TexCoords.MinY);
                GL.Vertex2(newpos1);
                GL.TexCoord2(TexCoords.MaxX, TexCoords.MinY);
                GL.Vertex2(newpos2);
                GL.TexCoord2(TexCoords.MaxX, TexCoords.MaxY);
                GL.Vertex2(newpos3);
                GL.TexCoord2(TexCoords.MinX, TexCoords.MaxY);
                GL.Vertex2(newpos4);
            }

            // Update old positions for RenderBlending
            Oldpos1 = pos1;
            Oldpos2 = pos2;
            Oldpos3 = pos3;
            Oldpos4 = pos4;
            OldFlipped = Flipped;

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
using Game.Tools;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CRender : IComponent
    {
        public CRender()
        {
            Texture = TextureTools.LoadFromResource("Content.default.png");
            TexCoords = new Rect(0f, 0f, 1f, 1f);
        }

        public GameObject MyGameObject { get; set; } = null;

        public int Layer { get; set; }

        private int Texture { get; set; }

        private Rect Boundary { get; set; } = new Rect(1f, 1f, 1f, 1f);

        private Rect TexCoords { get; set; }

        private CTransform Transform { get; set; }

        public void Update(float deltaTime)
        {
            // pull Transform from object
            Transform = MyGameObject.Transform;

            Boundary.MinX = Transform.Position.X;
            Boundary.MinY = Transform.Position.Y;
        }

        public void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, this.Texture);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(TexCoords.MinX, TexCoords.MinY);
            GL.Vertex2(Boundary.MinX, Boundary.MinY);
            GL.TexCoord2(TexCoords.MaxX, TexCoords.MinY);
            GL.Vertex2(Boundary.MaxX, Boundary.MinY);
            GL.TexCoord2(TexCoords.MaxX, TexCoords.MaxY);
            GL.Vertex2(Boundary.MaxX, Boundary.MaxY);
            GL.TexCoord2(TexCoords.MinX, TexCoords.MaxY);
            GL.Vertex2(Boundary.MinX, Boundary.MaxY);
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
    }
}
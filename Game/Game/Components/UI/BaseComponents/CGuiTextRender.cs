using System.Collections.Generic;
using System.Text;
using Game.Components.Renderer;
using Game.Entity;
using Game.Interfaces;
using Game.SimpleGeometry;
using Game.Tools;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components.UI.BaseComponents
{
    public class CGuiTextRender : IComponent, IGuiElement, IDrawable
    {
        private Matrix4 matrix = Matrix4.Identity;

        private bool transformSize = true;

        public CGuiTextRender()
        {
            LoadAndSetSpriteSheet("Content.Font.png");
        }

        public GameObject MyGameObject { get; set; } = null;

        public CCanvas Canvas { get; set; } = null;

        public int Layer { get; set; } = 30;

        public int SpriteSheet { get; set; }

        public string Text { get; set; } = string.Empty;

        public Vector2 Offset { get; set; } = Vector2.Zero;

        public Color FontColor { get; set; } = Color.White;

        public bool Centered { get; set; } = false;

        public uint FirstCharacter { get; set; } = 32;

        public uint CharactersPerColumn { get; set; } = 12;

        public uint CharactersPerRow { get; set; } = 8;

        private Vector2 Size { get; set; } = new Vector2(0.4f, 0.4f);

        public void LoadAndSetSpriteSheet(string name)
        {
            SpriteSheet = TextureTools.LoadFromResource(name);

            // set options
            GL.BindTexture(TextureTarget.Texture2D, SpriteSheet);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Draw(float alpha)
        {
            GL.Color3(Color.White);

            if (transformSize)
            {
                TransformSize();
            }

            var rect = new Rect(Vector2.Zero, Size); // rectangle of the first character
            if (Centered)
            {
                rect = new Rect(-(Text.Length * Size.X) / 2f, 0f, Size.X, Size.Y);
            }

            foreach (var spriteId in StringToSpriteIds(Text, FirstCharacter))
            {
                var texCoords = CalcTexCoords(spriteId, CharactersPerRow, CharactersPerColumn);
                DrawSingle(rect, texCoords);
                rect.Center = new Vector2(rect.Center.X + rect.Size.X, rect.Center.Y);
            }
        }

        public void SetSize(float size)
        {
            Size = new Vector2(size, size);
            transformSize = true;
        }

        private void DrawSingle(Rect rect, Rect texCoords)
        {
            matrix = Matrix4.Identity;
            GL.LoadMatrix(ref matrix);
            GL.BindTexture(TextureTarget.Texture2D, SpriteSheet);
            GL.Color3(FontColor);

            // calculate the corners
            Vector2 pos1 = new Vector2(rect.MinX + Offset.X, rect.MinY + Offset.Y);
            Vector2 pos2 = new Vector2(rect.MaxX + Offset.X, rect.MinY + Offset.Y);
            Vector2 pos3 = new Vector2(rect.MaxX + Offset.X, rect.MaxY + Offset.Y);
            Vector2 pos4 = new Vector2(rect.MinX + Offset.X, rect.MaxY + Offset.Y);

            // transform the corners
            pos1 = Transformation.Transform(pos1, MyGameObject.Transform.WorldTransformMatrix);
            pos2 = Transformation.Transform(pos2, MyGameObject.Transform.WorldTransformMatrix);
            pos3 = Transformation.Transform(pos3, MyGameObject.Transform.WorldTransformMatrix);
            pos4 = Transformation.Transform(pos4, MyGameObject.Transform.WorldTransformMatrix);

            // Draw
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(texCoords.MinX, texCoords.MinY);
            GL.Vertex2(pos1);
            GL.TexCoord2(texCoords.MaxX, texCoords.MinY);
            GL.Vertex2(pos2);
            GL.TexCoord2(texCoords.MaxX, texCoords.MaxY);
            GL.Vertex2(pos3);
            GL.TexCoord2(texCoords.MinX, texCoords.MaxY);
            GL.Vertex2(pos4);

            GL.End();
            matrix = Canvas.Camera.CameraMatrix;
            GL.LoadMatrix(ref matrix);
            GL.Color3(Color.White);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void TransformSize()
        {
            Size = Transformation.Transform(Size, Canvas.CanvasDrawMatrix);
            transformSize = false;
        }

        private Rect CalcTexCoords(uint spriteId, uint columns, uint rows)
        {
            uint row = spriteId / columns;
            uint col = spriteId % columns;

            float x = col / (float)columns;
            float y = 1f - ((row + 1f) / rows);
            float width = 1f / columns;
            float height = 1f / rows;

            return new Rect(x, y, width, height);
        }

        private IEnumerable<uint> StringToSpriteIds(string text, uint firstCharacter)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(text);
            foreach (var asciiCharacter in asciiBytes)
            {
                yield return asciiCharacter - firstCharacter;
            }
        }
    }
}

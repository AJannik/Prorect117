using System;
using System.Collections.Generic;
using System.Text;
using Game.Interfaces;
using Game.SimpleGeometry;
using Game.Tools;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components.Renderer
{
    public class CTextRender : IComponent, IDrawable
    {
        public CTextRender()
        {
            LoadAndSetSpriteSheet("Content.Font.png");
        }

        public GameObject MyGameObject { get; set; } = null;

        public int Layer { get; set; } = 20;

        public int SpriteSheet { get; set; }

        public string Text { get; set; } = string.Empty;

        public float Size { get; set; } = 0.4f;

        public Vector2 Offset { get; set; } = Vector2.Zero;

        public Color FontColor { get; set; } = Color.White;

        public bool Centered { get; set; } = false;

        public uint FirstCharacter { get; set; } = 32;

        public uint CharactersPerColumn { get; set; } = 12;

        public uint CharactersPerRow { get; set; } = 8;

        // Used for RenderBlending
        private Vector2 Oldpos1 { get; set; } = Vector2.Zero;

        private Vector2 Oldpos2 { get; set; } = Vector2.Zero;

        private Vector2 Oldpos3 { get; set; } = Vector2.Zero;

        private Vector2 Oldpos4 { get; set; } = Vector2.Zero;

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
            var rect = new Rect(0, 0, Size, Size); // rectangle of the first character
            if (Centered)
            {
                rect = new Rect(-(Text.Length * Size) / 2f, 0, Size, Size);
            }

            int i = 0;
            foreach (var spriteId in StringToSpriteIds(Text, FirstCharacter))
            {
                var texCoords = CalcTexCoords(spriteId, CharactersPerRow, CharactersPerColumn);
                DrawSingle(rect, texCoords, alpha, i);
                rect.Center = new Vector2(rect.Center.X + rect.Size.X, rect.Center.Y);
                i++;
            }
        }

        private void DrawSingle(Rect rect, Rect texCoords, float alpha, int i)
        {
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

            // Blend between old frame and alpha towards current frame
            Vector2 newpos1 = (pos1 * alpha) + ((Oldpos1 + (rect.Size * i)) * (1f - alpha));
            Vector2 newpos2 = (pos2 * alpha) + ((Oldpos2 + (rect.Size * i)) * (1f - alpha));
            Vector2 newpos3 = (pos3 * alpha) + ((Oldpos3 + (rect.Size * i)) * (1f - alpha));
            Vector2 newpos4 = (pos4 * alpha) + ((Oldpos4 + (rect.Size * i)) * (1f - alpha));

            // Draw (flipped)
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(texCoords.MinX, texCoords.MinY);
            GL.Vertex2(pos1);
            GL.TexCoord2(texCoords.MaxX, texCoords.MinY);
            GL.Vertex2(pos2);
            GL.TexCoord2(texCoords.MaxX, texCoords.MaxY);
            GL.Vertex2(pos3);
            GL.TexCoord2(texCoords.MinX, texCoords.MaxY);
            GL.Vertex2(pos4);

            if (i == 0)
            {
                // Update old positions for RenderBlending
                Oldpos1 = pos1;
                Oldpos2 = pos2;
                Oldpos3 = pos3;
                Oldpos4 = pos4;
            }

            GL.End();
            GL.Color3(Color.White);
            GL.BindTexture(TextureTarget.Texture2D, 0);
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

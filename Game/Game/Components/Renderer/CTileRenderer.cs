using Game.Interfaces;
using Game.SimpleGeometry;
using Game.Tools;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Components.Renderer
{
    public class CTileRenderer : IComponent, IDrawable
    {
        public CTileRenderer()
        {
            LoadAndSetTexture("Content.WallSpritesheet.png");
        }

        public GameObject MyGameObject { get; set; } = null;

        public int Texture { get; set; }

        public int Layer { get; set; } = 10;

        public Color TintColor { get; set; } = Color.White;

        public int Width { get; set; } = 3;

        public int Height { get; set; } = 3;

        public int TopLeftTileIndex { get; set; } = 0;

        private int TileSetColumns { get; set; } = 3;

        private int TileSetRows { get; set; } = 3;

        private float TileSize { get; set; } = 0.5f;

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

        public void Draw(float alpha)
        {
            for (float x = 0; x < Width; x += TileSize)
            {
                for (float y = 0; y < Height; y += TileSize)
                {
                    Vector2 position = new Vector2(x - (Width / 2f), y - (Height / 2f));
                    if (x == 0)
                    {
                        if (y == 0)
                        {
                            DrawSingleTile(position, TopLeftTileIndex + (2 * TileSetColumns));
                        }
                        else if (y == Height - TileSize)
                        {
                            DrawSingleTile(position, TopLeftTileIndex);
                        }
                        else
                        {
                            DrawSingleTile(position, TopLeftTileIndex + TileSetColumns);
                        }
                    }
                    else if (x == Width - TileSize)
                    {
                        if (y == 0)
                        {
                            DrawSingleTile(position, TopLeftTileIndex + (2 * TileSetColumns) + 2);
                        }
                        else if (y == Height - TileSize)
                        {
                            DrawSingleTile(position, TopLeftTileIndex + 2);
                        }
                        else
                        {
                            DrawSingleTile(position, TopLeftTileIndex + TileSetColumns + 2);
                        }
                    }
                    else
                    {
                        if (y == 0)
                        {
                            DrawSingleTile(position, TopLeftTileIndex + (2 * TileSetColumns) + 1);
                        }
                        else if (y == Height - TileSize)
                        {
                            DrawSingleTile(position, TopLeftTileIndex + 1);
                        }
                        else
                        {
                            DrawSingleTile(position, TopLeftTileIndex + TileSetColumns + 1);
                        }
                    }
                }
            }
        }

        private void DrawSingleTile(Vector2 offset, int tileID)
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture);
            GL.Color3(TintColor);

            Vector2 pos1;
            Vector2 pos2;
            Vector2 pos3;
            Vector2 pos4;

            // calculate the corners and TexCoords
            pos1 = new Vector2(offset.X, offset.Y);
            pos2 = new Vector2(offset.X + TileSize, offset.Y);
            pos3 = new Vector2(offset.X + TileSize, offset.Y + TileSize);
            pos4 = new Vector2(offset.X, offset.Y + TileSize);
            Rect texCoords = CalculateTexCoords(tileID);

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
            GL.Color3(Color.White);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private Rect CalculateTexCoords(int tileID)
        {
            int activeRow = tileID / TileSetColumns;
            int activeColumn = tileID % TileSetColumns;

            return new Rect(activeColumn / (float)TileSetColumns, 1f - ((activeRow + 1f) / TileSetRows), 1f / TileSetColumns, 1f / TileSetRows);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Game.SimpleGeometry;
using OpenTK.Graphics.OpenGL;

namespace Game.Components
{
    public class CAnmimationController : IComponent
    {
        public CAnmimationController()
        {
            Rows = 1;
            Columns = 1;
        }

        public GameObject MyGameObject { get; set; } = null;

        public Rect TexCoords { get; private set; }

        public int ActiveRow { get; private set; }

        public int ActiveColumn { get; private set; }

        public int Frames { get; private set; }

        private CRender Renderer { get; set; }

        private int Rows { get; set; }

        private int Columns { get; set; }

        public void Update(float deltaTime)
        {
            Renderer = MyGameObject.GetComponent<CRender>();
            if (Renderer == null)
            {
                return;
            }

            TexCoords = CalculateTexCoords();
            Renderer.SetTexCoords(TexCoords);
        }

        public void SetRowsAndColumns(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Frames = rows * columns;
        }

        public void SetActiveFrame(int frame)
        {
            frame = frame % Frames;
            ActiveRow = (frame % Rows) + 1;
            ActiveColumn = (frame / Columns) + 1;
        }

        private Rect CalculateTexCoords()
        {
            return new Rect((1 / Rows) * (ActiveRow - 1), (1 / Columns) * (ActiveColumn - 1), (1 / Rows) * ActiveRow, (1 / Columns) * ActiveColumn);
        }
    }
}

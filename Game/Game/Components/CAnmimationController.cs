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

        private CRender Renderer { get; set; }

        private Rect TexCoords { get; set; }

        private int Rows { get; set; }

        private int Columns { get; set; }

        private int ActiveRow { get; set; }

        private int ActiveColumn { get; set; }

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
        }

        private Rect CalculateTexCoords()
        {
            // TODO: calc texCoords
            return new Rect(0, 0, 1, 1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Tools
{
    public class Rect
    {
        public Rect(float minX, float minY, float sizeX, float sizeY)
        {
            this.MinX = minX;
            this.MinY = minY;
            this.SizeX = sizeX;
            this.SizeY = sizeY;
        }

        public float MinX { get; set; }

        public float MinY { get; set; }

        public float MaxX => this.MinX + this.SizeX;

        public float MaxY => this.MinY + this.SizeY;

        public float SizeX { get; set; }

        public float SizeY { get; set; }
    }
}

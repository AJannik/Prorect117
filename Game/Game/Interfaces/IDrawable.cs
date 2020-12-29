using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Interfaces
{
    public interface IDrawable
    {
        public int Layer { get; set; }

        public void Draw();
    }
}

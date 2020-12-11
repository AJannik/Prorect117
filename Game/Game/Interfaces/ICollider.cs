using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Interfaces
{
    public interface ICollider
    {
        public bool IsTrigger { get; set; }

        public ISimpleGeometry Geometry { get; set; }

        public void Draw();
    }
}
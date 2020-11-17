using System;
using System.Collections.Generic;
using System.Numerics;

namespace Game.Component
{
    public class Transform : IComponent
    {
        public Vector2 Position { get; set; }

        public GameObject GameObject { get; set; }

        public Transform()
        {
            this.Position = new Vector2(0, 0);
        }

        public void Update()
        {
            return;
        }
    }
}

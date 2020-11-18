using System;
using System.Collections.Generic;
using System.Numerics;

namespace Game.Component
{
    public class CTransform : IComponent
    {
        public CTransform()
        {
            Position = new Vector2(0, 0);
        }

        public Vector2 Position { get; set; }

        public GameObject MyGameObject { get; set; }

        public void Update(float deltaTime)
        {
            return;
        }
    }
}

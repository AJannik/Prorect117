using System;
using System.Collections.Generic;
using System.Numerics;

namespace Game.Component
{
    public class CTransform : IComponent
    {
        private Vector2 pos = new Vector2(0f, 0f); // local coordinates

        public CTransform()
        {
            Position = new Vector2(0, 0);
        }

        public Vector2 Position // world coordinates
        {
            get
            {
                if (MyGameObject.GetParent() != null)
                {
                    return pos + MyGameObject.GetParent().Transform.Position;
                }

                return pos;
            }

            set
            {
                pos = value;
            }
        }

        public GameObject MyGameObject { get; set; }

        public void Update(float deltaTime)
        {
            return;
        }
    }
}

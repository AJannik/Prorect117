using System;
using System.Collections.Generic;
using System.Text;
using Game.Interfaces;

namespace Game.Components
{
    public class CPowerDownScript : IComponent
    {
        public CPowerDownScript()
        {
        }

        public GameObject MyGameObject { get; set; } = null;

        public CCircleCollider Collider { get; set; }

        public void Update(float deltaTime)
        {
            // if (Collider.TriggerEntered)
            {
                // Apply Effect
            }
        }
    }
}

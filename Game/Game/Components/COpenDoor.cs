using System;
using System.Collections.Generic;
using System.Text;
using Game.Interfaces;

namespace Game.Components
{
    public class COpenDoor : IComponent, IUpdateable
    {
        public GameObject MyGameObject { get; set; }

        public bool IsOpen { get; set; } = false;

        public void Update(float deltaTime)
        {
        }
    }
}

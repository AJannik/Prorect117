using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public interface IComponent
    {
        public GameObject MyGameObject { get; set; }

        void Update(float deltaTime);
    }
}

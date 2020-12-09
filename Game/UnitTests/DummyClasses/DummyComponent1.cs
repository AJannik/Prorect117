using Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.DummyClasses
{
    public class DummyComponent1 : IComponent
    {
        public GameObject MyGameObject { get; set; } = null;

        public void Update(float deltaTime)
        {            
        }
    }
}
using Game;
using Game.Interfaces;
using System;

namespace UnitTests.DummyClasses
{
    public class DummyComponent2 : IComponent
    {
        public GameObject MyGameObject { get; set; } = null;

        public void Update(float deltaTime)
        {
            if (!MyGameObject.getActive()) return;
            throw new NotImplementedException();
        }
    }
}
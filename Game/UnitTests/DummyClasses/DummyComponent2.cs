using Game;
using Game.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.DummyClasses
{
    [ExcludeFromCodeCoverage]
    public class DummyComponent2 : IComponent, IUpdateable
    {
        public GameObject MyGameObject { get; set; } = null;

        public void Update(float deltaTime)
        {
            if (!MyGameObject.Active) return;
        }
    }
}
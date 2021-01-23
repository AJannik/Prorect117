using System.Diagnostics.CodeAnalysis;
using Game;
using Game.Interfaces;

namespace UnitTests.DummyClasses
{
    [ExcludeFromCodeCoverage]
    public class DummyComponent1 : IComponent, IUpdateable
    {
        public GameObject MyGameObject { get; set; } = null;

        public void Update(float deltaTime)
        {
            if (!MyGameObject.Active) return;
        }
    }
}
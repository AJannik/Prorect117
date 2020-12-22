using Game;
using Game.Interfaces;

namespace UnitTests.DummyClasses
{
    public class DummyComponent1 : IComponent
    {
        public GameObject MyGameObject { get; set; } = null;

        public void Update(float deltaTime)
        {
            if (!MyGameObject.Active) return;
        }
    }
}
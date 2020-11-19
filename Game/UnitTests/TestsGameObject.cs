using Game;
using Game.Component;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TestsGameObject
    {
        [TestMethod]
        public void TestComponentStoring()
        {
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<CTransform>();
            gameObject.AddComponent<CRender>();

            // Test adding two different components
            int numberComponents = gameObject.GetNumberComponents(); ;
            Assert.AreEqual(2, numberComponents);

            // Test getting desired component of two different components
            Assert.IsNotNull(gameObject.GetComponent<CTransform>());

            // Test removing one component while keeping one
            gameObject.RemoveComponent<CTransform>();
            Assert.AreEqual(1, gameObject.GetNumberComponents());

            // Test adding a second component of the same type as already existing
            gameObject.AddComponent<CRender>();
            Assert.AreEqual(2, gameObject.GetNumberComponents());

            // Testing what happens when you try to remove a component that doesn't exist
            gameObject.RemoveComponent<CTransform>();
            Assert.AreEqual(2, gameObject.GetNumberComponents());

            // Check if you get the desired component even if there are more then one components of the same type
            Assert.IsNotNull(gameObject.GetComponent<CRender>());

            // Check if you get null when trying to get a not existing component
            Assert.IsNull(gameObject.GetComponent<CTransform>());
        }
    }
}

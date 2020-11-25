using Game;
using Game.Components;
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

            // Test add component to empty list
            gameObject.AddComponent<CTransform>();
            GetComponentCTransform(gameObject);

            // Test add component to not empty list
            gameObject.AddComponent<CRender>();
            GetComponentCRender(gameObject);

            // Test all added components stored
            CheckNumberComponents(gameObject, 2);

            // Test remove component of type
            gameObject.RemoveComponent<CTransform>();
            CheckNumberComponents(gameObject, 1);

            // Test add component of already existing type
            gameObject.AddComponent<CRender>();
            CheckNumberComponents(gameObject, 2);

            // Test removing not existing component
            gameObject.RemoveComponent<CTransform>();            
            CheckNumberComponents(gameObject, 2);

            // Check if you get null when trying to get a not existing component
            Assert.IsNull(gameObject.GetComponent<CTransform>());
        }

        private void CheckNumberComponents(GameObject gameObject, int num)
        {
            Assert.AreEqual(num, gameObject.GetNumberComponents());
        }

        private void GetComponentCTransform(GameObject gameObject)
        {
            // Check if you get the desired component even if there are more then one components of the same type
            Assert.IsNotNull(gameObject.GetComponent<CTransform>());
        }

        private void GetComponentCRender(GameObject gameObject)
        {
            // Check if you get the desired component even if there are more then one components of the same type
            Assert.IsNotNull(gameObject.GetComponent<CRender>());
        }

        private void TestRemovingNotExistingComponent(GameObject gameObject)
        {
            gameObject.RemoveComponent<CTransform>();
            Assert.AreEqual(2, gameObject.GetNumberComponents());
        }
    }
}
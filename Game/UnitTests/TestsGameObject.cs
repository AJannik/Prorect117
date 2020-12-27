using Game;
using Game.SceneSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using UnitTests.DummyClasses;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TestsGameObject
    {
        /*
         *
         ### Component Storing ###
         *
        */
        [TestMethod]
        public void TestComponentStoring()
        {
            Scene scene = new Scene();
            GameObject gameObject = new GameObject(scene);

            AddComponentToEmptyList(gameObject);
            GetOneComponent(gameObject);
            AddComponentToNotEmptyList(gameObject);
            CheckNumberComponents(gameObject);
            RemoveOneComponent(gameObject);
            AddComponentToAlreadyExistingType(gameObject);
            CheckGetMultipleComponents(gameObject);
            RemoveNotExistingComponent(gameObject);
            TryGettingNotExistingComponent(gameObject);
            RemoveGivenComponent(gameObject);
            TestActiveProperty(gameObject);
        }

        [TestMethod]
        private void AddComponentToEmptyList(GameObject gameObject)
        {
            gameObject.AddComponent<DummyComponent1>();
            Assert.AreEqual(1, gameObject.GetNumberComponents());
        }

        [TestMethod]
        private void GetOneComponent(GameObject gameObject)
        {            
            Assert.IsNotNull(gameObject.GetComponent<DummyComponent1>());
        }

        [TestMethod]
        private void AddComponentToNotEmptyList(GameObject gameObject)
        {
            gameObject.AddComponent<DummyComponent2>();
            Assert.IsNotNull(gameObject.GetComponent<DummyComponent2>());
        }

        [TestMethod]
        private void CheckNumberComponents(GameObject gameObject)
        {
            Assert.AreEqual(2, gameObject.GetNumberComponents());
        }

        [TestMethod]
        private void RemoveOneComponent(GameObject gameObject)
        {
            gameObject.RemoveComponent<DummyComponent1>();
            Assert.AreEqual(1, gameObject.GetNumberComponents());
            Assert.IsNull(gameObject.GetComponent<DummyComponent1>());
        }

        [TestMethod]
        private void AddComponentToAlreadyExistingType(GameObject gameObject)
        {
            gameObject.AddComponent<DummyComponent2>();
            Assert.AreEqual(2, gameObject.GetNumberComponents());
            Assert.IsNotNull(gameObject.GetComponent<DummyComponent2>());
        }

        [TestMethod]
        private void CheckGetMultipleComponents(GameObject gameObject)
        {
            Assert.AreEqual(2, gameObject.GetComponents<DummyComponent2>().Length);
        }

        [TestMethod]
        private void RemoveNotExistingComponent(GameObject gameObject)
        {
            gameObject.RemoveComponent<DummyComponent1>();
            Assert.AreEqual(2, gameObject.GetNumberComponents());            
        }

        [TestMethod]
        private void TryGettingNotExistingComponent(GameObject gameObject)
        {
            Assert.IsNull(gameObject.GetComponent<DummyComponent1>());
        }

        [TestMethod]
        private void RemoveGivenComponent(GameObject gameObject)
        {
            // Test removing component T of given type
            DummyComponent2 dummy = gameObject.GetComponents<DummyComponent2>()[1];
            gameObject.RemoveComponent(dummy);
            Assert.AreEqual(1, gameObject.GetNumberComponents());
            Assert.AreNotEqual(dummy, gameObject.GetComponent<DummyComponent2>());
        }

        [TestMethod]
        private void TestActiveProperty(GameObject gameObject)
        {
            //Test setting component inactive or active
            if (gameObject.GetNumberComponents() > 0)
            {
                DummyComponent1 dummy = gameObject.GetComponent<DummyComponent1>();
                if(dummy == null) return;
                dummy.MyGameObject.Active = false;
                Assert.IsFalse(dummy.MyGameObject.Active);
                if (dummy.MyGameObject.ChildCount > 0)
                {
                    Assert.IsFalse(dummy.MyGameObject.GetChild(0).Active);
                }

                if (dummy.MyGameObject.GetParent() != null && dummy.MyGameObject.GetParent() != dummy.MyGameObject)
                {
                    Assert.IsTrue(dummy.MyGameObject.GetParent().Active);
                }
            }
        }

        /*
         * 
         ### Parent-Child hierarchy ###
         * 
        */
        [TestMethod]
        public void TestParentHierarchy()
        {
            Scene scene = new Scene();
            GameObject parent = new GameObject(scene);
            GameObject child1 = new GameObject(scene, parent);
            GameObject child2 = new GameObject(scene);

            // check if child1 is a child of parent
            Assert.AreEqual(child1, parent.GetChild(0));

            // check if parent is the Parent of child1
            Assert.AreEqual(parent, child1.GetParent());

            // set parent as Parent of child2
            child2.SetParent(parent);

            // check if parent now has two childs
            Assert.AreEqual(2, parent.ChildCount);

            // check if you can get all children of a Parent
            Assert.AreEqual(2, parent.GetAllChildren().Length);

            // remove parent from Parent of child1
            child1.RemoveParent();

            // check if parent has only one child
            Assert.AreEqual(1, parent.ChildCount);

            // check if child1 has no parent anymore
            Assert.IsNull(child1.GetParent());

            // set child2 as Parent of child1
            child1.SetParent(child2);

            // check if child1 was added to child2, a child of parent
            Assert.AreEqual(1, parent.ChildCount);

            // check the if the Parent of the Parent of child1 is parent
            Assert.AreEqual(parent, child1.GetParent().GetParent());

            // check that you can't add a child of a Parent as a Parent (hierarchy cycle)
            Assert.IsFalse(parent.SetParent(child1));
            Assert.AreEqual(0, child1.ChildCount);
        }
    }
}
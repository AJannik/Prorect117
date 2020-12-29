using System;
using Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using UnitTests.DummyClasses;
using Game.SceneSystem;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TestsScene
    {
        [TestMethod]
        public void TestAddGameObjects()
        {
            Scene scene = new Scene();
            GameObject gameObject1 = new GameObject(scene);
            GameObject gameObject2 = new GameObject(scene);
            GameObject gameObject3 = new GameObject(scene);

            Assert.AreEqual(3, scene.GetGameObjects().Count);
        }

        [TestMethod]
        public void TestRemoveGameObjects()
        {
            Scene scene = new Scene();
            GameObject gameObject1 = new GameObject(scene);
            GameObject gameObject2 = new GameObject(scene);
            GameObject gameObject3 = new GameObject(scene);

            scene.RemoveGameObject(gameObject3);
            scene.Update(0f);

            Assert.AreEqual(2, scene.GetGameObjects().Count);
        }

        [TestMethod]
        public void TestAddComponents()
        {
            Scene scene = new Scene();
            GameObject gameObject1 = new GameObject(scene);

            gameObject1.AddComponent<DummyComponent1>();
            gameObject1.AddComponent<DummyComponent1>();

            Assert.AreEqual(2, scene.GetUpdateables().Count);
        }

        [TestMethod]
        public void TestRemoveComponent()
        {
            Scene scene = new Scene();
            GameObject gameObject1 = new GameObject(scene);

            gameObject1.AddComponent<DummyComponent1>();
            gameObject1.AddComponent<DummyComponent1>();
            gameObject1.RemoveComponent<DummyComponent1>();

            Assert.AreEqual(1, scene.GetUpdateables().Count);
        }

        [TestMethod]
        public void TestRemoveGameObjectWithComponents()
        {
            Scene scene = new Scene();
            GameObject gameObject1 = new GameObject(scene);
            GameObject gameObject2 = new GameObject(scene);

            gameObject1.AddComponent<DummyComponent1>();
            gameObject2.AddComponent<DummyComponent1>();
            scene.RemoveGameObject(gameObject2);
            scene.Update(0f);
            
            Assert.AreEqual(1, scene.GetUpdateables().Count);            
        }
    }
}
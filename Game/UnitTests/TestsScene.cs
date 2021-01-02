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
            GameManager gameManager = new GameManager();
            Scene scene = new Scene(gameManager);
            GameObject gameObject1 = new GameObject(scene);
            GameObject gameObject2 = new GameObject(scene);
            GameObject gameObject3 = new GameObject(scene);

            Assert.AreEqual(3, scene.GetGameObjects().Count);
        }

        [TestMethod]
        public void TestRemoveGameObjects()
        {
            GameManager gameManager = new GameManager();
            Scene scene = new Scene(gameManager);
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
            GameManager gameManager = new GameManager();
            Scene scene = new Scene(gameManager);
            GameObject gameObject1 = new GameObject(scene);

            gameObject1.AddComponent<DummyComponent1>();
            gameObject1.AddComponent<DummyComponent1>();

            Assert.AreEqual(2, scene.GetUpdateables().Count);
        }

        [TestMethod]
        public void TestRemoveComponent()
        {
            GameManager gameManager = new GameManager();
            Scene scene = new Scene(gameManager);
            GameObject gameObject1 = new GameObject(scene);

            gameObject1.AddComponent<DummyComponent1>();
            gameObject1.AddComponent<DummyComponent1>();
            gameObject1.RemoveComponent<DummyComponent1>();

            Assert.AreEqual(1, scene.GetUpdateables().Count);
        }

        [TestMethod]
        public void TestRemoveGameObjectWithComponents()
        {
            GameManager gameManager = new GameManager();
            Scene scene = new Scene(gameManager);
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
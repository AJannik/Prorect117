using System;
using Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using Game.Components;

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

            Assert.AreEqual(2, scene.GetGameObjects().Count);
            Assert.IsFalse(scene.GetGameObjects().Contains(gameObject3));
        }

        [TestMethod]
        public void TestAddComponents()
        {
            Scene scene = new Scene();
            GameObject gameObject1 = new GameObject(scene);

            gameObject1.AddComponent<CRender>();
            gameObject1.AddComponent<CRender>();

            Assert.AreEqual(2, scene.GetCRenders().Count);
        }

        [TestMethod]
        public void TestRemoveComponent()
        {
            Scene scene = new Scene();
            GameObject gameObject1 = new GameObject(scene);

            gameObject1.AddComponent<CRender>();
            gameObject1.AddComponent<CRender>();
            gameObject1.RemoveComponent<CRender>();

            Assert.AreEqual(1, scene.GetCRenders().Count);
        }

        [TestMethod]
        public void TestRemoveGameObjectWithComponents()
        {
            Scene scene = new Scene();
            GameObject gameObject1 = new GameObject(scene);
            GameObject gameObject2 = new GameObject(scene);

            gameObject1.AddComponent<CRender>();
            gameObject2.AddComponent<CRender>();
            scene.RemoveGameObject(gameObject2);
            
            Assert.AreEqual(1, scene.GetCRenders().Count);            
        }

        [TestMethod]
        public void TestCRenderLayerOrder()
        {
            Scene scene = new Scene();
            GameObject gameObject1 = new GameObject(scene);
            Random rn = new Random();

            gameObject1.AddComponent<CRender>();
            gameObject1.AddComponent<CRender>();
            gameObject1.AddComponent<CRender>();

            // set random layers to render components so we can test if scene sorts the renderers list correctly
            CRender[] renderers = gameObject1.GetComponents<CRender>();
            foreach (CRender item in renderers)
            {
                item.Layer = rn.Next(0, 4);
            }

            scene.Update(0);

            List<CRender> cRenders = scene.GetCRenders();
            for (int i = 1; i < cRenders.Count; i++)
            {
                Assert.IsTrue(cRenders[i - 1].Layer <= cRenders[i].Layer);
            }
        }
    }
}
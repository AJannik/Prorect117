using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnitTests.DummyClasses;
using Game.Components.Renderer;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TestsAnimationController
    {
        [TestMethod]
        public void TestCalculateTexCoords1x1()
        {
            DummyGameObject dummy = DummyGameObject.BuildDummy();
            dummy.AddComponent<CAnimationSystem>();
            CAnimationSystem acontroller = dummy.GetComponent<CAnimationSystem>();
            acontroller.SetColumnsAndRows(1, 1);
            acontroller.SetActiveFrame(0);
            acontroller.Update(1f);
            Assert.IsTrue(acontroller.TexCoords.MinX == 0.0f);
            Assert.IsTrue(acontroller.TexCoords.MinY == 0.0f);
            Assert.IsTrue(acontroller.TexCoords.MaxX == 1f);
            Assert.IsTrue(acontroller.TexCoords.MaxY == 1f);
        }

        [TestMethod]
        public void TestCalculateTexCoords4x1()
        {
            DummyGameObject dummy = DummyGameObject.BuildDummy();
            dummy.AddComponent<CAnimationSystem>();
            CAnimationSystem acontroller = dummy.GetComponent<CAnimationSystem>();
            acontroller.SetColumnsAndRows(4, 1);
            acontroller.SetActiveFrame(1);
            acontroller.Update(0f);
            Assert.IsTrue(acontroller.TexCoords.MinX == 0.25f);
            Assert.IsTrue(acontroller.TexCoords.MinY == 0.0f);
            Assert.IsTrue(acontroller.TexCoords.MaxX == 0.5f);
            Assert.IsTrue(acontroller.TexCoords.MaxY == 1f);
        }

        [TestMethod]
        public void TestCalculateTexCoords1x4()
        {
            DummyGameObject dummy = DummyGameObject.BuildDummy();
            dummy.AddComponent<CAnimationSystem>();
            CAnimationSystem acontroller = dummy.GetComponent<CAnimationSystem>();
            acontroller.SetColumnsAndRows(1, 4);
            acontroller.SetActiveFrame(3);
            acontroller.Update(0f);
            Assert.IsTrue(acontroller.TexCoords.MinX == 0.0f);
            Assert.IsTrue(acontroller.TexCoords.MinY == 0.0f);
            Assert.IsTrue(acontroller.TexCoords.MaxX == 1.0f);
            Assert.IsTrue(acontroller.TexCoords.MaxY == 0.25f);
        }
    }
}

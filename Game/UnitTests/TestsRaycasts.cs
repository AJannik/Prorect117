using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using OpenTK;
using Game.SimpleGeometry;
using Game.Tools;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TestsRaycasts
    {
        [TestMethod]
        public void TestRaycastImpalesCircle()
        {
            // Raycast enters and exits circle
            Ray ray = new Ray(new Vector2(-0.9f, 0.0f), new Vector2(1f, 0f), 10f);
            Circle circle = new Circle(new Vector2(0f, 0f), 0.2f);

            Assert.IsTrue(CollisionCheck.CircleAndLine(circle, ray));
        }

        [TestMethod]
        public void TestRaycastPokeCircle()
        {
            // Raycast only enters circle, but doesn't exit
            Ray ray = new Ray(new Vector2(-0.9f, 0.0f), new Vector2(1f, 0f), 0.9f);
            Circle circle = new Circle(new Vector2(0f, 0f), 0.2f);

            Assert.IsTrue(CollisionCheck.CircleAndLine(circle, ray));
        }

        [TestMethod]
        public void TestRaycastExitsCircle()
        {
            // Raycast only exits circle, it starts inside the circle
            Ray ray = new Ray(new Vector2(0f, 0f), new Vector2(1f, 0f), 0.9f);
            Circle circle = new Circle(new Vector2(0f, 0f), 0.2f);

            Assert.IsTrue(CollisionCheck.CircleAndLine(circle, ray));
        }

        [TestMethod]
        public void TestRaycastMissedCircle()
        {
            Ray ray = new Ray(new Vector2(-0.9f, 0.9f), new Vector2(1f, 0f), 10f);
            Circle circle = new Circle(new Vector2(0f, 0f), 0.2f);

            Assert.IsFalse(CollisionCheck.CircleAndLine(circle, ray));
        }

        [TestMethod]
        public void TestRaycastFallsShortCircle()
        {
            // Raycast stops before reaching circle
            Ray ray = new Ray(new Vector2(-0.9f, 0f), new Vector2(1f, 0f), 0.5f);
            Circle circle = new Circle(new Vector2(0f, 0f), 0.2f);

            Assert.IsFalse(CollisionCheck.CircleAndLine(circle, ray));
        }

        [TestMethod]
        public void TestRaycastPastCircle()
        {
            // Raycast starts behind circle
            Ray ray = new Ray(new Vector2(0.9f, 0f), new Vector2(1f, 0f), 0.5f);
            Circle circle = new Circle(new Vector2(0f, 0f), 0.2f);

            Assert.IsFalse(CollisionCheck.CircleAndLine(circle, ray));
        }

        [TestMethod]
        public void TestRaycastInsideCircle()
        {
            // Raycast starts inside circle but doesn't exit either
            Ray ray = new Ray(new Vector2(-0.1f, 0f), new Vector2(1f, 0f), 0.1f);
            Circle circle = new Circle(new Vector2(0f, 0f), 0.2f);

            Assert.IsFalse(CollisionCheck.CircleAndLine(circle, ray));
        }

        [TestMethod]
        public void TestRaycastImpale1Aabb()
        {
            Ray ray = new Ray(new Vector2(-0.9f, 0f), new Vector2(1f, 0f), 10f);
            Rect rect = new Rect(-0.1f, -0.1f, 0.2f, 0.2f);

            Assert.IsTrue(CollisionCheck.AabbAndLine(rect, ray));
        }

        [TestMethod]
        public void TestRaycastImpale2Aabb()
        {
            Ray ray = new Ray(new Vector2(-0.9f, 0.9f), new Vector2(1f, -1f), 10f);
            Rect rect = new Rect(-0.1f, -0.1f, 0.2f, 0.2f);

            Assert.IsTrue(CollisionCheck.AabbAndLine(rect, ray));
        }

        [TestMethod]
        public void TestRaycastExitsAabb()
        {
            Ray ray = new Ray(new Vector2(0f, 0f), new Vector2(1f, 0f), 10f);
            Rect rect = new Rect(-0.1f, -0.1f, 0.2f, 0.2f);

            Assert.IsTrue(CollisionCheck.AabbAndLine(rect, ray));
        }

        [TestMethod]
        public void TestRaycastMissesAabb()
        {
            Ray ray = new Ray(new Vector2(-0.9f, 0.9f), new Vector2(1f, 0f), 10f);
            Rect rect = new Rect(-0.1f, -0.1f, 0.2f, 0.2f);

            Assert.IsFalse(CollisionCheck.AabbAndLine(rect, ray));
        }

        [TestMethod]
        public void TestRaycastFallsShortAabb()
        {
            Ray ray = new Ray(new Vector2(-0.9f, 0f), new Vector2(1f, 0f), 0.5f);
            Rect rect = new Rect(-0.1f, -0.1f, 0.2f, 0.2f);

            Assert.IsFalse(CollisionCheck.AabbAndLine(rect, ray));
        }

        [TestMethod]
        public void TestRaycastPastAabb()
        {
            Ray ray = new Ray(new Vector2(0.5f, 0f), new Vector2(1f, 0f), 10f);
            Rect rect = new Rect(-0.1f, -0.1f, 0.2f, 0.2f);

            Assert.IsFalse(CollisionCheck.AabbAndLine(rect, ray));
        }

        [TestMethod]
        public void TestRaycastInsideAabb()
        {
            Ray ray = new Ray(new Vector2(-0.1f, 0f), new Vector2(1f, 0f), 0.1f);
            Rect rect = new Rect(-0.2f, -0.2f, 0.4f, 0.4f);

            Assert.IsFalse(CollisionCheck.AabbAndLine(rect, ray));
        }
    }
}
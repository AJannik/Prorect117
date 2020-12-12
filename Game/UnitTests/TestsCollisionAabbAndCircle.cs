using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Game.SimpleGeometry;
using Game.Tools;
using OpenTK;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TestsCollisionAabbAndCircle
    {
        [TestMethod]
        public void TestAabbAndAabbMiss()
        {
            Rect rect1 = new Rect(-1f, -1f, 0.1f, 0.1f);
            Rect rect2 = new Rect(1f, 1f, 0.1f, 0.1f);

            Assert.IsFalse(CollisionCheck.AabbAndAabb(rect1, rect2 , Vector2.Zero));
        }

        [TestMethod]
        public void TestAabbAndAabbTouch()
        {
            Rect rect1 = new Rect(-1f, -1f, 0.1f, 0.1f);
            Rect rect2 = new Rect(-1f, -0.9f, 0.1f, 0.1f);

            Assert.IsTrue(CollisionCheck.AabbAndAabb(rect1, rect2, Vector2.Zero));
        }

        [TestMethod]
        public void TestAabbAndAabbCollide()
        {
            Rect rect1 = new Rect(0f, 0f, 0.4f, 0.4f);
            Rect rect2 = new Rect(0.2f, 0.2f, 0.2f, 0.4f);

            Assert.IsTrue(CollisionCheck.AabbAndAabb(rect1, rect2, Vector2.Zero));
        }

        [TestMethod]
        public void TestCircleAndCircleMiss()
        {
            Circle circle1 = new Circle(new Vector2(-1f, -1f), 0.1f);
            Circle circle2 = new Circle(new Vector2(1f, 1f), 0.1f);

            Assert.IsFalse(CollisionCheck.CircleAndCircle(circle1, circle2));
        }

        [TestMethod]
        public void TestCircleAndCircleTouch()
        {
            Circle circle1 = new Circle(new Vector2(0f, 0f), 0.1f);
            Circle circle2 = new Circle(new Vector2(0.2f, 0f), 0.1f);

            Assert.IsTrue(CollisionCheck.CircleAndCircle(circle1, circle2));
        }

        [TestMethod]
        public void TestCircleAndCircleHit()
        {
            Circle circle1 = new Circle(new Vector2(0f, 0f), 0.4f);
            Circle circle2 = new Circle(new Vector2(0.2f, 0f), 0.1f);

            Assert.IsTrue(CollisionCheck.CircleAndCircle(circle1, circle2));
        }

        [TestMethod]
        public void TestAabbAndCircleMiss()
        {
            Circle circle = new Circle(new Vector2(0f, 0f), 0.4f);
            Rect rect = new Rect(-1f, -1f, 0.1f, 0.1f);

            Assert.IsFalse(CollisionCheck.AabbAndCircle(rect, circle, Vector2.Zero));
        }

        [TestMethod]
        public void TestAabbAndCircleTouch()
        {
            Circle circle = new Circle(new Vector2(0f, 0f), 0.3f);
            Rect rect = new Rect(-0.5f, 0f, 0.2f, 0.2f);

            Assert.IsTrue(CollisionCheck.AabbAndCircle(rect, circle, Vector2.UnitX * -0.1f));
        }

        [TestMethod]
        public void TestAabbAndCircleHit()
        {
            Circle circle = new Circle(new Vector2(0f, 0f), 0.4f);
            Rect rect = new Rect(-0.5f, 0f, 0.4f, 0.4f);

            Assert.IsTrue(CollisionCheck.AabbAndCircle(rect, circle, Vector2.Zero));
        }
    }
}
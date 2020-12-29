using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using OpenTK;
using Game.SimpleGeometry;
using Game.Physics.RaycastSystem;

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
            RaycastHit hit = new RaycastHit();
            Circle circle = new Circle(new Vector2(0f, 0f), 0.2f);

            Assert.IsTrue(RaycastCollisionCheck.HandleRaycastCollision(circle, ray, hit));
        }

        [TestMethod]
        public void TestRaycastPokeCircle()
        {
            // Raycast only enters circle, but doesn't exit
            Ray ray = new Ray(new Vector2(-0.9f, 0.0f), new Vector2(1f, 0f), 0.9f);
            RaycastHit hit = new RaycastHit();
            Circle circle = new Circle(new Vector2(0f, 0f), 0.2f);

            Assert.IsTrue(RaycastCollisionCheck.HandleRaycastCollision(circle, ray, hit));
        }

        [TestMethod]
        public void TestRaycastExitsCircle()
        {
            // Raycast only exits circle, it starts inside the circle
            Ray ray = new Ray(new Vector2(0f, 0f), new Vector2(1f, 0f), 0.9f);
            RaycastHit hit = new RaycastHit();
            Circle circle = new Circle(new Vector2(0f, 0f), 0.2f);

            Assert.IsTrue(RaycastCollisionCheck.HandleRaycastCollision(circle, ray, hit));
        }

        [TestMethod]
        public void TestRaycastMissedCircle()
        {
            Ray ray = new Ray(new Vector2(-0.9f, 0.9f), new Vector2(1f, 0f), 10f);
            RaycastHit hit = new RaycastHit();
            Circle circle = new Circle(new Vector2(0f, 0f), 0.2f);

            Assert.IsFalse(RaycastCollisionCheck.HandleRaycastCollision(circle, ray, hit));
        }

        [TestMethod]
        public void TestRaycastFallsShortCircle()
        {
            // Raycast stops before reaching circle
            Ray ray = new Ray(new Vector2(-0.9f, 0f), new Vector2(1f, 0f), 0.5f);
            RaycastHit hit = new RaycastHit();
            Circle circle = new Circle(new Vector2(0f, 0f), 0.2f);

            Assert.IsFalse(RaycastCollisionCheck.HandleRaycastCollision(circle, ray, hit));
        }

        [TestMethod]
        public void TestRaycastPastCircle()
        {
            // Raycast starts behind circle
            Ray ray = new Ray(new Vector2(0.9f, 0f), new Vector2(1f, 0f), 0.5f);
            RaycastHit hit = new RaycastHit();
            Circle circle = new Circle(new Vector2(0f, 0f), 0.2f);

            Assert.IsFalse(RaycastCollisionCheck.HandleRaycastCollision(circle, ray, hit));
        }

        [TestMethod]
        public void TestRaycastInsideCircle()
        {
            // Raycast starts inside circle but doesn't exit either
            Ray ray = new Ray(new Vector2(-0.1f, 0f), new Vector2(1f, 0f), 0.1f);
            RaycastHit hit = new RaycastHit();
            Circle circle = new Circle(new Vector2(0f, 0f), 0.2f);

            Assert.IsFalse(RaycastCollisionCheck.HandleRaycastCollision(circle, ray, hit));
        }

        [TestMethod]
        public void TestRaycastImpale1Aabb()
        {
            Ray ray = new Ray(new Vector2(-0.9f, 0f), new Vector2(1f, 0f), 10f);
            RaycastHit hit = new RaycastHit();
            Rect rect = new Rect(-0.1f, -0.1f, 0.2f, 0.2f);

            Assert.IsTrue(RaycastCollisionCheck.HandleRaycastCollision(rect, ray, hit));
        }

        [TestMethod]
        public void TestRaycastImpale2Aabb()
        {
            Ray ray = new Ray(new Vector2(-0.9f, 0.9f), new Vector2(1f, -1f), 10f);
            RaycastHit hit = new RaycastHit();
            Rect rect = new Rect(-0.1f, -0.1f, 0.2f, 0.2f);

            Assert.IsTrue(RaycastCollisionCheck.HandleRaycastCollision(rect, ray, hit));
        }

        [TestMethod]
        public void TestRaycastExitsAabb()
        {
            Ray ray = new Ray(new Vector2(0f, 0f), new Vector2(1f, 0f), 10f);
            RaycastHit hit = new RaycastHit();
            Rect rect = new Rect(-0.1f, -0.1f, 0.2f, 0.2f);

            Assert.IsTrue(RaycastCollisionCheck.HandleRaycastCollision(rect, ray, hit));
        }

        [TestMethod]
        public void TestRaycastMissesAabb()
        {
            Ray ray = new Ray(new Vector2(-0.9f, 0.9f), new Vector2(1f, 0f), 10f);
            RaycastHit hit = new RaycastHit();
            Rect rect = new Rect(-0.1f, -0.1f, 0.2f, 0.2f);

            Assert.IsFalse(RaycastCollisionCheck.HandleRaycastCollision(rect, ray, hit));
        }

        [TestMethod]
        public void TestRaycastFallsShortAabb()
        {
            Ray ray = new Ray(new Vector2(-0.9f, 0f), new Vector2(1f, 0f), 0.5f);
            RaycastHit hit = new RaycastHit();
            Rect rect = new Rect(-0.1f, -0.1f, 0.2f, 0.2f);

            Assert.IsFalse(RaycastCollisionCheck.HandleRaycastCollision(rect, ray, hit));
        }

        [TestMethod]
        public void TestRaycastPastAabb()
        {
            Ray ray = new Ray(new Vector2(0.5f, 0f), new Vector2(1f, 0f), 10f);
            RaycastHit hit = new RaycastHit();
            Rect rect = new Rect(-0.1f, -0.1f, 0.2f, 0.2f);

            Assert.IsFalse(RaycastCollisionCheck.HandleRaycastCollision(rect, ray, hit));
        }

        [TestMethod]
        public void TestRaycastInsideAabb()
        {
            Ray ray = new Ray(new Vector2(-0.1f, 0f), new Vector2(1f, 0f), 0.1f); 
            RaycastHit hit = new RaycastHit();
            Rect rect = new Rect(-0.2f, -0.2f, 0.4f, 0.4f);

            Assert.IsFalse(RaycastCollisionCheck.HandleRaycastCollision(rect, ray, hit));
        }
    }
}
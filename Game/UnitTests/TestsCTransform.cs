﻿using Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using OpenTK;
using System;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TestsCTransform
    {
        [TestMethod]
        public void TestSetAndGetSimplePosition()
        {
            GameObject gameObject = new GameObject();
            Vector2 pos = new Vector2(2.5f, 3f);

            gameObject.Transform.Position = pos;
            Assert.AreEqual(pos, gameObject.Transform.Position);
        }

        [TestMethod]
        public void TestSetAndGetSimpleRotation()
        {
            GameObject gameObject = new GameObject();
            float rad = 1.3f;

            gameObject.Transform.Rotation = rad;
            Assert.AreEqual(rad, gameObject.Transform.Rotation);
        }

        [TestMethod]
        public void TestSetAndGetSimpleScale()
        {
            GameObject gameObject = new GameObject();
            Vector2 scale = new Vector2(2f, 1.5f);

            gameObject.Transform.Scale = scale;
            Assert.AreEqual(scale, gameObject.Transform.Scale);
        }

        [TestMethod]
        public void TestGetPositionOfChild()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject(parent);
            Vector2 posParent = new Vector2(2f, 1f);
            Vector2 posChild = new Vector2(1f, 3f);

            parent.Transform.Position = posParent;
            child.Transform.Position = posChild;

            Vector2 realPosition = posChild + posParent;

            Assert.AreEqual(realPosition, child.Transform.WorldPosition);
        }

        [TestMethod]
        public void TestGetScaleOfChild()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject(parent);
            Vector2 scaleParent = new Vector2(2f, 1f);
            Vector2 scaleChild = new Vector2(1f, 3f);

            parent.Transform.Scale = scaleParent;
            child.Transform.Scale = scaleChild;

            Vector2 realScale = scaleChild * scaleParent;

            Assert.AreEqual(realScale, child.Transform.WorldScale);
        }

        [TestMethod]
        public void TestGetRotationOfChild()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject(parent);
            float rotParent = 1f;
            float rotChild = 1f;

            parent.Transform.Rotation = rotParent;
            child.Transform.Rotation = rotChild;

            rotParent = rotParent * 180 / MathF.PI;
            rotChild = rotChild * 180 / MathF.PI;

            float realRotation = rotChild + rotParent;
            float rot = child.Transform.WorldRotation * 180 / MathF.PI;

            Assert.AreEqual(realRotation, rot);
        }

        [TestMethod]
        public void TestGetPositionOfChildFromRotatedParent()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject(parent);
            float rotParent = 1f;
            Vector2 posChild = new Vector2(2f, 1f);

            parent.Transform.Rotation = rotParent;
            child.Transform.Position = posChild;

            Vector2 realPosition = new Vector2(0.2391336f, 2.2232442f); // manually tested
            Assert.AreEqual(realPosition, child.Transform.WorldPosition);
        }

        [TestMethod]
        public void TestGetPositionOfChildFromScaledParent()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject(parent);
            Vector2 scaleParent = new Vector2(0.5f, 2f);
            Vector2 posChild = new Vector2(2f, 1f);

            parent.Transform.Scale = scaleParent;
            child.Transform.Position = posChild;

            Vector2 realPosition = new Vector2(1f, 2f); // manually tested
            Assert.AreEqual(realPosition, child.Transform.WorldPosition);
        }

        [TestMethod]
        public void TestGetPositionOfChildFromMovedRotatedScaledParent()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject(parent);
            Vector2 posParent = new Vector2(1.5f, -2f);
            float rotParent = 10 * (MathF.PI / 180); // radians
            Vector2 scaleParent = new Vector2(0.5f, 2f);

            Vector2 posChild = new Vector2(-1.7f, 3f);

            parent.Transform.Scale = scaleParent;
            parent.Transform.Position = posParent;
            parent.Transform.Rotation = rotParent;
            child.Transform.Position = posChild;

            Vector2 realPosition = new Vector2(-0.37897563f, 3.7612453f); // manually tested
            Assert.AreEqual(realPosition, child.Transform.WorldPosition);
        }
    }
}
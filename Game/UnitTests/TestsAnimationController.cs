using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Game.Components;
using Game;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TestsAnimationController
    {
        //[TestMethod]
        public void TestSetActiveFrame()
        {
            CAnmimationController controller = new CAnmimationController();

            controller.SetRowsAndColumns(2, 2);
            controller.SetActiveFrame(4);
            Console.WriteLine(controller.ActiveColumn + controller.ActiveRow);
            Assert.IsTrue(controller.ActiveRow == 2 && controller.ActiveColumn == 2);
        }

        //[TestMethod]
        public void TestCalculateTexCoords()
        {
            Scene scene = new Scene();
            GameObject gameObject = new GameObject(scene);
            gameObject.AddComponent<CAnmimationController>();

            CAnmimationController controller = gameObject.GetComponent<CAnmimationController>();
            controller.SetActiveFrame(4);
            controller.Update(1f);
            Assert.IsTrue(controller.TexCoords == new Game.SimpleGeometry.Rect(0.5f,0.5f,1,1));
        }
    }
}

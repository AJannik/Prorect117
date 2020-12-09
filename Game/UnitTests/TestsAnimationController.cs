using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Game.Components;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TestsAnimationController
    {
        [TestMethod]
        public void TestSetActiveFrame()
        {
            CAnmimationController controller = new CAnmimationController();

            controller.SetRowsAndColumns(2, 2);
            controller.SetActiveFrame(4);

            Assert.IsTrue(controller.ActiveRow == 2 && controller.ActiveColumn == 2);
        }

        [TestMethod]
        public void TestCalculateTexCoords()
        {
            CAnmimationController controller = new CAnmimationController();

            controller.SetRowsAndColumns(2, 2);
            controller.SetActiveFrame(4);

            Assert.IsTrue(controller.TexCoords == new Game.SimpleGeometry.Rect(0.5f,0.5f,1,1));
        }
    }
}

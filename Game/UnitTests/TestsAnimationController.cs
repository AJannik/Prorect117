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
        public void TestCalculateTexCoords()
        {
            CAnmimationController acontroller = new CAnmimationController();
            acontroller.SetColumnsAndRows(2, 2);
            acontroller.SetActiveFrame(3);
            acontroller.Update(1f);
            Assert.IsTrue(acontroller.TexCoords.MinX == 0.5f);
            Assert.IsTrue(acontroller.TexCoords.MinY == 0.0f);
            Assert.IsTrue(acontroller.TexCoords.MaxX == 1f);
            Assert.IsTrue(acontroller.TexCoords.MaxY == 0.5f);
        }
    }
}

using System;
using System.Diagnostics.CodeAnalysis;
using Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestsProject
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Add2ints()
        {
            Temp temp = new Temp();
            int result = temp.Add2ints(3, 2);
            Assert.AreEqual(5, result);
        }
    }
}

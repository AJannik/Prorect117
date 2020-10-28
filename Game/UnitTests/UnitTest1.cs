using Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
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

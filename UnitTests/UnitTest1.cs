using Microsoft.VisualStudio.TestTools.UnitTesting;
using Game;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Add2int()
        {
            Temp temp = new Temp();
            int result = temp.Add2int(2, 3);
            Assert.AreEqual(5, result);
        }
    }
}

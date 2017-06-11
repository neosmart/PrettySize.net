using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoSmart.PrettySize;

namespace NeoSmart.PrettySize.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSingular()
        {
            Assert.AreEqual("0 bytes", PrettySize.Format(0));
            Assert.AreEqual("0 bytes", PrettySize.Format(0, CalculationBase.Base10));
            Assert.AreEqual("1 byte", PrettySize.Format(1));
            Assert.AreEqual("1 byte", PrettySize.Format(1, CalculationBase.Base10));
            Assert.AreEqual("10 bytes", PrettySize.Format(10));
            Assert.AreEqual("10 bytes", PrettySize.Format(10, CalculationBase.Base10));
            Assert.AreEqual("1.00 KiB", PrettySize.Format(1024));
            Assert.AreEqual("1.00 KB", PrettySize.Format(1000, CalculationBase.Base10));
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeoSmart.PrettySize.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void SingularVsPlural()
        {
            Assert.AreEqual("0 bytes", PrettySize.Format(0));
            Assert.AreEqual("0 bytes", PrettySize.Format(0, UnitBase.Base10));
            Assert.AreEqual("1 byte", PrettySize.Format(1));
            Assert.AreEqual("1 byte", PrettySize.Format(1, UnitBase.Base10));
            Assert.AreEqual("10 bytes", PrettySize.Format(10));
            Assert.AreEqual("10 bytes", PrettySize.Format(10, UnitBase.Base10));
            Assert.AreEqual("1.00 KiB", PrettySize.Format(1024));
            Assert.AreEqual("1.00 KB", PrettySize.Format(1000, UnitBase.Base10));
        }

        [TestMethod]
        public void NegativeSizes()
        {
            Assert.AreEqual(-1024, PrettySize.KiB(-1).TotalBytes);
            Assert.AreEqual(0, PrettySize.KiB(-0).TotalBytes);
            Assert.AreEqual(PrettySize.KiB(-1), PrettySize.KiB(1) - PrettySize.Bytes(2048));
        }

        [TestMethod]
        public void NegativeSizeFormatting()
        {
            Assert.AreEqual("0 bytes", PrettySize.KiB(-0).ToString());
            Assert.AreEqual("-1 byte", PrettySize.Bytes(-1).ToString());
            Assert.AreEqual("-1.00 KiB", PrettySize.KiB(-1).ToString());
        }

        [TestMethod]
        public void NumericLimits()
        {
            Assert.AreEqual("8.00 EiB", PrettySize.Bytes(long.MaxValue).ToString());
            Assert.AreEqual("-8.00 EiB", PrettySize.Bytes(long.MinValue).ToString());
        }

        [TestMethod]
        public void Rounding()
        {
            Assert.AreEqual("1.50 KB", PrettySize.Format(1500, UnitBase.Base10));
            Assert.AreEqual("2.00 KB", PrettySize.Format(1999, UnitBase.Base10));
        }

        [TestMethod]
        public void Addition()
        {
            Assert.AreEqual(PrettySize.KiB(4) + PrettySize.KiB(8), PrettySize.KiB(12));
        }

        [TestMethod]
        public void Subtraction()
        {
            Assert.AreEqual(PrettySize.KiB(4) - PrettySize.KiB(2), PrettySize.KiB(2));
        }

        [TestMethod]
        public void NegativeSubtraction()
        {
            Assert.AreEqual(PrettySize.KiB(4) - PrettySize.KiB(8), PrettySize.KiB(-4));
        }

        [TestMethod]
        public void Multiplication()
        {
            Assert.AreEqual(PrettySize.Megabytes(4) * 2, PrettySize.Bytes(8_000_000));
            Assert.AreEqual(2 * PrettySize.Megabytes(4), PrettySize.Bytes(8_000_000));
        }

        [TestMethod]
        public void Division()
        {
            Assert.AreEqual(PrettySize.Megabytes(5) / 2, PrettySize.Bytes(2_500_000));
        }

        [TestMethod]
        public void Equality()
        {
            Assert.AreEqual(PrettySize.KiB(1), PrettySize.Bytes(1024));
            Assert.AreEqual(PrettySize.KiB(-1), PrettySize.Bytes(-1024));
            Assert.IsTrue(PrettySize.KiB(42) == PrettySize.Kibibytes(42));
            Assert.IsTrue(PrettySize.KiB(42) != PrettySize.Kibibytes(43));
        }

        [TestMethod]
        public void Comparison()
        {
            Assert.IsTrue(PrettySize.KiB(1) > PrettySize.KB(1));
            Assert.IsTrue(PrettySize.KiB(-2) < PrettySize.Bytes(-1));
            Assert.IsTrue(PrettySize.KiB(2) >= PrettySize.KiB(2));
            Assert.IsTrue(PrettySize.KiB(2) >= PrettySize.KiB(1));
            Assert.IsTrue(PrettySize.KiB(1) <= PrettySize.KiB(2));
            Assert.IsTrue(PrettySize.KiB(1) <= PrettySize.KiB(1));
        }
    }
}

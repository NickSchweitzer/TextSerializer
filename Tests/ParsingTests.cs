using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCodingMonkey.Serialization.Utilities;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Parsing")]
    public class ParsingTests
    {
        [TestMethod]
        public void TruncationTest()
        {
            string dontTruncate = ParsingHelper.Truncate("1234567890", 10);
            Assert.AreEqual("1234567890", dontTruncate);

            string truncate = ParsingHelper.Truncate("1234567890", 5);
            Assert.AreEqual("12345", truncate);

            string shorter = ParsingHelper.Truncate("1234567890", 25);
            Assert.AreEqual("1234567890", shorter);

            string negativeLength = ParsingHelper.Truncate("1234567890", -1);
            Assert.AreEqual("1234567890", negativeLength);
        }
    }
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCodingMonkey.Serialization.Formatters;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Formatter")]
    public class BooleanIntFormatterTests
    {
        private readonly ITextFormatter Formatter = new BooleanIntFormatter();

        [TestMethod]
        public void SerializeTest()
        {
            Assert.AreEqual("0", Formatter.Serialize(false));
            Assert.AreEqual("1", Formatter.Serialize(true));
        }

        [TestMethod]
        public void DeserializeTest()
        {
            Assert.AreEqual(false, Formatter.Deserialize("0"));
            Assert.AreEqual(true, Formatter.Deserialize("1"));
            Assert.AreEqual(true, Formatter.Deserialize("-1"));
            Assert.AreEqual(true, Formatter.Deserialize(int.MaxValue.ToString()));
            Assert.AreEqual(true, Formatter.Deserialize(int.MinValue.ToString()));
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void DeserializeInvalidBooleanTest()
        {
            Formatter.Deserialize("false");
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void DeserializeInvalidIntegerTest()
        {
            Formatter.Deserialize("one");
        }
    }
}

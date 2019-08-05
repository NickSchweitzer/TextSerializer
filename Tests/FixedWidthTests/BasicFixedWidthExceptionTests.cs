using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fixed Width")]
    public class BasicFixedWidthExceptionTests
    {
        private RecordSerializer<FixedWidthRecord> Serializer = new FixedWidthSerializer<FixedWidthRecord>();

        [TestMethod]
        public void DeserializeNullStringTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Serializer.Deserialize(null));
        }

        [TestMethod]
        public void DeserializeEmptyStringTest()
        {
            Assert.ThrowsException<TextSerializationException>(() => Serializer.Deserialize(string.Empty));
        }

        [TestMethod]
        public void DeserializeIncompleteRecordTest()
        {
            string incompleteRecord = "00001   First Record     Long Description, with a Comma03.14159";
            Assert.ThrowsException<TextSerializationException>(() => Serializer.Deserialize(incompleteRecord));
        }

        [TestMethod]
        public void DeserializeBadPaddingRecordTest()
        {
            string incompleteRecord = "00001 First Record   Long Description, with a Comma03.14159True";
            Assert.ThrowsException<TextSerializationException>(() => Serializer.Deserialize(incompleteRecord));
        }
    }
}
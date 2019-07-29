using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fixed Width")]
    public class BasicFixedWidthExceptionTests
    {
        private RecordSerializer<FixedWidthRecord> Serializer = new FixedWidthSerializer<FixedWidthRecord>();

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void DeserializeNullStringTest()
        {
            Serializer.Deserialize(null);
        }

        [TestMethod, ExpectedException(typeof(TextSerializationException))]
        public void DeserializeEmptyStringTest()
        {
            Serializer.Deserialize(string.Empty);
        }

        [TestMethod, ExpectedException(typeof(TextSerializationException))]
        public void DeserializeIncompleteRecordTest()
        {
            string incompleteRecord = "00001   First Record     Long Description, with a Comma03.14159";
            Serializer.Deserialize(incompleteRecord);
        }

        [TestMethod, ExpectedException(typeof(TextSerializationException))]
        public void DeserializeBadPaddingRecordTest()
        {
            string incompleteRecord = "00001 First Record   Long Description, with a Comma03.14159True";
            Serializer.Deserialize(incompleteRecord);
        }
    }
}
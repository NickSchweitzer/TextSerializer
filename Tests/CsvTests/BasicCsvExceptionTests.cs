using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Configuration;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("CSV")]
    public class BasicCsvExceptionTests
    {
        private RecordSerializer<CsvRecord> Serializer = new CsvSerializer<CsvRecord>();

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
            string incompleteRecord = "\"1\",\"First Record\",\"Long Description, with a Comma\",\"3.14159\"";
            Assert.ThrowsException<TextSerializationException>(() => Serializer.Deserialize(incompleteRecord));
        }

        [TestMethod]
        public void MissingFieldExceptionTest()
        {
            Assert.ThrowsException<TextSerializationConfigurationException>(() => new CsvSerializer<CsvRecordWithMissingFields>());
        }

        [TestMethod]
        public void MissingTextSerializableAttributeTest()
        {
            Assert.ThrowsException<TextSerializationConfigurationException>(() => new CsvSerializer<PocoRecord>());
        }
    }
}
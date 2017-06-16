using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Configuration;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("CSV")]
    public class BasicCsvExceptionTests
    {
        private TextSerializer<CsvRecord> Serializer = new CsvSerializer<CsvRecord>();

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
            string incompleteRecord = "\"1\",\"First Record\",\"Long Description, with a Comma\",\"3.14159\"";
            Serializer.Deserialize(incompleteRecord);
        }

        [TestMethod, ExpectedException(typeof(TextSerializationConfigurationException))]
        public void MissingFieldExceptionTest()
        {
            var serializer = new CsvSerializer<CsvRecordWithMissingFields>();
        }
    }
}
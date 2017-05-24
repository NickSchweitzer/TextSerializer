using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Tests.Helpers;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("CSV")]
    public class CsvWithHeaderTests
    {
        private CsvSerializer<CsvRecord> Serializer = new CsvSerializer<CsvRecord>();
        private string TestFile = "CsvWithHeaderFile.csv";

        [TestMethod]
        public void DeserializeArrayTest()
        {
            Helpers.Tests.DeserializeArrayWithHeaderTest(TestFile, Serializer, Records.CsvRecords);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            Helpers.Tests.DeserializeEnumerableWithHeaderTest(TestFile, Serializer, Records.CsvRecords);
        }

        [TestMethod]
        public void SerializeTest()
        {
            Helpers.Tests.SerializeWithHeaderTest(TestFile, Serializer, Records.CsvRecords);
        }

        [TestMethod]
        public void SerializeArrayTest()
        {
            Helpers.Tests.SerializeArrayWithHeaderTest(TestFile, Serializer, Records.CsvRecords);
        }

        [TestMethod, ExpectedException(typeof(TextSerializationException))]
        public void DeserializeWithoutHeaderTest()
        {
            Helpers.Tests.DeserializeArrayWithHeaderTest("CsvFile.csv", Serializer, Records.CsvRecords);
        }
    }
}
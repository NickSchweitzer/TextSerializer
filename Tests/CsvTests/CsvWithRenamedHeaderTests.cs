using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Tests.Helpers;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("CSV")]
    public class CsvWithRenamedHeaderTests
    {
        private CsvSerializer<CsvRenamedHeaderRecord> Serializer = new CsvSerializer<CsvRenamedHeaderRecord>();
        private string TestFile = "CsvWithRenamedHeaderFile.csv";

        [TestMethod]
        public void DeserializeArrayTest()
        {
            Helpers.Tests.DeserializeArrayWithHeaderTest(TestFile, Serializer, Records.CsvRenamedHeaderRecords);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            Helpers.Tests.DeserializeEnumerableWithHeaderTest(TestFile, Serializer, Records.CsvRenamedHeaderRecords);
        }

        [TestMethod]
        public void SerializeTest()
        {
            Helpers.Tests.SerializeWithHeaderTest(TestFile, Serializer, Records.CsvRenamedHeaderRecords);
        }

        [TestMethod]
        public void SerializeArrayTest()
        {
            Helpers.Tests.SerializeArrayWithHeaderTest(TestFile, Serializer, Records.CsvRenamedHeaderRecords);
        }

        [TestMethod, ExpectedException(typeof(TextSerializationException))]
        public void DeserializeWithoutHeaderTest()
        {
            Helpers.Tests.DeserializeArrayWithHeaderTest("CsvFile.csv", Serializer, Records.CsvRenamedHeaderRecords);
        }

        [TestMethod, ExpectedException(typeof(TextSerializationException))]
        public void DeserializeWithWrongHeaderTest()
        {
            Helpers.Tests.DeserializeArrayWithHeaderTest("CsvWithHeaderFile.csv", Serializer, Records.CsvRenamedHeaderRecords);
        }
    }
}
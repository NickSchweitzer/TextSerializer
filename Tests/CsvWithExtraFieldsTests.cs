using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Tests.Helpers;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("CSV")]
    public class CsvWithExtraFieldsTests
    {
        protected CsvSerializer<CsvWithExtraFieldsRecord> Serializer = new CsvSerializer<CsvWithExtraFieldsRecord>();
        protected string TestFile = "CsvFile.csv";

        [TestMethod]
        public void DeserializeArrayTest()
        {
            Helpers.Tests.DeserializeArrayTest(TestFile, Serializer, Records.CsvWithExtraFieldsRecords);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            Helpers.Tests.DeserializeEnumerableTest(TestFile, Serializer, Records.CsvWithExtraFieldsRecords);
        }

        [TestMethod]
        public void SerializeTest()
        {
            Helpers.Tests.SerializeTest(TestFile, Serializer, Records.CsvWithExtraFieldsRecords);
        }

        [TestMethod]
        public void SerializeArrayTest()
        {
            Helpers.Tests.SerializeArrayTest(TestFile, Serializer, Records.CsvWithExtraFieldsRecords);
        }
    }
}
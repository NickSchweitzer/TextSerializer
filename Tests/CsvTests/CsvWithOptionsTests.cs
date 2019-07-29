using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Tests.Helpers;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("CSV")]
    public class CsvWithOptionsTests
    {
        private CsvSerializer<CsvWithOptionsRecord> Serializer = new CsvSerializer<CsvWithOptionsRecord>();
        private string DeserializeTestFile = "CsvWithOptionsFile.csv";
        private string SerializeTestFile = "CsvFile.csv";

        [TestMethod]
        public void DeserializeArrayTest()
        {
            Helpers.Tests.DeserializeArrayTest(DeserializeTestFile, Serializer, Records.CsvWithOptionsRecords);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            Helpers.Tests.DeserializeEnumerableTest(DeserializeTestFile, Serializer, Records.CsvWithOptionsRecords);
        }

        [TestMethod]
        public void SerializeTest()
        {
            Helpers.Tests.SerializeTest(SerializeTestFile, Serializer, Records.CsvWithOptionsRecords);
        }

        [TestMethod]
        public void SerializeArrayTest()
        {
            Helpers.Tests.SerializeArrayTest(SerializeTestFile, Serializer, Records.CsvWithOptionsRecords);
        }
    }
}
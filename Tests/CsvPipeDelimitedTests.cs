using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Tests.Helpers;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("CSV")]
    public class CsvPipeDelimitedTests
    {
        private CsvSerializer<CsvRecord> Serializer = new CsvSerializer<CsvRecord>();
        private string TestFile = "CsvPipeDelimitedFile.csv";

        public CsvPipeDelimitedTests()
        {
            Serializer = new CsvSerializer<CsvRecord>
            {
                Delimiter = '|'
            };
        }

        [TestMethod]
        public void DeserializeArrayTest()
        {
            Helpers.Tests.DeserializeArrayTest(TestFile, Serializer, Records.CsvPipeDelimitedRecords);
        }

        [TestMethod]
        public void DeserializeArrayCountTest()
        {
            Helpers.Tests.DeserializeArrayTest(TestFile, Serializer, Records.CsvPipeDelimitedRecords, 2);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            Helpers.Tests.DeserializeEnumerableTest(TestFile, Serializer, Records.CsvPipeDelimitedRecords);
        }

        [TestMethod]
        public void SerializeTest()
        {
            Helpers.Tests.SerializeTest(TestFile, Serializer, Records.CsvPipeDelimitedRecords);
        }

        [TestMethod]
        public void SerializeArrayTest()
        {
            Helpers.Tests.SerializeArrayTest(TestFile, Serializer, Records.CsvPipeDelimitedRecords);
        }
    }
}
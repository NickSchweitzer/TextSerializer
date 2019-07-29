using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Helpers;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class CsvFluentEnumStringTests
    {
        protected RecordSerializer<PocoWithEnumRecord> Serializer;
        protected string TestFile = "CsvWithEnumStringFile.csv";

        public CsvFluentEnumStringTests()
        {
            Serializer = new CsvSerializer<PocoWithEnumRecord>(config => config.ByConvention());
        }

        [TestMethod]
        public void DeserializeArrayWithStringTest()
        {
            Helpers.Tests.DeserializeArrayTest(TestFile, Serializer, Records.PocoWithEnumRecords);
        }

        [TestMethod]
        public void DeserializeEnumerableWithStringTest()
        {
            Helpers.Tests.DeserializeEnumerableTest(TestFile, Serializer, Records.PocoWithEnumRecords);
        }

        [TestMethod]
        public void SerializeWithStringTest()
        {
            Helpers.Tests.SerializeTest(TestFile, Serializer, Records.PocoWithEnumRecords);
        }

        [TestMethod]
        public void SerializeArrayWithStringTest()
        {
            Helpers.Tests.SerializeArrayTest(TestFile, Serializer, Records.PocoWithEnumRecords);
        }
    }
}
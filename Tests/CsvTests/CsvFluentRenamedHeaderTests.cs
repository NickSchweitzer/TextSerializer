using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Helpers;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class CsvFluentRenamedHeaderTests
    {
        private string TestFile = "CsvWithRenamedHeaderFile.csv";
        CsvSerializer<PocoRecord> Serializer;

        public CsvFluentRenamedHeaderTests()
        {
            Serializer = new CsvSerializer<PocoRecord>(config => config.ByConvention()
                .ForMember(field => field.Id, opt => opt.Name("UniqueId")));
        }

        [TestMethod]
        public void DeserializeArrayTest()
        {
            Helpers.Tests.DeserializeArrayWithHeaderTest(TestFile, Serializer, Records.PocoRecords);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            Helpers.Tests.DeserializeEnumerableWithHeaderTest(TestFile, Serializer, Records.PocoRecords);
        }

        [TestMethod]
        public void DeserializeWithWrongHeaderTest()
        {
            Assert.ThrowsException<TextSerializationException>(() =>
                Helpers.Tests.DeserializeArrayWithHeaderTest("CsvWithHeaderFile.csv", Serializer, Records.PocoRecords));
        }

        [TestMethod]
        public void SerializeTest()
        {
            Helpers.Tests.SerializeWithHeaderTest(TestFile, Serializer, Records.PocoRecords);
        }

        [TestMethod]
        public void SerializeArrayTest()
        {
            Helpers.Tests.SerializeArrayWithHeaderTest(TestFile, Serializer, Records.PocoRecords);
        }
    }
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Helpers;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class CsvFluentConfigurationTests
    {
        protected RecordSerializer<PocoRecord> Serializer;
        protected string DeserializeTestFile = "CsvWithOptionsFile.csv";
        protected string SerializeTestFile = "CsvFile.csv";

        public CsvFluentConfigurationTests()
        {
            Serializer = new CsvSerializer<PocoRecord>(config => config
                .ForMember(field => field.Id, opt => opt.Position(0))
                .ForMember(field => field.Name, opt => opt.Position(1))
                .ForMember(field => field.Description, opt => opt.Position(2))
                .ForMember(field => field.Value, opt => opt.Position(3))
                .ForMember(field => field.Enabled, opt => opt.Optional().Position(4)));
        }

        [TestMethod]
        public void DeserializeArrayTest()
        {
            Helpers.Tests.DeserializeArrayTest(DeserializeTestFile, Serializer, Records.PocoRecords);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            Helpers.Tests.DeserializeEnumerableTest(DeserializeTestFile, Serializer, Records.PocoRecords);
        }

        [TestMethod]
        public void SerializeTest()
        {
            Helpers.Tests.SerializeTest(SerializeTestFile, Serializer, Records.PocoRecords);
        }

        [TestMethod]
        public void SerializeArrayTest()
        {
            Helpers.Tests.SerializeArrayTest(SerializeTestFile, Serializer, Records.PocoRecords);
        }
    }
}
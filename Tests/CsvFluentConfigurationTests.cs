using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Helpers;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class CsvFluentConfigurationTests
    {
        protected CsvSerializer<CsvPocoRecord> Serializer;
        protected string TestFile = "CsvWithOptionsFile.csv";

        public CsvFluentConfigurationTests()
        {
            Serializer = new CsvSerializer<CsvPocoRecord>(config => config
                .ForMember(field => field.Id, opt => opt.Position(0))
                .ForMember(field => field.Name, opt => opt.Position(1))
                .ForMember(field => field.Description, opt => opt.Position(2))
                .ForMember(field => field.Value, opt => opt.Position(3))
                .ForMember(field => field.Enabled, opt => opt.Optional().Position(4)));
        }

        [TestMethod]
        public void DeserializeArrayTest()
        {
            Helpers.Tests.DeserializeArrayTest(TestFile, Serializer, Records.CsvPocoRecords);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            Helpers.Tests.DeserializeEnumerableTest(TestFile, Serializer, Records.CsvPocoRecords);
        }

        [TestMethod]
        public void SerializeTest()
        {
            Helpers.Tests.SerializeTest("CsvFile.csv", Serializer, Records.CsvPocoRecords);
        }

        [TestMethod]
        public void SerializeArrayTest()
        {
            Helpers.Tests.SerializeArrayTest("CsvFile.csv", Serializer, Records.CsvPocoRecords);
        }
    }
}
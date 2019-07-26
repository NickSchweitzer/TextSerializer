using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Tests.Helpers;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class CsvIgnoreConfigurationTests
    {
        protected RecordSerializer<PocoWithExtraFieldsRecord> Serializer;
        protected string DeserializeTestFile = "CsvWithOptionsFile.csv";
        protected string SerializeTestFile = "CsvFile.csv";

        public CsvIgnoreConfigurationTests()
        {
            Serializer = new CsvSerializer<PocoWithExtraFieldsRecord>(config => config.ByConvention()
                .Ignore(field => field.ExtraField)
                .ForMember(field => field.Enabled, opt => opt.Optional()));
        }

        [TestMethod]
        public void DeserializeArrayTest()
        {
            Helpers.Tests.DeserializeArrayTest(DeserializeTestFile, Serializer, Records.PocoWithExtraFieldsRecords);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            Helpers.Tests.DeserializeEnumerableTest(DeserializeTestFile, Serializer, Records.PocoWithExtraFieldsRecords);
        }

        [TestMethod]
        public void SerializeTest()
        {
            Helpers.Tests.SerializeTest(SerializeTestFile, Serializer, Records.PocoWithExtraFieldsRecords);
        }

        [TestMethod]
        public void SerializeArrayTest()
        {
            Helpers.Tests.SerializeArrayTest(SerializeTestFile, Serializer, Records.PocoWithExtraFieldsRecords);
        }
    }
}
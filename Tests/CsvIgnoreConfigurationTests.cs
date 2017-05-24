using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Tests.Helpers;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class CsvIgnoreConfigurationTests
    {
        protected CsvSerializer<CsvPocoWithExtraFieldsRecord> Serializer;
        private string TestFile = "CsvWithOptionsFile.csv";

        public CsvIgnoreConfigurationTests()
        {
            Serializer = new CsvSerializer<CsvPocoWithExtraFieldsRecord>(config => config.ByConvention()
                .Ignore(field => field.ExtraField)
                .ForMember(field => field.Enabled, opt => opt.Optional()));
        }

        [TestMethod]
        public void DeserializeArrayTest()
        {
            Helpers.Tests.DeserializeArrayTest(TestFile, Serializer, Records.CsvPocoWithExtraFieldsRecords);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            Helpers.Tests.DeserializeEnumerableTest(TestFile, Serializer, Records.CsvPocoWithExtraFieldsRecords);
        }

        [TestMethod]
        public void SerializeTest()
        {
            Helpers.Tests.SerializeTest("CsvFile.csv", Serializer, Records.CsvPocoWithExtraFieldsRecords);
        }

        [TestMethod]
        public void SerializeArrayTest()
        {
            Helpers.Tests.SerializeArrayTest("CsvFile.csv", Serializer, Records.CsvPocoWithExtraFieldsRecords);
        }
    }
}
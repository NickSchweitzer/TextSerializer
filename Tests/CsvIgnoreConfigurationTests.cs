using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Tests.Helpers;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class CsvIgnoreConfigurationTests
    {
        protected CsvSerializer<PocoWithExtraFieldsRecord> Serializer;
        private string TestFile = "CsvWithOptionsFile.csv";

        public CsvIgnoreConfigurationTests()
        {
            Serializer = new CsvSerializer<PocoWithExtraFieldsRecord>(config => config.ByConvention()
                .Ignore(field => field.ExtraField)
                .ForMember(field => field.Enabled, opt => opt.Optional()));
        }

        [TestMethod]
        public void DeserializeArrayTest()
        {
            Helpers.Tests.DeserializeArrayTest(TestFile, Serializer, Records.PocoWithExtraFieldsRecords);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            Helpers.Tests.DeserializeEnumerableTest(TestFile, Serializer, Records.PocoWithExtraFieldsRecords);
        }

        [TestMethod]
        public void SerializeTest()
        {
            Helpers.Tests.SerializeTest("CsvFile.csv", Serializer, Records.PocoWithExtraFieldsRecords);
        }

        [TestMethod]
        public void SerializeArrayTest()
        {
            Helpers.Tests.SerializeArrayTest("CsvFile.csv", Serializer, Records.PocoWithExtraFieldsRecords);
        }
    }
}
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

        [TestMethod]
        public void DeserializeCallerCreationTest()
        {
            CsvWithExtraFieldsRecord myRecord = new CsvWithExtraFieldsRecord
            {
                ExtraField = "Created By Caller"
            };
            using (var reader = Helpers.Utilities.OpenEmbeddedFile(TestFile))
            {
                Serializer.Deserialize(reader.ReadLine(), myRecord);
            }
            var expectedResult = Records.CsvWithExtraFieldsRecords[0];
            var originalValue = expectedResult.ExtraField;
            expectedResult.ExtraField = "Created By Caller";
            Assert.AreEqual(0, new ReflectionComparer().Compare(expectedResult, myRecord));
            expectedResult.ExtraField = originalValue;  // Reset this value so that other tests aren't affected
        }
    }
}
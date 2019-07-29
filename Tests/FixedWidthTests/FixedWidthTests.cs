using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Tests.Helpers;
using TheCodingMonkey.Serialization.Configuration;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fixed Width")]
    public class FixedWidthTests
    {
        protected FixedWidthSerializer<FixedWidthRecord> Serializer = new FixedWidthSerializer<FixedWidthRecord>();
        protected string TestFile = "FixedWidthFile.txt";

        [TestMethod]
        public void DeserializeArrayTest()
        {
            Helpers.Tests.DeserializeArrayTest(TestFile, Serializer, Records.FixedWidthRecords);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            Helpers.Tests.DeserializeEnumerableTest(TestFile, Serializer, Records.FixedWidthRecords);
        }

        [TestMethod]
        public void SerializeTest()
        {
            Helpers.Tests.SerializeTest(TestFile, Serializer, Records.FixedWidthRecords);
        }

        [TestMethod]
        public void SerializeArrayTest()
        {
            Helpers.Tests.SerializeArrayTest(TestFile, Serializer, Records.FixedWidthRecords);
        }

        [TestMethod, ExpectedException(typeof(TextSerializationConfigurationException))]
        public void ZeroSizeTest()
        {
            var serializer = new FixedWidthSerializer<FixedWidthInvalidSizeRecord>();
        }
    }
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Tests.Helpers;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fixed Width")]
    public class FixedWidthWithOptionsTests
    {
        private FixedWidthSerializer<FixedWidthWithOptionsRecord> Serializer = new FixedWidthSerializer<FixedWidthWithOptionsRecord>();
        private string DeserializeTestFile = "FixedWidthWithOptionsFile.txt";
        private string SerializeTestFile = "FixedWidthFile.txt";

        [TestMethod]
        public void DeserializeArrayTest()
        {
            Helpers.Tests.DeserializeArrayTest(DeserializeTestFile, Serializer, Records.FixedWidthWithOptionsRecords);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            Helpers.Tests.DeserializeEnumerableTest(DeserializeTestFile, Serializer, Records.FixedWidthWithOptionsRecords);
        }

        [TestMethod]
        public void SerializeTest()
        {
            Helpers.Tests.SerializeTest(SerializeTestFile, Serializer, Records.FixedWidthWithOptionsRecords);
        }

        [TestMethod]
        public void SerializeArrayTest()
        {
            Helpers.Tests.SerializeArrayTest(SerializeTestFile, Serializer, Records.FixedWidthWithOptionsRecords);
        }
    }
}
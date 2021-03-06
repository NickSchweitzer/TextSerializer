﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Helpers;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class CsvStructConfigurationTests
    {
        protected CsvSerializer<PocoStructRecord> Serializer;
        protected string TestFile = "CsvFile.csv";

        public CsvStructConfigurationTests()
        {
            Serializer = new CsvSerializer<PocoStructRecord>(config => config.ByConvention()
                .ForMember(field => field.Enabled, opt => opt.Optional()));
        }

        [TestMethod]
        public void DeserializeArrayTest()
        {
            Helpers.Tests.DeserializeArrayTest(TestFile, Serializer, Records.PocoStructRecords);
        }

        [TestMethod]
        public void DeserializeEnumerableTest()
        {
            Helpers.Tests.DeserializeEnumerableTest(TestFile, Serializer, Records.PocoStructRecords);
        }

        [TestMethod]
        public void SerializeTest()
        {
            Helpers.Tests.SerializeTest(TestFile, Serializer, Records.PocoStructRecords);
        }

        [TestMethod]
        public void SerializeArrayTest()
        {
            Helpers.Tests.SerializeArrayTest(TestFile, Serializer, Records.PocoStructRecords);
        }
    }
}
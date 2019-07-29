using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("CSV")]
    public class CsvWithoutQualifierTests : CsvTests
    {
        public CsvWithoutQualifierTests() 
        {
            Serializer = new CsvSerializer<CsvRecord>()
            {
                AlwaysWriteQualifier = false
            };
            TestFile = "CsvWithoutQualifierFile.csv";
        }
    }
}
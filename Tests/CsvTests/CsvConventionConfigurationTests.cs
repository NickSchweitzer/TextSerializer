using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class CsvConventionConfigurationTests : CsvFluentConfigurationTests
    {
        public CsvConventionConfigurationTests()
        {
            Serializer = new CsvSerializer<PocoRecord>(config => config.ByConvention()
                .ForMember(field => field.Enabled, opt => opt.Optional()));
        }
    }
}
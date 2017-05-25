using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Helpers;
using TheCodingMonkey.Serialization.Tests.Models;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class FixedWidthFluentConfigurationTests : CsvFluentConfigurationTests
    {
        public FixedWidthFluentConfigurationTests()
        {
            DeserializeTestFile = "FixedWidthWithOptionsFile.txt";
            SerializeTestFile = "FixedWidthFile.txt";
            Serializer = new FixedWidthSerializer<PocoRecord>(config => config
                .ForMember(field => field.Id, opt => opt.Position(0).Size(5).Padding('0'))
                .ForMember(field => field.Name, opt => opt.Position(1).Size(15))
                .ForMember(field => field.Description, opt => opt.Position(2).Size(35))
                .ForMember(field => field.Value, opt => opt.Position(3).Size(8).Padding('0'))
                .ForMember(field => field.Enabled, opt => opt.Optional().Position(4).Size(5)));
        }
    }
}
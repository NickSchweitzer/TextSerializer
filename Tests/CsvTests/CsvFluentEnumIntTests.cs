using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Formatters;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class CsvFluentEnumIntTests : CsvFluentEnumStringTests
    {
        public CsvFluentEnumIntTests()
        {
            TestFile = "CsvWithEnumIntFile.csv";
            Serializer = new CsvSerializer<PocoWithEnumRecord>(config => config.ByConvention()
                .ForMember(field => field.Enabled, opt => opt.FormatterType(typeof(BooleanIntFormatter)))
                .ForMember(field => field.Options, opt => opt.FormatEnum(EnumOptions.Integer)));
        }
    }
}
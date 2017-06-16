using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheCodingMonkey.Serialization.Tests.Models;
using TheCodingMonkey.Serialization.Formatters;

namespace TheCodingMonkey.Serialization.Tests
{
    [TestClass, TestCategory("Fluent")]
    public class FixedWidthWithEnumIntTests : CsvFluentEnumStringTests
    {
        public FixedWidthWithEnumIntTests()
        {
            TestFile = "FixedWidthWithEnumIntFile.txt";
            Serializer = new FixedWidthSerializer<PocoWithEnumRecord>(config => config
                .ForMember(field => field.Id, opt => opt.Position(0).Size(5).Padding('0'))
                .ForMember(field => field.Name, opt => opt.Position(1).Size(15))
                .ForMember(field => field.Description, opt => opt.Position(2).Size(35))
                .ForMember(field => field.Value, opt => opt.Position(3).Size(8).Padding('0'))
                .ForMember(field => field.Enabled, opt => opt.Position(4).Size(1).FormatterType(typeof(BooleanIntFormatter)))
                .ForMember(field => field.Options, opt => opt.Position(5).Size(1).FormatEnum(EnumOptions.Integer)));
        }
    }
}
using System;
using System.Collections.Generic;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable, IniSection("General Settings")]
    public class IniSimpleListModel
    {
        [IniField, FormatEnum(Formatters.EnumOptions.String)]
        public List<MyEnum> MyList = new List<MyEnum>();

        public enum MyEnum
        {
            Value1,
            Value2,
            Value3,
            Value4
        }
    }
}
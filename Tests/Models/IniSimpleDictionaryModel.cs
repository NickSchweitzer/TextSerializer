using System;
using System.Collections.Generic;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable, IniSection("General Settings")]
    public class IniSimpleDictionaryModel
    {
        [IniField]
        public Dictionary<string, MyEnum> Dictionary { get; set; } = new Dictionary<string, MyEnum>();

        public enum MyEnum
        {
            Value1 = 1,
            Value2,
            Value3,
            Value4
        }
    }
}
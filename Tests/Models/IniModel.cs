using System;
using TheCodingMonkey.Serialization.Formatters;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable]
    public struct IniModel  // Test to make sure Structs work
    {
        [IniField]
        public int IntValue { get; set; }
        
        [IniField(Name = "Double Value")]
        public double DoubleValue { get; set; }

        [IniField]
        public string StringValue { get; set; }

        [IniField]
        public bool BoolValue;  // Test to make sure Fields work too

        [IniField, FormatEnum(EnumOptions.Integer)]
        public MyEnum EnumIntValue { get; set; }

        [IniField]
        public MyEnum EnumStringValue { get; set; }

        public enum MyEnum
        {
            First = 1,
            Second,
            Third
        }
    }
}
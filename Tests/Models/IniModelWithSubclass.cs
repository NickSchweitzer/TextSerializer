using System;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable, IniSection("General Settings")]
    public class IniModelWithSubclass
    {
        [IniField]
        public int IntValue { get; set; }

        [IniField(Name = "Double Value")]
        public double DoubleValue { get; set; }

        [IniField]
        public string StringValue { get; set; }

        [IniField]
        public bool BoolValue { get; set; }
        
        [IniField(Name = "Class as Property")]
        public IniModelSubclass Subclass { get; set; }
        
        [TextSerializable]
        public class IniModelSubclass
        {
            [IniField]
            public int MyValue { get; set; }
            [IniField]
            public bool BooleanValue { get; set; }
            [IniField]
            public string TestString { get; set; }
        }
    }
}
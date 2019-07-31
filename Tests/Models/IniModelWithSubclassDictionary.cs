using System;
using System.Collections.Generic;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable, IniSection("General Settings")]
    public class IniModelWithSubclassDictionary
    {
        [IniField]
        public int IntValue { get; set; }

        [IniField(Name = "Double Value")]
        public double DoubleValue { get; set; }

        [IniField]
        public string StringValue { get; set; }

        [IniField]
        public bool BoolValue { get; set; }

        [IniField]
        public Dictionary<string, IniModelSubclass> Dictionary { get; set; } = new Dictionary<string, IniModelSubclass>();
        
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
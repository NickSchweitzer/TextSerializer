using System;
using System.Collections.Generic;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable, IniSection("General Settings")]
    public class IniModelWithSubclassList
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
        public List<IniModelSubclass> List { get; set; } = new List<IniModelSubclass>();
        
        [TextSerializable]
        public class IniModelSubclass
        {
            [IniField, IniSection]
            public string MySection { get; set; }

            [IniField]
            public int MyValue { get; set; }

            [IniField]
            public bool BooleanValue { get; set; }

            [IniField]
            public string TestString { get; set; }
        }
    }
}
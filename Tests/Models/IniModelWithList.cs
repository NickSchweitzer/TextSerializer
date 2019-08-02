using System;
using System.Collections.Generic;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable, IniSection("General Settings")]
    public class IniModelWithList
    {
        [IniField]
        public int IntValue { get; set; }

        [IniField(Name = "Double Value")]
        public double DoubleValue { get; set; }

        [IniField]
        public string StringValue { get; set; }

        [IniField]
        public bool BoolValue { get; set; }

        [IniField(Name = "List of Things")]
        public IList<string> StringList { get; set; } = new List<string>();
    }
}
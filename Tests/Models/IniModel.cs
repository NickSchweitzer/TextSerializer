using System;
using System.Collections.Generic;
using System.Text;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable]
    public class IniModel
    {
        [IniField]
        public int IntValue { get; set; }
        
        [IniField(Name = "Double Value")]
        public double DoubleValue { get; set; }

        [IniField]
        public string StringValue { get; set; }

        [IniField]
        public bool BoolValue { get; set; }
    }
}
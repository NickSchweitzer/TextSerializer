﻿using System;
using System.Collections.Generic;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable, IniSection("General Settings")]
    public class IniDictionarySectionModel
    {
        [IniField]
        public int IntValue { get; set; }

        [IniField(Name = "Double Value")]
        public double DoubleValue { get; set; }

        [IniField]
        public string StringValue { get; set; }

        [IniField]
        public bool BoolValue { get; set; }

        [IniField(Name = "My Dictionary")]
        public IDictionary<string, int> Dictionary { get; set; } = new Dictionary<string, int>();
    }
}
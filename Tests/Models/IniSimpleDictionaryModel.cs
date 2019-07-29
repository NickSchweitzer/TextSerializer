using System;
using System.Collections.Generic;
using System.Text;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable, IniSection("General Settings")]
    public class IniSimpleDictionaryModel
    {
        [IniField]
        public Dictionary<string, string> Dictionary { get; set; } = new Dictionary<string, string>();
    }
}
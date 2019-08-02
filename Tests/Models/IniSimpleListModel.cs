using System;
using System.Collections.Generic;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable, IniSection("General Settings")]
    public class IniSimpleListModel
    {
        [IniField]
        public List<string> MyList = new List<string>();
    }
}
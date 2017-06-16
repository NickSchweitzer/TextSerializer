using System;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable]
    public struct CsvStructRecord
    {
        [TextField(0)]
        public int Id;
        [TextField(1)]
        public string Name;
        [TextField(2)]
        public string Description { get; set; } // Can have properties on Structs too
        [TextField(3)]
        public double Value;
        [TextField(4)]
        public bool Enabled;
    }
}
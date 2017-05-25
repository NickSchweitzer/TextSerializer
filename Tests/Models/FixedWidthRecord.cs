using System;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable]
    public class FixedWidthRecord
    {
        [FixedWidthField(0, 5, Padding = '0')]
        public int Id { get; set; }
        [FixedWidthField(1, 15)]
        public string Name { get; set; }
        [FixedWidthField(2, 35)]
        public string Description;
        [FixedWidthField(3, 8, Padding = '0')]
        public double Value { get; set; }
        [FixedWidthField(4, 5)]
        public bool Enabled { get; set; }
    }
}
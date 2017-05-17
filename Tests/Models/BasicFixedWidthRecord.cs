using System;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable]
    public class BasicFixedWidthRecord
    {
        [FixedWidthField(0, 5, Padding = '0')]
        public int Id { get; set; }
        [FixedWidthField(5, 15)]
        public string Name { get; set; }
        [FixedWidthField(20, 30)]
        public string Description { get; set; }
        [FixedWidthField(50, 8, Padding = '0')]
        public double Value { get; set; }
        [FixedWidthField(58, 5)]
        public bool Enabled { get; set; }
    }
}
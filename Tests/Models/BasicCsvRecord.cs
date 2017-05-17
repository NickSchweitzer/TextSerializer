using System;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable]
    public class BasicCsvRecord
    {
        [TextField(0)]
        public int Id { get; set; }
        [TextField(1)]
        public string Name { get; set; }
        [TextField(2)]
        public string Description { get; set; }
        [TextField(3)]
        public double Value { get; set; }
        [TextField(4)]
        public bool Enabled { get; set; }
    }
}
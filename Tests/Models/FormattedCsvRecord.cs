using System;
using TheCodingMonkey.Serialization.Formatters;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable]
    public class FormattedCsvRecord
    {
        [TextField(0)]
        public int Id { get; set; }
        [TextField(1)]
        public string Name { get; set; }
        [TextField(2)]
        public string Description { get; set; }
        [TextField(3)]
        public double Value { get; set; }
        [TextField(4, FormatterType = typeof(BooleanIntFormatter))]
        public bool Enabled { get; set; }
    }
}
using System;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    public class CsvPocoMixedRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description;      // Specifically test Fields working instead of properties in different scenarios
        public double Value { get; set; }
        public bool Enabled { get; set; }
    }
}
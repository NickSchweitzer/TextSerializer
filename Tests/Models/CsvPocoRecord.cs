using System;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    public class CsvPocoRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public bool Enabled { get; set; }
    }
}
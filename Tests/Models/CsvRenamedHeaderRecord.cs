using System;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable]
    public class CsvRenamedHeaderRecord
    {
        private int id;
        [TextField(0, Name = "UniqueId")]
        public int Id { get => id; set => id = value; } // Specifically test expression bodies
        [TextField(1)]
        public string Name { get; set; }
        [TextField(2)]
        public string Description;  // Specifically test fields instead of properties
        [TextField(3)]
        public double Value { get; set; }
        [TextField(4)]
        public bool Enabled { get; set; }
    }
}
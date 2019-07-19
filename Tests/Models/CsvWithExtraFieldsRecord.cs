using System;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    [TextSerializable]
    public class CsvWithExtraFieldsRecord
    {
        public CsvWithExtraFieldsRecord()
        {
            ExtraField = "Should Not Get Overwritten";
        }

        [TextField(0)]
        public int Id { get; set; }
        [TextField(1)]
        public string Name { get; set; }
        [TextField(2)]
        public string Description;
        [TextField(3)]
        public double Value { get; set; }
        [TextField(4)]
        public bool Enabled { get; set; }
        public string ExtraField { get; set; }  // Should not be serialized
    }
}
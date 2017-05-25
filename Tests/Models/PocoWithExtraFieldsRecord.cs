using System;

namespace TheCodingMonkey.Serialization.Tests.Models
{
    public class PocoWithExtraFieldsRecord
    {
        public PocoWithExtraFieldsRecord()
        {
            ExtraField = "Should Not Get Overwritten";
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public bool Enabled { get; set; }
        public string ExtraField { get; set; }
    }
}
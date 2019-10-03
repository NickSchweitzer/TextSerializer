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

        public void ExtraMethod()
        {
            ExtraField = "Do Not Call";
        }

        // Disable the "Event Never Used Warning" - Need this here to validate that its not picked up by Reflection later
#pragma warning disable 67
        public event ExtraFieldChangedHandler ExtraEvent;
#pragma warning restore 67
    }

    public delegate void ExtraFieldChangedHandler(object sender, EventArgs e);
}
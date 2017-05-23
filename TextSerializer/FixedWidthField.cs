using System;

namespace TheCodingMonkey.Serialization
{
    public class FixedWidthField : Field
    {
        /// <summary>Position (column) where this field is serialized in the CSV file.</summary>
        public char Padding { get; set; }
    }
}
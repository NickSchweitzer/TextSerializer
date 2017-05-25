using System;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Contains configuration properties for a field/property that are specific to Fixed Width files</summary>
    public class FixedWidthField : Field
    {
        /// <summary>Default Constructor</summary>
        public FixedWidthField()
        {
            Padding = ' ';
        }

        /// <summary>Character to use to pad a text field if it doesn't meet the minimum size requirement for the
        /// fixed length field.</summary>
        public char Padding { get; set; }
    }
}
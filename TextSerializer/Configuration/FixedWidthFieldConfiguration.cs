using System;

namespace TheCodingMonkey.Serialization.Configuration
{
    /// <summary>Fluent Configuration class used to configure fields and properties which are serialized using the 
    /// <see cref="FixedWidthSerializer{TTargetType}">FixedWidthSerialize</see> class.</summary>
    public class FixedWidthFieldConfiguration
    {
        internal FixedWidthField Field { get; set; }

        internal FixedWidthFieldConfiguration(FixedWidthField field)
        {
            Field = field;
        }

        /// <summary>Position (column) where this field is serialized in the CSV file.</summary>
        public FixedWidthFieldConfiguration Position(int pos)
        {
            Field.Position = pos;
            return this;
        }

        /// <summary>Maximum length in the CSV file that this field should take up.</summary>
        public FixedWidthFieldConfiguration Size(int size)
        {
            Field.Size = size;
            return this;
        }

        /// <summary>Determines whether this field is optional. Because of the nature of CSV and FixedWidth file formats,
        /// optional fields should only be a the end of the record.</summary>
        public FixedWidthFieldConfiguration Optional(bool optional = true)
        {
            Field.Optional = optional;
            return this;
        }

        /// <summary>Optional class which is used to control custom serialization/deserialization of this field.
        /// This class must implement the <see cref="ITextFormatter">ITextFormatter</see> interface.</summary>
        public FixedWidthFieldConfiguration FormatterType(Type formatter)
        {
            Field.FormatterType = formatter;
            return this;
        }

        /// <summary>Position (column) where this field is serialized in the CSV file.</summary>
        public FixedWidthFieldConfiguration Padding(char pad)
        {
            Field.Padding = pad;
            return this;
        }
    }
}
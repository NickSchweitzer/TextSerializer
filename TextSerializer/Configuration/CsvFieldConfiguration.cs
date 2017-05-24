using System;

namespace TheCodingMonkey.Serialization.Configuration
{
    /// <summary>Fluent Configuration class used to configure fields and properties which are serialized using the <see cref="CsvSerializer{TTargetType}">CsvSerialize</see> class.</summary>
    public class CsvFieldConfiguration
    {
        internal CsvField Field { get; set; }

        internal CsvFieldConfiguration(CsvField field)
        {
            Field = field;
        }

        /// <summary>Position (column) where this field is serialized in the CSV file.</summary>
        public CsvFieldConfiguration Position(int pos)
        {
            Field.Position = pos;
            return this;
        }

        /// <summary>Maximum length in the CSV file that this field should take up.</summary>
        public CsvFieldConfiguration Size(int size)
        {
            Field.Size = size;
            return this;
        }

        /// <summary>Name of this field.  If not specified, then the property/field name of the class/struct
        /// is used.  If a header is written to the CSV file, then this is the value that is used.</summary>
        public CsvFieldConfiguration Name(string name)
        {
            Field.Name = name;
            return this;
        }

        /// <summary>Determines whether this field is optional. Because of the nature of CSV and FixedWidth file formats,
        /// optional fields should only be a the end of the record.</summary>
        public CsvFieldConfiguration Optional(bool optional = true)
        {
            Field.Optional = optional;
            return this;
        }

        /// <summary>Optional class which is used to control custom serialization/deserialization of this field.
        /// This class must implement the <see cref="ITextFormatter">ITextFormatter</see> interface.</summary>
        public CsvFieldConfiguration FormatterType(Type formatter)
        {
            Field.FormatterType = formatter;
            return this;
        }
    }
}
using System;

namespace TheCodingMonkey.Serialization.Configuration
{
    public class TextFieldConfiguration
    {
        internal TextFieldAttribute Attribute { get; set; }

        internal TextFieldConfiguration(TextFieldAttribute attr)
        {
            Attribute = attr;
            Position(-1);
            Size(-1);
            Optional(false);
        }

        /// <summary>Position (column) where this field is serialized in the CSV file.</summary>
        public TextFieldConfiguration Position(int pos)
        {
            Attribute.Position = pos;
            return this;
        }

        /// <summary>Maximum length in the CSV file that this field should take up.</summary>
        public TextFieldConfiguration Size(int size)
        {
            Attribute.Size = size;
            return this;
        }

        /// <summary>Name of this field.  If not specified, then the property/field name of the class/struct
        /// is used.  If a header is written to the CSV file, then this is the value that is used.</summary>
        public TextFieldConfiguration Name(string name)
        {
            Attribute.Name = name;
            return this;
        }

        /// <summary>Determines whether this field is optional. Because of the nature of CSV and FixedWidth file formats,
        /// optional fields should only be a the end of the record.</summary>
        public TextFieldConfiguration Optional(bool optional = true)
        {
            Attribute.Optional = optional;
            return this;
        }

        /// <summary>Optional class which is used to control custom serialization/deserialization of this field.
        /// This class must implement the <see cref="ITextFormatter">ITextFormatter</see> interface.</summary>
        public TextFieldConfiguration FormatterType(Type formatter)
        {
            Attribute.FormatterType = formatter;
            return this;
        }
    }
}
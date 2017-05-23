using System;

namespace TheCodingMonkey.Serialization
{
    /// <summary>This attribute is applied to Fields or Properties of a class to control where in the CSV file
    /// this field belongs.</summary>
    [AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false )]
    public class TextFieldAttribute : Attribute
    {
        internal Field Field { get; set; }

        /// <summary>Default constructor.</summary>
        public TextFieldAttribute()
        {
            Field = new CsvField();
        }

        /// <summary>Default constructor.</summary>
        /// <param name="position">Position (column) where this field is serialized in the CSV file.</param>
        public TextFieldAttribute( int position )
        : this()
        {
            Position = position;
        }

        /// <summary>Position (column) where this field is serialized in the CSV file.</summary>
        public int Position
        {
            get { return Field.Position; }
            set { Field.Position = value; }
        }

        /// <summary>Maximum length in the CSV file that this field should take up.</summary>
        public int Size
        {
            get { return Field.Size; }
            set { Field.Size = value; }
        }

        /// <summary>Name of this field.  If not specified, then the property/field name of the class/struct
        /// is used.  If a header is written to the CSV file, then this is the value that is used.</summary>
        public string Name
        {
            get { return ((CsvField)Field).Name; }
            set { ((CsvField)Field).Name = value; }
        }
        
        /// <summary>Determines whether this field is optional. Because of the nature of CSV and FixedWidth file formats,
        /// optional fields should only be a the end of the record.</summary>
        public bool Optional
        {
            get { return Field.Optional; }
            set { Field.Optional = value; }
        }

        /// <summary>Optional class which is used to control custom serialization/deserialization of this field.
        /// This class must implement the <see cref="ITextFormatter">ITextFormatter</see> interface.</summary>
        public Type FormatterType
        {
            get { return Field.FormatterType; }
            set { Field.FormatterType = value; }
        }
    }
}
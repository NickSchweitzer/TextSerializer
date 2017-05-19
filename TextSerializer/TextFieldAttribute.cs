using System;
using System.Reflection;

namespace TheCodingMonkey.Serialization
{
    /// <summary>This attribute is applied to Fields or Properties of a class to control where in the CSV file
    /// this field belongs.</summary>
    [AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false )]
    public class TextFieldAttribute : Attribute
    {
        private Type _formatterType;

        internal TextFieldAttribute() { }

        /// <summary>Default constructor.</summary>
        /// <param name="position">Position (column) where this field is serialized in the CSV file.</param>
        public TextFieldAttribute( int position )
        {
            Position = position;
            Size = -1;
            Optional = false;
        }

        /// <summary>Position (column) where this field is serialized in the CSV file.</summary>
        public int Position { get; set; }

        /// <summary>Maximum length in the CSV file that this field should take up.</summary>
        public int Size { get; set; }

        /// <summary>Name of this field.  If not specified, then the property/field name of the class/struct
        /// is used.  If a header is written to the CSV file, then this is the value that is used.</summary>
        public string Name { get; set; }

        /// <summary>Determines whether this field is optional. Because of the nature of CSV and FixedWidth file formats,
        /// optional fields should only be a the end of the record.</summary>
        public bool Optional { get; set; }

        /// <summary>Optional class which is used to control custom serialization/deserialization of this field.
        /// This class must implement the <see cref="ITextFormatter">ITextFormatter</see> interface.</summary>
        public Type FormatterType
        {
            get { return _formatterType; }
            set
            {
                _formatterType = value;
                Formatter = (ITextFormatter)_formatterType.Assembly.CreateInstance( _formatterType.FullName );
            }
        }

        internal MemberInfo Member { get; set; }
        internal ITextFormatter Formatter { get; private set; }
        internal object[] AllowedValues { get; set; }

        internal Type GetNativeType()
        {
            if ( Member is PropertyInfo )
                return ( (PropertyInfo)Member ).PropertyType;
            else if ( Member is FieldInfo )
                return ( (FieldInfo)Member ).FieldType;
            else
                throw new TextSerializationException( "Invalid MemberInfo type encountered" );
        }
    }
}
using System;

namespace TheCodingMonkey.Serialization
{
    /// <summary>This attribute is applied to Fields or Properties of a class to control how an INI field is handled.</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class IniFieldAttribute : Attribute
    {
        internal IniField Field { get; set; }

        /// <summary>Default constructor.</summary>
        public IniFieldAttribute()
        {
            Field = new IniField();
        }

        /// <summary>Name of this field.  If not specified, then the property/field name of the class/struct
        /// is used.  If a header is written to the CSV file, then this is the value that is used.</summary>
        public string Name
        {
            get { return ((IniField)Field).Name; }
            set { ((IniField)Field).Name = value; }
        }

        /// <summary>Maximum length in the CSV file that this field should take up.</summary>
        public int Size
        {
            get { return Field.Size; }
            set { Field.Size = value; }
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
using System;
using System.Reflection;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Class which contains the configuration details of a field/property that is created either using Attributes or Fluent Configuration</summary>
    public abstract class Field
    {
        /// <summary>Default Constructor</summary>
        protected Field()
        {
            Position = -1;
            Size = -1;
            Optional = false;
        }

        private Type _formatterType;
        private ITextFormatter _formatter;

        /// <summary>Position (column) where this field is serialized in the CSV file.</summary>
        public int Position { get; set; }

        /// <summary>Maximum length in the CSV file that this field should take up.</summary>
        public int Size { get; set; }
        
        /// <summary>Determines whether this field is optional. Because of the nature of CSV and FixedWidth file formats,
        /// optional fields should only be a the end of the record.</summary>
        public bool Optional { get; set; }

        /// <summary>Optional class which is used to control custom serialization/deserialization of this field.
        /// This class must implement the <see cref="ITextFormatter">ITextFormatter</see> interface.</summary>
        public Type FormatterType
        {
            get => _formatterType;
            set
            {
                _formatterType = value;
                Formatter = (ITextFormatter)_formatterType.Assembly.CreateInstance(_formatterType.FullName);
            }
        }

        /// <summary>The reflected MemberInfo details of the field/property that this configures.</summary>
        public MemberInfo Member { get; set; }

        /// <summary>The Formatter to be used for Serialization/Deserialization if the default formatting is not used.</summary>
        public ITextFormatter Formatter {
            get => _formatter;
            set
            {
                _formatter = value;
                _formatterType = value.GetType();
            }
        }

        /// <summary>Defines the allowed characters that can be used for a field in the file.</summary>
        public object[] AllowedValues { get; set; }

        /// <summary>Returns the native underlying type of the Property or Field defined for this class.</summary>
        public Type GetNativeType()
        {
            if (Member is PropertyInfo)
                return ((PropertyInfo)Member).PropertyType;
            else if (Member is FieldInfo)
                return ((FieldInfo)Member).FieldType;
            else
                throw new TextSerializationException("Invalid MemberInfo type encountered");
        }
    }
}
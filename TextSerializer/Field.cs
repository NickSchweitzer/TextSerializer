using System;
using System.Reflection;

namespace TheCodingMonkey.Serialization
{
    public abstract class Field
    {
        public Field()
        {
            Position = -1;
            Size = -1;
            Optional = false;
        }

        private Type _formatterType;

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
            get { return _formatterType; }
            set
            {
                _formatterType = value;
                Formatter = (ITextFormatter)_formatterType.Assembly.CreateInstance(_formatterType.FullName);
            }
        }

        public MemberInfo Member { get; set; }
        public ITextFormatter Formatter { get; private set; }
        public object[] AllowedValues { get; set; }

        internal Type GetNativeType()
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
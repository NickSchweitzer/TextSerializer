using System;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Contains configuration properties for a field/property that are specific to INI files</summary>
    public class IniField : Field
    {
        private string _Name = null;

        /// <summary>Name of this field.  If not specified, then the property/field name of the class/struct is used.</summary>
        public string Name
        {
            get { return _Name ?? this.Member.Name; }
            set { _Name = value; }
        }

        /// <summary>True if this property accepts list items for this INI section, i.e. lines that
        /// are not key value pairs</summary>
        public bool IsList { get; set; } = false;

        /// <summary>True if this property accepts key value pairs for this INI section. This allows someone to deserialize an INI
        /// file as a Property Bag, instead of having named properties for every single Key entry.</summary>
        public bool IsDictionary { get; set; } = false;

        internal IniSection Section { get; set; } = null;
    }
}
using System;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Contains configuration properties for a field/property that are specific to CSV files</summary>
    public class CsvField : Field
    {
        private string _Name = null;

        /// <summary>Name of this field.  If not specified, then the property/field name of the class/struct
        /// is used.  If a header is written to the CSV file, then this is the value that is used.</summary>
        public string Name
        {
            get { return _Name ?? this.Member.Name; }
            set { _Name = value; }
        }
    }
}
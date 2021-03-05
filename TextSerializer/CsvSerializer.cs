using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using TheCodingMonkey.Serialization.Configuration;
using System.Reflection;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Used to serialize a TargetType object to a CSV file.</summary>
    /// <typeparam name="TTargetType">The type of object that will be serialized.  TargetType either must have the 
    /// <see cref="TextSerializableAttribute">TextSerializable attribute</see> applied, and any fields contained must have the 
    /// <see cref="TextFieldAttribute">TextField attribute</see> applied to them, or <see cref="CsvConfiguration{TTargetType}">Fluent Configuration</see>
    /// must be used.</typeparam>
    public class CsvSerializer<TTargetType> : TextSerializer<TTargetType>
        where TTargetType : new()
    {
        /// <summary>Initializes a new instance of the CSVSerializer class with default values, using Attributes on the target type
        /// to determine the configuration of fields and properties.</summary>
        public CsvSerializer()
        : this(true, ',', '"')
        { }

        /// <summary>Initializes a new instance of the CSVSerializer class with specific values for how the CSV should be formatted, using Attributes 
        /// on the target type to determine the configuration of fields and properties.</summary>
        /// <param name="alwaysWriteQualifier">True to always use qualifiers around all fields in the CSV.</param>
        /// <param name="delimiter">The delimiter to use between CSV fields.</param>
        /// <param name="qualifier">The qualifier character to use around the fields.</param>
        public CsvSerializer(bool alwaysWriteQualifier, char delimiter, char qualifier)
        {
            AlwaysWriteQualifier = alwaysWriteQualifier;
            Delimiter = delimiter;
            Qualifier = qualifier;
            InitializeFromAttributes();
        }

        /// <summary>Initializes a new instance of the CSVSerializer class using Fluent configuration.</summary>
        ///<param name="config">Fluent configuration for the serializer.</param>
        public CsvSerializer(Action<CsvConfiguration<TTargetType>> config)
        {
            CsvConfiguration<TTargetType> completedConfig = new CsvConfiguration<TTargetType>(this);
            config.Invoke(completedConfig);
            Fields.Sort((x, y) => x.Position.CompareTo(y.Position));

            if (!FieldGapsAllowed)
            {
                for (int i = 0; i < Fields.Count; i++)
                {
                    if (Fields[i].Position != i)
                        throw new TextSerializationConfigurationException($"Missing field definition for Position {i}");
                }
            }
        }

        /// <summary>Used by a derived class to return a Field configuration specific to this serializer back for a given method based on the attributes applied.</summary>
        /// <param name="member">Property or Field to return a configuration for.</param>
        /// <returns>Field configuration if this property should be serialized, otherwise null to ignore.</returns>
        protected override Field GetFieldFromAttribute(MemberInfo member)
        {
            // Check to see if they've marked up this field/property with the attribute for serialization
            object[] fieldAttrs = member.GetCustomAttributes(typeof(TextFieldAttribute), false);
            if (fieldAttrs.Length > 0)
                return ((TextFieldAttribute)fieldAttrs[0]).Field;
            else
                return null;
        }

        /// <summary>True if should wrap every field in the <see cref="Qualifier">Qualifier</see> during serialization.  If false, then
        /// the qualifier is only written if the field contains the <see cref="Delimiter">Delimiter</see>.</summary>
        public bool AlwaysWriteQualifier { get; set; }

        /// <summary>Character which is used to delimit fields in the record.</summary>
        public char Delimiter { get; set; }

        /// <summary>Character used to wrap a field if the field contains the <see cref="Delimiter">Delimiter</see>.</summary>
        public char Qualifier { get; set; }

        /// <summary>Serializes out the header row by itself</summary>
        /// <returns>Header string</returns>
        public string SerializeHeader()
        {
            var headerList = new List<string>();
            for (int i = 0; i < Fields.Count; i++)
                headerList.Add(((CsvField)Fields[i]).Name);

            return FormatOutput(headerList);
        }

        /// <summary>Serializes an array of objects to CSV Format</summary>
        /// <param name="writer">TextWriter where CSV text should go</param>
        /// <param name="records">Array of objects to serialize</param>
        /// <param name="writeHeader">True if should write a header record first, false otherwise</param>
        public void SerializeArray( TextWriter writer, ICollection<TTargetType> records, bool writeHeader )
        {
            if (writeHeader)
                writer.WriteLine(SerializeHeader());

            base.SerializeArray( writer, records );
        }

        /// <summary>Creates a collection of TargetType objects from a stream of text containing CSV.</summary>
        /// <param name="reader">TextReader to read in from.</param>
        /// <param name="readHeader">True if a header row is expected, false otherwise.</param>
        /// <returns>An array of objects containing the records in the file.</returns>
        public ICollection<TTargetType> DeserializeArray( TextReader reader, bool readHeader )
        {
            return DeserializeArray( reader, 0, readHeader );
        }

        /// <summary>Creates a collection of TargetType objects from a stream of text containing CSV.</summary>
        /// <param name="reader">TextReader to read in from.</param>
        /// <param name="count">Number of records to read.</param>
        /// <param name="readHeader">True if a header row is expected, false otherwise.</param>
        /// <returns>An array of objects containing the records in the file.</returns>
        /// <exception cref="TextSerializationException">Thrown if the header record does not match the 
        /// names of the parameters in the TargetType class.</exception>
        public ICollection<TTargetType> DeserializeArray( TextReader reader, int count, bool readHeader )
        {
            if (readHeader)
                ReadHeader(reader);

            return base.DeserializeArray( reader, count );
        }

        /// <summary>Deserializes a CSV file one record at a time suitable for usage in a foreach loop.</summary>
        /// <param name="reader">Reader which contains the CSV or FixedWidth data to deserialize.</param>
        /// <param name="readHeader">True if there is a Header row. This row will be ignored and not 
        /// returned as part of the IEnumerable.</param>
        /// <returns>An enumerable collection of TargetType objects.</returns>
        public IEnumerable<TTargetType> DeserializeEnumerable(TextReader reader, bool readHeader)
        {
            if (readHeader)
                ReadHeader(reader);

            bool bContinue = true;
            while (bContinue)
            {
                string row = reader.ReadLine();
                if (!string.IsNullOrEmpty(row))
                    yield return Deserialize(row);
                else
                    bContinue = false;
            }
        }

        private void ReadHeader(TextReader reader)
        {
            string strHeader = reader.ReadLine();
            if (!string.IsNullOrEmpty(strHeader))
            {
                List<string> headerList = Parse(strHeader);
                for (int i = 0; i < headerList.Count; i++)
                {
                    if (((CsvField)Fields[i]).Name != headerList[i])
                        throw new TextSerializationException("Header row does not match structure");
                }
            }
        }

        /// <summary>Parses a line of text as a record and returns the fields.</summary>
        /// <param name="text">A single line of text for the entire record out of the file.</param>
        /// <returns>A list of strings, where each string represents a field in the record.</returns>
        protected override List<string> Parse( string text )
        {
            List<string> returnList = new List<string>();       // Return value
            bool countDelimiter = true;                         // If we hit the delimiter character, should it be treated as a delimiter?
            StringBuilder currentField = new StringBuilder();   // Current field we're parsing through

            foreach ( char ch in text )
            {
                if ( ch == Qualifier )
                {
                    // We found a Qualifier character (usually a quote), so should treat a delimiter character as part of the field
                    countDelimiter = !countDelimiter;
                }
                else if ( ch == Delimiter )
                {
                    if ( countDelimiter )
                    {
                        // Found a delimiter, so end the field we're building up, add to our return list and clear the current field
                        returnList.Add( currentField.ToString() );
                        currentField = new StringBuilder();
                    }
                    else
                    {
                        // Inside of a qualified field, so just add this to the field string as usual
                        currentField.Append( ch );
                    }
                }
                else
                {
                    // Add this to the field string... just a normal character
                    currentField.Append( ch );
                }
            }

            // End of record, so add whatever we have to the list
            if ( countDelimiter )
                returnList.Add( currentField.ToString() );

            return returnList;
        }

        /// <summary>Write out a list of fields in CSV format.</summary>
        /// <param name="fieldList">List of strings where each string represents one field in the record.</param>
        /// <returns>A single CSV record.</returns>
        protected override string FormatOutput( List<string> fieldList )
        {
            StringBuilder sb = new StringBuilder();
            foreach ( string field in fieldList )
            {
                string qual = Qualifier.ToString();

                // Don't use the qualifier if AlwaysWriteQualifier is false, or if the field doesn't contain the delimiter
                if ( !AlwaysWriteQualifier && !field.Contains( Delimiter.ToString() ) )
                    qual = "";

                // Add the field to the record string
                sb.AppendFormat( "{0}{1}{0}{2}", qual, field, Delimiter );
            }

            // Remove the last delimiter from the end of the record
            if ( sb.Length > 0 )
                sb.Remove( sb.Length - 1, 1 );

            return sb.ToString();
        }
    }
}
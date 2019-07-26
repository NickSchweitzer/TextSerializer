using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Base class that contains common code for the <see cref="CsvSerializer{TTargetType}">CsvSerializer</see> and 
    /// <see cref="FixedWidthSerializer{TTargetType}">FixedWidthSerializer</see> class.</summary>
    /// <typeparam name="TTargetType">The type of object that will be serialized. Either TargetType must have the 
    /// <see cref="TextSerializableAttribute">TextSerializable attribute</see> applied, and any fields contained must have the 
    /// <see cref="TextFieldAttribute">TextField attribute</see> applied to them, or the Fluent Configuration model must be used.</typeparam>
    public abstract class RecordSerializer<TTargetType> : BaseSerializer<TTargetType>
        where TTargetType : new()
    {
        /// <summary>Serializes a single TargetType object into a properly formatted record string.</summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>Record string</returns>
        /// <exception cref="TextSerializationException">Thrown if an invalid class member is encountered.</exception>
        public string Serialize( TTargetType obj )
        {
            // First go through the object and get string representations of all the fields
            List<string> fieldList = new List<string>();
            for ( int i = 0; i < Fields.Count; i++ )
            {
                Field field = Fields[i];
                object objValue;

                // Get the object from the field
                if ( field.Member is PropertyInfo )
                    objValue = ( (PropertyInfo)field.Member ).GetValue( obj, null );
                else if ( field.Member is FieldInfo )
                    objValue = ( (FieldInfo)field.Member ).GetValue( obj );
                else
                    throw new TextSerializationException( "Invalid MemberInfo type encountered" );

                // Get the string representation for the object.  If there is a custom formatter for this field, then
                // use that, otherwise use the default ToString behavior.
                string str = field.Formatter != null ? field.Formatter.Serialize( objValue ) : objValue.ToString();

                // Truncate the string if required
                fieldList.Add( Truncate( str, field.Size ) );
            }

            // Now that all the strings are collected, put them together into a record string depending on the
            // type of serializer that this is.
            return FormatOutput( fieldList );
        }

        /// <summary>Creates a single TargetType object given a record string.</summary>
        /// <param name="text">Record string to deserialize.</param>
        /// <param name="returnMaybe">Possible return object. Allows creation in calling program.</param>
        /// <returns>Newly created TargetType object.</returns>
        /// <exception cref="ArgumentNullException">Thrown if text is null.</exception>
        /// <exception cref="TextSerializationException">Thrown if the number of fields in the record doesn't match that in TargetType.</exception>
        public TTargetType Deserialize( string text, TTargetType returnMaybe = default(TTargetType))
        {
            if ( text == null )
                throw new ArgumentNullException( "text", "text cannot be null" );

            // Create the correct return objects depending on whether this is a value or reference type
            // This makes a difference for reflection later on when populating the fields dynamically.
            TTargetType returnObj = returnMaybe == null || returnMaybe.Equals(default(TTargetType)) ? new TTargetType() : returnMaybe;
            ValueType returnStruct = null;
            if ( TargetType.IsValueType )
            {
                object tempObj = returnObj;
                returnStruct = (ValueType)tempObj;
            }

            // Parse the record into it's individual fields depending on the type of serializer this is
            List<string> parseList = Parse( text );
            int requiredCount = Fields.Count(attr => !attr.Optional);
            if ( parseList.Count < requiredCount || parseList.Count > Fields.Count )
                throw new TextSerializationException( "TargetType field count doesn't match number of items in text" );

            // For each field that is parsed in the string, populate the correct corresponding field in the TargetType
            for ( int i = 0; i < parseList.Count; i++ )
            {
                Field field = Fields[i];
                if ( field != null )
                {
                    string strVal = Truncate( parseList[i], field.Size );

                    // If there is a custom formatter, then use that to deserialize the string, otherwise use the default .NET behvavior.
                    object fieldObj = field.Formatter != null ? field.Formatter.Deserialize( strVal ) : Convert.ChangeType( strVal, field.GetNativeType() );

                    // Depending on whether the TargetType is a class or struct, you have to populate the fields differently
                    if ( TargetType.IsValueType )
                        AssignToStruct( returnStruct, fieldObj, field.Member );
                    else
                        AssignToClass( returnObj, fieldObj, field.Member );
                }
            }

            // If this was a value type, need to do a little more magic so can be returned properly
            if ( TargetType.IsValueType )
            {
                object tempObj = (object)returnStruct;
                returnObj = (TTargetType)tempObj;
            }

            return returnObj;
        }

        /// <summary>Serializes a collection of TargetType objects to the given TextWriter.</summary>
        /// <param name="writer">Writer to output serialized data to.</param>
        /// <param name="records">Collection of TargetType objects to serialize to.</param>
        public void SerializeArray( TextWriter writer, ICollection<TTargetType> records )
        {
            foreach ( TTargetType record in records )
                writer.WriteLine( Serialize( record ) );
        }

        /// <summary>Creates a collection of TargetType objects from the data in the passed in stream.</summary>
        /// <param name="reader">Reader which contains the CSV or FixedWidth data to deserialize.</param>
        /// <returns>A collection of TargetType objects.</returns>
        public ICollection<TTargetType> DeserializeArray( TextReader reader )
        {
            return DeserializeArray( reader, 0 );
        }

        /// <summary>Creates a collection of TargetType objects from the data in the passed in stream.</summary>
        /// <param name="reader">Reader which contains the CSV or FixedWidth data to deserialize.</param>
        /// <param name="count">Number of records to read into the stream.</param>
        /// <returns>A collection of TargetType objects.</returns>
        public ICollection<TTargetType> DeserializeArray( TextReader reader, int count )
        {
            List<TTargetType> returnList = new List<TTargetType>();
            bool bContinue = true;
            int recordsLeft = count;

            while ( bContinue )
            {
                string strRow = reader.ReadLine();
                if ( !string.IsNullOrEmpty( strRow ) )
                {
                    returnList.Add( Deserialize( strRow ) );
                    recordsLeft--;
                }
                else
                    bContinue = false;

                if ( count != 0 && recordsLeft <= 0 )
                    bContinue = false;
            }

            return returnList;
        }

        /// <summary>Deserializes a file one record at a time suitable for usage in a foreach loop.</summary>
        /// <param name="reader">Reader which contains the CSV or FixedWidth data to deserialize.</param>
        /// <returns>An enumerable collection of TargetType objects.</returns>
        public IEnumerable<TTargetType> DeserializeEnumerable(TextReader reader)
        {
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

        /// <summary>Write out a list of fields in the correct format.</summary>
        /// <param name="fieldList">List of strings where each string represents one field in the record.</param>
        /// <returns>A single record.</returns>
        protected abstract string FormatOutput( List<string> fieldList );

        /// <summary>Parses a line of text as a record and returns the fields.</summary>
        /// <param name="text">A single line of text for the entire record out of the file.</param>
        /// <returns>A list of strings, where each string represents a field in the record.</returns>
        protected abstract List<string> Parse( string text );
    }
}
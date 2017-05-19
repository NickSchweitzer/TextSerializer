using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using TheCodingMonkey.Serialization.Configuration;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Base class that contains common code for the <see cref="CsvSerializer">CSVSerializer</see> and 
    /// <see cref="FixedWidthSerializer">FixedWidthSerializer</see> class.</summary>
    /// <typeparam name="TTargetType">The type of object that will be serialized.  TargetType must have the 
    /// <see cref="TextSerializable">TextSerializable attribute</see> applied, and any fields contained must have the 
    /// <see cref="TextFieldAttribute">TextField attribute</see> applied to them.</typeparam>
    public abstract class TextSerializer<TTargetType>
        where TTargetType : new()
    {
        protected readonly Type _type;
        protected readonly Dictionary<int, TextFieldAttribute> _textFields = new Dictionary<int, TextFieldAttribute>();

        protected TextSerializer()
        {
            // Get the Reflection type for the Generic Argument
            _type = GetType().GetGenericArguments()[0];
        }

        protected void InitializeFromAttributes()
        {
            // Double check that the TextSerializableAttribute has been attached to the TargetType
            object[] serAttrs = _type.GetCustomAttributes(typeof(TextSerializableAttribute), false);
            if (serAttrs.Length == 0)
                throw new TextSerializationException("TargetType must have a TextSerializableAttribute attached");

            // Get all the public properties and fields on the class
            MemberInfo[] members = _type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty);
            foreach (MemberInfo member in members)
            {
                // Check to see if they've marked up this field/property with the attribute for serialization
                object[] fieldAttrs = member.GetCustomAttributes(typeof(TextFieldAttribute), false);
                if (fieldAttrs.Length > 0)
                {
                    Type memberType;
                    if (member is FieldInfo)
                        memberType = ((FieldInfo)member).FieldType;
                    else if (member is PropertyInfo)
                        memberType = ((PropertyInfo)member).PropertyType;
                    else
                        throw new TextSerializationException("Invalid MemberInfo type encountered");

                    TextFieldAttribute textField = (TextFieldAttribute)fieldAttrs[0];

                    // Check for the AllowedValues Attribute and if it's there, store away the values into the other holder attribute
                    object[] allowedAttrs = member.GetCustomAttributes(typeof(AllowedValuesAttribute), false);
                    if (allowedAttrs.Length > 0)
                    {
                        AllowedValuesAttribute allowedAttr = (AllowedValuesAttribute)allowedAttrs[0];
                        textField.AllowedValues = allowedAttr.AllowedValues;
                    }

                    // If they don't override the name in the Attribute, use the MemberInfo name
                    if (string.IsNullOrEmpty(textField.Name))
                        textField.Name = member.Name;

                    textField.Member = member;
                    _textFields.Add(textField.Position, textField);
                }
            }
        }

        internal Dictionary<int, TextFieldAttribute> Fields
        {
            get { return _textFields; }
        }

        /// <summary>Serializes a single TargetType object into a properly formatted record string.</summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>Record string</returns>
        /// <exception cref="TextSerializationException">Thrown if an invalid class member is encountered.</exception>
        public string Serialize( TTargetType obj )
        {
            // First go through the object and get string representations of all the fields
            List<string> fieldList = new List<string>();
            for ( int i = 0; i < _textFields.Count; i++ )
            {
                TextFieldAttribute attr = _textFields[i];
                object objValue;

                // Get the object from the field
                if ( attr.Member is PropertyInfo )
                    objValue = ( (PropertyInfo)attr.Member ).GetValue( obj, null );
                else if ( attr.Member is FieldInfo )
                    objValue = ( (FieldInfo)attr.Member ).GetValue( obj );
                else
                    throw new TextSerializationException( "Invalid MemberInfo type encountered" );

                // Get the string representation for the object.  If there is a custom formatter for this field, then
                // use that, otherwise use the default ToString behavior.
                string str = attr.Formatter != null ? attr.Formatter.Serialize( objValue ) : objValue.ToString();

                // Truncate the string if required
                fieldList.Add( Truncate( str, attr.Size ) );
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
            if ( _type.IsValueType )
            {
                object tempObj = returnObj;
                returnStruct = (ValueType)tempObj;
            }

            // Parse the record into it's individual fields depending on the type of serializer this is
            List<string> parseList = Parse( text );
            int requiredCount = _textFields.Values.Count(attr => !attr.Optional);
            if ( parseList.Count < requiredCount || parseList.Count > _textFields.Count )
                throw new TextSerializationException( "TargetType field count doesn't match number of items in text" );

            // For each field that is parsed in the string, populate the correct corresponding field in the TargetType
            TextFieldAttribute[] attributes = _textFields.Values.ToArray();
            for ( int i = 0; i < parseList.Count; i++ )
            {
                TextFieldAttribute attr = attributes[i];
                if ( attr != null )
                {
                    string strVal = Truncate( parseList[i], attr.Size );

                    // If there is a custom formatter, then use that to deserialize the string, otherwise use the default .NET behvavior.
                    object fieldObj = attr.Formatter != null ? attr.Formatter.Deserialize( strVal ) : Convert.ChangeType( strVal, attr.GetNativeType() );

                    // Depending on whether the TargetType is a class or struct, you have to populate the fields differently
                    if ( _type.IsValueType )
                        AssignToStruct( returnStruct, fieldObj, attr.Member );
                    else
                        AssignToClass( returnObj, fieldObj, attr.Member );
                }
            }

            // If this was a value type, need to do a little more magic so can be returned properly
            if ( _type.IsValueType )
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
        /// <param name="count">Number of records to deserialize with the enumerable</param>
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

        private static string Truncate( string str, int length )
        {
            if ( length > -1 && str.Length > length )
                str = str.Substring( 0, length );

            return str;
        }

        private static void AssignToClass( object obj, object val, MemberInfo member )
        {
            if ( member is PropertyInfo )
                ( (PropertyInfo)member ).SetValue( obj, val, null );
            else if ( member is FieldInfo )
                ( (FieldInfo)member ).SetValue( obj, val );
            else
                throw new TextSerializationException( "Invalid MemberInfo type encountered" );
        }

        private static void AssignToStruct( ValueType obj, object val, MemberInfo member )
        {
            if ( member is PropertyInfo )
                ( (PropertyInfo)member ).SetValue( obj, val, null );
            else if ( member is FieldInfo )
                ( (FieldInfo)member ).SetValue( obj, val );
            else
                throw new TextSerializationException( "Invalid MemberInfo type encountered" );
        }
    }
}
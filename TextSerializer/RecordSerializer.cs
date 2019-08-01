using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using TheCodingMonkey.Serialization.Formatters;
using TheCodingMonkey.Serialization.Configuration;
using TheCodingMonkey.Serialization.Utilities;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Base class that contains common code for the <see cref="CsvSerializer{TTargetType}">CsvSerializer</see> and 
    /// <see cref="FixedWidthSerializer{TTargetType}">FixedWidthSerializer</see> class.</summary>
    /// <typeparam name="TTargetType">The type of object that will be serialized. Either TargetType must have the 
    /// <see cref="TextSerializableAttribute">TextSerializable attribute</see> applied, and any fields contained must have the 
    /// <see cref="TextFieldAttribute">TextField attribute</see> applied to them, or the Fluent Configuration model must be used.</typeparam>
    public abstract class RecordSerializer<TTargetType>
        where TTargetType : new()
    {
        /// <summary>Dictionary of Fields which have been configured for this Serializer. The Key is the Position, and the Value is the Field definition class.</summary>
        public List<Field> Fields { get; private set; }

        /// <summary>The type which will be created for each record in the file.</summary>
        public Type TargetType { get; private set; }

        /// <summary>Default constructor for the base type. Does only basic initialization of the TargetType.</summary>
        protected RecordSerializer()
        {
            Fields = new List<Field>();

            // Get the Reflection type for the Generic Argument
            TargetType = GetType().GetGenericArguments()[0];
        }

        /// <summary>Used by a derived class to return a Field configuration specific to this serializer back for a given method based on the attributes applied.</summary>
        /// <param name="member">Property or Field to return a configuration for.</param>
        /// <returns>Field configuration if this property should be serialized, otherwise null to ignore.</returns>
        protected abstract Field GetFieldFromAttribute(MemberInfo member);

        /// <summary>Initializes the field definitions for this class using Attributes, which occurs if a Fluent Configuration is not passed into the Constructor.</summary>
        protected virtual void InitializeFromAttributes()
        {
            // Double check that the TextSerializableAttribute has been attached to the TargetType
            TextSerializableAttribute serAttr = TargetType.GetCustomAttribute<TextSerializableAttribute>();
            if (serAttr == null)
                throw new TextSerializationConfigurationException("TargetType must have a TextSerializableAttribute attached");

            // Get all the public properties and fields on the class
            MemberInfo[] members = TargetType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty);
            foreach (MemberInfo member in members)
            {
                Field textField = GetFieldFromAttribute(member);
                if (textField != null)
                {
                    Type memberType;
                    if (member is FieldInfo)
                        memberType = ((FieldInfo)member).FieldType;
                    else if (member is PropertyInfo)
                        memberType = ((PropertyInfo)member).PropertyType;
                    else
                        throw new TextSerializationConfigurationException("Invalid MemberInfo type encountered");

                    // Check for the AllowedValues Attribute and if it's there, store away the values into the other holder attribute
                    AllowedValuesAttribute allowedAttr = member.GetCustomAttribute<AllowedValuesAttribute>();
                    if (allowedAttr != null)
                        textField.AllowedValues = allowedAttr.AllowedValues;

                    if (memberType.IsEnum && textField.FormatterType == null)
                    {
                        FormatEnumAttribute enumAttr = member.GetCustomAttribute<FormatEnumAttribute>();
                        if (enumAttr != null)
                            textField.Formatter = new EnumFormatter(memberType, enumAttr.Options);
                        else
                            textField.Formatter = new EnumFormatter(memberType);
                    }

                    textField.Member = member;
                    Fields.Add(textField);
                }
            }

            // Guarantee that all fields are in order with no gaps
            Fields.Sort((x, y) => x.Position.CompareTo(y.Position));
            for (int i = 0; i < Fields.Count; i++)
            {
                if (Fields[i].Position != i)
                    throw new TextSerializationConfigurationException($"Missing field definition for Position {i}");
            }

        }

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

                // Get the object from the field and format it for output
                object objValue = ReflectionHelper.GetPropertyFieldValue(field, obj, null);
                string str = field.FormatString(objValue);

                // Truncate the string if required
                fieldList.Add( Utilities.ParsingHelper.Truncate( str, field.Size ) );
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
                    string strVal = Utilities.ParsingHelper.Truncate( parseList[i], field.Size );

                    // If there is a custom formatter, then use that to deserialize the string, otherwise use the default .NET behvavior.
                    object fieldObj = field.FormatValue(strVal);

                    // Depending on whether the TargetType is a class or struct, you have to populate the fields differently
                    if ( TargetType.IsValueType )
                        Utilities.ReflectionHelper.AssignToStruct( returnStruct, fieldObj, field.Member );
                    else
                        Utilities.ReflectionHelper.AssignToClass( returnObj, fieldObj, field.Member );
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
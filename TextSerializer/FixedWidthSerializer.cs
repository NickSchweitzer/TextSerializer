using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TheCodingMonkey.Serialization.Configuration;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Used to serialize a TargetType object to a CSV file.</summary>
    /// <typeparam name="TTargetType">The type of object that will be serialized. Either TargetType must have the 
    /// <see cref="TextSerializableAttribute">TextSerializable attribute</see> applied, and any fields contained must have the 
    /// <see cref="FixedWidthFieldAttribute">FixedWidthField attribute</see> applied to them, or <see cref="FixedWidthConfiguration{TTargetType}">Fluent Configuration</see>
    /// must be used.</typeparam>
    public class FixedWidthSerializer<TTargetType> : RecordSerializer<TTargetType>
        where TTargetType : new()
    {
        /// <summary>Initializes a new instance of the FixedWidthSerializer class with default values, using Attributes on the target type
        /// to determine the configuration of fields and properties.</summary>
        public FixedWidthSerializer()
        {
            InitializeFromAttributes();
        }

        /// <summary>Initializes a new instance of the FixedWidthSerializer class using Fluent configuration.</summary>
        ///<param name="config">Fluent configuration for the serializer.</param>
        public FixedWidthSerializer(Action<FixedWidthConfiguration<TTargetType>> config)
        {
            FixedWidthConfiguration<TTargetType> completedConfig = new FixedWidthConfiguration<TTargetType>(this);
            config.Invoke(completedConfig);
            Fields.Sort((x, y) => x.Position.CompareTo(y.Position));

            for (int i = 0; i < Fields.Count; i++)
            {
                if (Fields[i].Position != i)
                    throw new TextSerializationConfigurationException($"Missing field definition for Position {i}");

                if (Fields[i].Size <= 0)
                    throw new TextSerializationConfigurationException("TextField Size must be specified for Fixed Width");
            }
        }

        /// <summary>Used by a derived class to return a Field configuration specific to this serializer back for a given method based on the attributes applied.</summary>
        /// <param name="member">Property or Field to return a configuration for.</param>
        /// <returns>Field configuration if this property should be serialized, otherwise null to ignore.</returns>
        protected override Field GetFieldFromAttribute(MemberInfo member)
        {
            // Check to see if they've marked up this field/property with the attribute for serialization
            FixedWidthFieldAttribute fieldAttr = member.GetCustomAttribute<FixedWidthFieldAttribute>();
            if (fieldAttr != null)
            {
                var attr = fieldAttr.Field;
                if (attr.Size <= 0)
                    throw new TextSerializationConfigurationException("TextField Size must be specified for Fixed Width");
                return attr;
            }
            else
                return null;
        }

        /// <summary>Parses a line of text as a record and returns the fields.</summary>
        /// <param name="text">A single line of text for the entire record out of the file.</param>
        /// <returns>A list of strings, where each string represents a field in the record.</returns>
        /// <exception cref="TextSerializationException">Thrown if the record length does not match the calculated length for the 
        /// TargetType object.  Also thrown if any one field's length is not positive.</exception>
        protected override List<string> Parse( string text )
        {
            List<string> returnList = new List<string>();
            int startPos = 0;

            for ( int i = 0; i < Fields.Count; i++ )
            {
                FixedWidthField field = (FixedWidthField)Fields[i];
                int fieldLen = field.Size;

                // Double check that the field length attribute on the property or field is positive.
                if ( fieldLen <= 0 )
                    throw new TextSerializationConfigurationException( "TextField Size must be specified for Fixed Width" );

                // Double check that the field length for the property won't make us go over the total record length.
                if (startPos + fieldLen > text.Length)
                {
                    if (field.Optional)    // If we're going to go over, and this field is optional, then we can bail
                        break;
                    else                   // Otherwise, this is an error condition
                        throw new TextSerializationException("Fixed width field length mismatch");
                }

                // Trim out any padding characters that may exist
                string strField = text.Substring( startPos, fieldLen );
                returnList.Add( strField.Trim( field.Padding ) );

                startPos += fieldLen;   // Move the pointer
            }

            // Double check that we didn't go UNDER the record length either.
            if (startPos != text.Length)
                throw new TextSerializationException("Fixed width field length mismatch");
            return returnList;
        }

        /// <summary>Write out a list of fields in the correct fixed width format.</summary>
        /// <param name="fieldList">List of strings where each string represents one field in the record.</param>
        /// <returns>A single record.</returns>
        protected override string FormatOutput( List<string> fieldList )
        {
            StringBuilder sb = new StringBuilder();
            for ( int i = 0; i < fieldList.Count; i++ )
            {
                // Compare the length of the field with what the fixed width file is expecting
                int paddingCount = Fields[i].Size - fieldList[i].Length;

                // Pad the string if required
                if ( paddingCount > 0 )
                {
                    FixedWidthField fieldAttr = (FixedWidthField)Fields[i];
                    sb.Append( fieldAttr.Padding, paddingCount );
                }

                // Add the string to the record
                sb.Append( fieldList[i] );
            }

            return sb.ToString();
        }
    }
}
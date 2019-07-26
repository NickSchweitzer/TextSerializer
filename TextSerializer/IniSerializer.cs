using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Used to serialize a TargetType object to an INI file.</summary>
    /// <typeparam name="TTargetType">The type of object that will be serialized.  TargetType either must have the 
    /// <see cref="TextSerializableAttribute">TextSerializable attribute</see> applied, and any fields contained must have the 
    /// <see cref="IniFieldAttribute">IniTextField attribute</see> applied to them, or <see cref="IniConfiguration{TTargetType}">Fluent Configuration</see>
    /// must be used.</typeparam>
    public class IniSerializer<TTargetType> : BaseSerializer<TTargetType>
        where TTargetType : new()
    {
        /// <summary>Used by a derived class to return a Field configuration specific to this serializer back for a given method based on the attributes applied.</summary>
        /// <param name="member">Property or Field to return a configuration for.</param>
        /// <returns>Field configuration if this property should be serialized, otherwise null to ignore.</returns>
        protected override Field GetFieldFromAttribute(MemberInfo member)
        {
            // TODO: Fill this in after create IniFieldAttribute
            return null;
        }

        /// <summary>Serializes a single TargetType object into a properly formatted Ini File.</summary>
        /// <param name="writer">Writer to output serialized data to.</param>
        /// <param name="obj">Object to serialize</param>
        /// <exception cref="TextSerializationException">Thrown if an invalid class member is encountered.</exception>
        public void Serialize(TextWriter writer, TTargetType obj)
        {

        }

        /// <summary>Creates a single TargetType object given a TextReader which has INI data.</summary>
        /// <param name="reader">Reader which contains the INI data to deserialize.</param>
        /// <param name="returnMaybe">Possible return object. Allows creation in calling program.</param>
        /// <returns>Newly created TargetType object.</returns>
        /// <exception cref="ArgumentNullException">Thrown if reader is null.</exception>
        /// <exception cref="TextSerializationException">Thrown if the number and shape of the fields in the INI file doesn't match that in TargetType.</exception>
        public TTargetType Deserialize(TextReader reader, TTargetType returnMaybe = default(TTargetType))
        {
            if (reader == null)
                throw new ArgumentNullException("reader", "reader cannot be null");

            // Create the correct return objects depending on whether this is a value or reference type
            // This makes a difference for reflection later on when populating the fields dynamically.
            TTargetType returnObj = returnMaybe == null || returnMaybe.Equals(default(TTargetType)) ? new TTargetType() : returnMaybe;
            ValueType returnStruct = null;
            if (TargetType.IsValueType)
            {
                object tempObj = returnObj;
                returnStruct = (ValueType)tempObj;
            }

            // TODO: Write Deserialize Code Here

            // If this was a value type, need to do a little more magic so can be returned properly
            if (TargetType.IsValueType)
            {
                object tempObj = (object)returnStruct;
                returnObj = (TTargetType)tempObj;
            }

            return returnObj;
        }
    }
}
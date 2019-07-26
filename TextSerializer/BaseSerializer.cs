using System;
using System.Reflection;
using System.Collections.Generic;
using TheCodingMonkey.Serialization.Formatters;
using TheCodingMonkey.Serialization.Configuration;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Base class that contains common code for the <see cref="RecordSerializer{TTargetType}">RecordSerializer</see> and 
    /// <see cref="IniSerializer{TTargetType}">IniSerializer</see> class.</summary>
    /// <typeparam name="TTargetType">The type of object that will be serialized. Either TargetType must have the 
    /// <see cref="TextSerializableAttribute">TextSerializable attribute</see> applied, and any fields contained must have the 
    /// <see cref="TextFieldAttribute">TextField attribute</see> applied to them, or the Fluent Configuration model must be used.</typeparam>
    public abstract class BaseSerializer<TTargetType>
        where TTargetType : new()
    {
        /// <summary>Default constructor for the base type. Does only basic initialization of the TargetType.</summary>
        protected BaseSerializer()
        {
            Fields = new List<Field>();

            // Get the Reflection type for the Generic Argument
            TargetType = GetType().GetGenericArguments()[0];
        }

        /// <summary>Dictionary of Fields which have been configured for this Serializer. The Key is the Position, and the Value is the Field definition class.</summary>
        public List<Field> Fields { get; private set; }

        /// <summary>The type which will be created for each record in the file.</summary>
        public Type TargetType { get; private set; }

        /// <summary>Used by a derived class to return a Field configuration specific to this serializer back for a given method based on the attributes applied.</summary>
        /// <param name="member">Property or Field to return a configuration for.</param>
        /// <returns>Field configuration if this property should be serialized, otherwise null to ignore.</returns>
        protected abstract Field GetFieldFromAttribute(MemberInfo member);

        /// <summary>Initializes the field definitions for this class using Attributes, which occurs if a Fluent Configuration is not passed into the Constructor.</summary>
        protected void InitializeFromAttributes()
        {
            // Double check that the TextSerializableAttribute has been attached to the TargetType
            object[] serAttrs = TargetType.GetCustomAttributes(typeof(TextSerializableAttribute), false);
            if (serAttrs.Length == 0)
                throw new TextSerializationException("TargetType must have a TextSerializableAttribute attached");

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
                        throw new TextSerializationException("Invalid MemberInfo type encountered");

                    // Check for the AllowedValues Attribute and if it's there, store away the values into the other holder attribute
                    object[] allowedAttrs = member.GetCustomAttributes(typeof(AllowedValuesAttribute), false);
                    if (allowedAttrs.Length > 0)
                    {
                        AllowedValuesAttribute allowedAttr = (AllowedValuesAttribute)allowedAttrs[0];
                        textField.AllowedValues = allowedAttr.AllowedValues;
                    }

                    if (memberType.IsEnum && textField.FormatterType == null)
                    {
                        object[] enumAttrs = member.GetCustomAttributes(typeof(FormatEnumAttribute), false);
                        if (enumAttrs.Length > 0)
                        {
                            FormatEnumAttribute enumAttr = (FormatEnumAttribute)enumAttrs[0];
                            textField.Formatter = new EnumFormatter(memberType, enumAttr.Options);
                        }
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

        /// <summary>Helper method which safely truncates a string to a given length</summary>
        /// <param name="str">String value to truncate</param>
        /// <param name="length">Maximum allowed length of the string</param>
        /// <returns>Truncated string value</returns>
        protected static string Truncate(string str, int length)
        {
            if (length > -1 && str.Length > length)
                str = str.Substring(0, length);

            return str;
        }

        /// <summary>Helper method which uses reflection to assign a value to a property of a class.</summary>
        /// <param name="obj">Class object which contains the property receiving the value</param>
        /// <param name="val">Value which is bieng assigned</param>
        /// <param name="member">MemberInfo of the Property or Field being assigned to.</param>
        protected static void AssignToClass(object obj, object val, MemberInfo member)
        {
            if (member is PropertyInfo)
                ((PropertyInfo)member).SetValue(obj, val, null);
            else if (member is FieldInfo)
                ((FieldInfo)member).SetValue(obj, val);
            else
                throw new TextSerializationException("Invalid MemberInfo type encountered");
        }

        /// <summary>Helper method which uses reflection to assign a value to a property of a structure.</summary>
        /// <param name="obj">Structure object which contains the property receiving the value</param>
        /// <param name="val">Value which is bieng assigned</param>
        /// <param name="member">MemberInfo of the Property or Field being assigned to.</param>
        protected static void AssignToStruct(ValueType obj, object val, MemberInfo member)
        {
            if (member is PropertyInfo)
                ((PropertyInfo)member).SetValue(obj, val, null);
            else if (member is FieldInfo)
                ((FieldInfo)member).SetValue(obj, val);
            else
                throw new TextSerializationException("Invalid MemberInfo type encountered");
        }
    }
}
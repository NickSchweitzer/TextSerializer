using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheCodingMonkey.Serialization.Configuration;
using TheCodingMonkey.Serialization.Formatters;
using TheCodingMonkey.Serialization.Utilities;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Contains configuration properties for a class/struct that represents a section in an INI file</summary>
    public class IniSection
    {
        /// <summary>Default Constructor for the IniSection</summary>
        public IniSection()
        {
            Fields = new List<IniField>();
        }

        internal IniSection(Type type)
        : this()
        {
            InitializeClassFromAttributes(type);
        }

        /// <summary>Class or Struct that this Section will be Serialized From/Deserialized To</summary>
        public Type SectionType { get; set; }

        /// <summary>Section Name as it appears in the INI file</summary>
        public string Name { get; set; }

        /// <summary>List of Fields in this Section, with the corresponding data about the class Properties/Fields</summary>
        public List<IniField> Fields { get; set; }

        internal IniField GetFieldByName(string propertyName)
        {
            return Fields.Where(f => f.Name == propertyName).FirstOrDefault() as IniField;
        }

        internal IniField GetListField()
        {
            return Fields.Where(f => f.IsList).FirstOrDefault();
        }

        internal IniField GetDictionaryField()
        {
            return Fields.Where(f => f.IsDictionary).FirstOrDefault();
        }

        internal IniField GetSectionNameField()
        {
            return Fields.Where(f => f.IsSectionName).FirstOrDefault();
        }

        private void InitializeClassFromAttributes(Type type)
        {
            SectionType = type;

            // Double check that the TextSerializableAttribute has been attached to the TargetType
            object[] serAttrs = type.GetCustomAttributes(typeof(TextSerializableAttribute), false);
            if (serAttrs.Length == 0)
                throw new TextSerializationConfigurationException($"{type.Name} must have a TextSerializableAttribute attached");

            // Look for a custom Ini Section Name
            object[] sectionAttrs = type.GetCustomAttributes(typeof(IniSectionAttribute), false);
            if (sectionAttrs.Length == 0)
                Name = type.Name;
            else
            {
                IniSectionAttribute sectionAttr = (IniSectionAttribute)sectionAttrs[0];
                Name = sectionAttr.Name;
            }

            // Get all the public properties and fields on the class
            MemberInfo[] members = type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty);
            foreach (MemberInfo member in members)
            {
                IniField iniField = GetFieldFromAttribute(member);
                if (iniField != null)
                {
                    Type memberType;
                    if (member is FieldInfo)
                        memberType = ((FieldInfo)member).FieldType;
                    else if (member is PropertyInfo)
                        memberType = ((PropertyInfo)member).PropertyType;
                    else
                        throw new TextSerializationException("Invalid MemberInfo type encountered");

                    // Check for the AllowedValues Attribute and if it's there, store away the values into the other holder attribute
                    AllowedValuesAttribute allowedAttr = member.GetCustomAttribute<AllowedValuesAttribute>();
                    if (allowedAttr != null)
                        iniField.AllowedValues = allowedAttr.AllowedValues;

                    // Check to see if one of these properties is marked as the Section - This is used when creating a list of sections
                    // of the parent object, and the section name will be stored as a property on the subclass
                    IniSectionAttribute sectionNameAttr = member.GetCustomAttribute<IniSectionAttribute>();
                    if (sectionNameAttr != null)
                        iniField.IsSectionName = true;

                    // TODO - The code below doesn't handle the case where the property is declared as an Interface itself. Need to fix that
                    // Check to see if this is a List or a Dictionary - We do special things with those
                    if (memberType.GetInterface("System.Collections.IList") != null)
                    {
                        iniField.IsList = true;
                        // Catch the case where its a list of complex objects, not primitives
                        Type innerType = memberType.GenericTypeArguments[0];
                        if (innerType.IsUserDefinedClass() || innerType.IsUserDefinedStruct())
                            iniField.Section = new IniSection(innerType);
                    }
                    else if (memberType.GetInterface("System.Collections.IDictionary") != null)
                    {
                        iniField.IsDictionary = true;
                        // Catch the case where its a dictionary of complex objects, not primitives
                        Type innerType = memberType.GenericTypeArguments[1];
                        if (innerType.IsUserDefinedClass() || innerType.IsUserDefinedStruct())
                            iniField.Section = new IniSection(innerType);
                    }
                    else if (memberType.IsEnum && iniField.FormatterType == null)
                    {
                        FormatEnumAttribute enumAttr = member.GetCustomAttribute<FormatEnumAttribute>();
                        if (enumAttr != null)
                            iniField.Formatter = new EnumFormatter(memberType, enumAttr.Options);
                        else
                            iniField.Formatter = new EnumFormatter(memberType);
                    }
                    else if (memberType.IsUserDefinedClass() || memberType.IsUserDefinedStruct())
                        iniField.Section = new IniSection(memberType);

                    iniField.Member = member;
                    Fields.Add(iniField);
                }
            }

            // Guarantee that all fields are in order with no gaps
            // TODO - Determine if need to actually care about Position sorting or Gaps for INI files - More important during Serialization to guarantee same order as Deserialization?
            //Fields.Sort((x, y) => x.Position.CompareTo(y.Position));
            //for (int i = 0; i < Fields.Count; i++)
            //{
            //    if (Fields[i].Position != i)
            //        throw new TextSerializationConfigurationException($"Missing field definition for Position {i}");
            //}
        }

        /// <summary>Used by a derived class to return a Field configuration specific to this serializer back for a given method based on the attributes applied.</summary>
        /// <param name="member">Property or Field to return a configuration for.</param>
        /// <returns>Field configuration if this property should be serialized, otherwise null to ignore.</returns>
        protected IniField GetFieldFromAttribute(MemberInfo member)
        {
            // Check to see if they've marked up this field/property with the attribute for serialization
            IniFieldAttribute fieldAttr = member.GetCustomAttribute<IniFieldAttribute>();
            if (fieldAttr != null)
                return fieldAttr.Field;
            else
                return null;
        }
    }
}
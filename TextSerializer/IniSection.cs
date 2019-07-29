using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheCodingMonkey.Serialization.Configuration;
using TheCodingMonkey.Serialization.Formatters;

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

        internal void InitializeClassFromAttributes(Type type)
        {
            SectionType = type;

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
                    object[] allowedAttrs = member.GetCustomAttributes(typeof(AllowedValuesAttribute), false);
                    if (allowedAttrs.Length > 0)
                    {
                        AllowedValuesAttribute allowedAttr = (AllowedValuesAttribute)allowedAttrs[0];
                        iniField.AllowedValues = allowedAttr.AllowedValues;
                    }

                    // TODO - The code below doesn't handle the case where the property is declared as an Interface itself. Need to fix that

                    // Check to see if this is a List or a Dictionary - We do special things with those
                    if (memberType.GetInterface("System.Collections.IList") != null)
                            iniField.IsList = true;
                    if (memberType.GetInterface("System.Collections.IDictionary") != null)
                        iniField.IsDictionary = true;

                    if (memberType.IsEnum && iniField.FormatterType == null)
                    {
                        object[] enumAttrs = member.GetCustomAttributes(typeof(FormatEnumAttribute), false);
                        if (enumAttrs.Length > 0)
                        {
                            FormatEnumAttribute enumAttr = (FormatEnumAttribute)enumAttrs[0];
                            iniField.Formatter = new EnumFormatter(memberType, enumAttr.Options);
                        }
                        else
                            iniField.Formatter = new EnumFormatter(memberType);
                    }

                    iniField.Member = member;
                    Fields.Add(iniField);
                }
            }

            // Guarantee that all fields are in order with no gaps
            // TODO - Determine if need to actually care about Position sorting or Gaps for INI files
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
            object[] fieldAttrs = member.GetCustomAttributes(typeof(IniFieldAttribute), false);
            if (fieldAttrs.Length > 0)
                return ((IniFieldAttribute)fieldAttrs[0]).Field;
            else
                return null;
        }
    }
}
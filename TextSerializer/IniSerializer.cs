using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using TheCodingMonkey.Serialization.Utilities;
using TheCodingMonkey.Serialization.Configuration;
using TheCodingMonkey.Serialization.Formatters;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Used to serialize a TargetType object to an INI file.</summary>
    /// <typeparam name="TTargetType">The type of object that will be serialized.  TargetType either must have the 
    /// <see cref="TextSerializableAttribute">TextSerializable attribute</see> applied, and any fields contained must have the 
    /// <see cref="IniFieldAttribute">IniField attribute</see>/<see cref="IniSectionAttribute">IniSection attribute</see> applied to them, or <see cref="IniConfiguration{TTargetType}">Fluent Configuration</see>
    /// must be used.</typeparam>
    public class IniSerializer<TTargetType>
        where TTargetType : new()
    {
        private IniSection Section { get; set; }

        /// <summary>The type which will be created for each record in the file.</summary>
        public Type TargetType { get; private set; }

        /// <summary>Initializes a new instance of the IniSerializer class with default values, using Attributes on the target type
        /// to determine the configuration of fields and properties.</summary>
        public IniSerializer()
        {
            // Get the Reflection type for the Generic Argument
            TargetType = GetType().GetGenericArguments()[0];
            InitializeFromAttributes();
        }

        /// <summary>Initializes a new instance of the CSVSerializer class using Fluent configuration.</summary>
        ///<param name="config">Fluent configuration for the serializer.</param>
        public IniSerializer(Action<IniConfiguration<TTargetType>> config)
        : this()
        {
            IniConfiguration<TTargetType> completedConfig = new IniConfiguration<TTargetType>(this);
            config.Invoke(completedConfig);

            
        }

        /// <summary>Initializes the field definitions for this class using Attributes, which occurs if a Fluent Configuration is not passed into the Constructor.</summary>
        protected virtual void InitializeFromAttributes()
        {
            Section = new IniSection(TargetType);
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
            object returnObj = returnMaybe == null || returnMaybe.Equals(default(TTargetType)) ? new TTargetType() : returnMaybe;
            ValueType returnStruct = null;
            if (TargetType.IsValueType)
            {
                object tempObj = returnObj;
                returnStruct = (ValueType)tempObj;
            }

            var sections = ParsingHelper.SplitIniSections(reader);
            foreach (var (SectionName, Text) in sections)
            {
                // We're in this classes key value pair properties
                if (string.IsNullOrEmpty(SectionName) || SectionName == Section.Name)
                {
                    DeserializeClass(Text, Section, returnObj, returnStruct);
                }
                else
                {
                    // TODO - IList and IDictionary code assumes that the List and Dictionary were already created by the Model owner. Should handle the case wehre the Serializer creates it.
                    IniField field = Section.GetFieldByName(SectionName);
                    if (field == null)
                    {
                        // Handle the case where sections are instances of a subclass that get added to a list or a dictionary
                        IniField dictionaryField = Section.GetDictionaryField();
                        if (dictionaryField != null)
                        {
                            object objDictionary;
                            // Get the object from the field
                            if (dictionaryField.Member is PropertyInfo)
                                objDictionary = ((PropertyInfo)dictionaryField.Member).GetValue(returnObj.GetType().IsValueType ? returnStruct : returnObj, null);
                            else if (dictionaryField.Member is FieldInfo)
                                objDictionary = ((FieldInfo)dictionaryField.Member).GetValue(returnObj.GetType().IsValueType ? returnStruct : returnObj);
                            else
                                throw new TextSerializationException("Invalid MemberInfo type encountered");

                            IDictionary dictionary = objDictionary as IDictionary;
                            object sectionObj = Activator.CreateInstance(dictionaryField.Section.SectionType);
                            ValueType sectionStruct = null;
                            if (dictionaryField.Section.SectionType.IsValueType)
                            {
                                object tempObj = sectionObj;
                                sectionStruct = (ValueType)tempObj;
                            }

                            DeserializeClass(Text, dictionaryField.Section, sectionObj, sectionStruct);
                            dictionary.Add(SectionName, sectionObj.GetType().IsValueType ? sectionStruct : sectionObj);
                        }
                    }
                    else if (field.IsList)
                        DeserializeList(field, Text, returnObj, returnStruct);
                    else if (field.IsDictionary)
                        DeserializeDictionary(field, Text, returnObj, returnStruct);
                    else if (field.Section != null)
                    {
                        object sectionObj = Activator.CreateInstance(field.Section.SectionType);
                        ValueType sectionStruct = null;
                        if (field.Section.SectionType.IsValueType)
                        {
                            object tempObj = sectionObj;
                            sectionStruct = (ValueType)tempObj;
                        }

                        DeserializeClass(Text, field.Section, sectionObj, sectionStruct);

                        // Depending on whether the TargetType is a class or struct, you have to populate the fields differently
                        if (returnObj.GetType().IsValueType)
                            ReflectionHelper.AssignToStruct(returnStruct, sectionObj.GetType().IsValueType ? sectionStruct : sectionObj, field.Member);
                        else
                            ReflectionHelper.AssignToClass(returnObj, sectionObj.GetType().IsValueType ? sectionStruct : sectionObj, field.Member);
                    }
                }
            }    

            // If this was a value type, need to do a little more magic so can be returned properly
            if (TargetType.IsValueType)
            {
                object tempObj = (object)returnStruct;
                returnObj = (TTargetType)tempObj;
            }

            return (TTargetType)returnObj;
        }

        private static void DeserializeClass(string text, IniSection section, object returnObj, ValueType returnStruct)
        {
            foreach (string line in text.Split(Environment.NewLine.ToCharArray()))
            {
                var parsedLine = ParsingHelper.ParseIniLine(line);
                if (parsedLine.LineType == IniLineType.BlankLine || parsedLine.LineType == IniLineType.Comment)
                    continue;

                if (parsedLine.LineType != IniLineType.KeyValuePair)
                    throw new TextSerializationException($"Non Key Value Pair items can only be added to Lists");

                IniField field = section.GetFieldByName(parsedLine.Key);
                if (field == null)
                {
                    IniField dictionaryField = section.GetDictionaryField();
                    IniField listField = section.GetListField();
                    if (dictionaryField == null && listField == null)
                        throw new TextSerializationException($"Missing Property mapping for Key {parsedLine.Key}");
                    else
                    {
                        // Catches the case where the object just contains a single Dictionary or List as a catch all for everything in the section
                        if (dictionaryField != null)
                            DeserializeDictionary(dictionaryField, text, returnObj, returnStruct);
                        else
                            DeserializeList(listField, text, returnObj, returnStruct);

                        return;
                    }
                }

                // If there is a custom formatter, then use that to deserialize the string, otherwise use the default .NET behvavior.
                object fieldObj = field.Formatter != null ? field.Formatter.Deserialize(parsedLine.Value) : Convert.ChangeType(parsedLine.Value, field.GetNativeType());

                // Depending on whether the TargetType is a class or struct, you have to populate the fields differently
                if (returnObj.GetType().IsValueType)
                    ReflectionHelper.AssignToStruct(returnStruct, fieldObj, field.Member);
                else
                    ReflectionHelper.AssignToClass(returnObj, fieldObj, field.Member);
            }
        }

        private static void DeserializeList(IniField field, string text, object returnObj, ValueType returnStruct)
        {
            object objList;
            // Get the object from the field
            if (field.Member is PropertyInfo)
                objList = ((PropertyInfo)field.Member).GetValue(returnObj.GetType().IsValueType ? returnStruct : returnObj, null);
            else if (field.Member is FieldInfo)
                objList = ((FieldInfo)field.Member).GetValue(returnObj.GetType().IsValueType ? returnStruct : returnObj);
            else
                throw new TextSerializationException("Invalid MemberInfo type encountered");

            IList list = objList as IList;

            foreach (string line in text.Split(Environment.NewLine.ToCharArray()))
            {
                var parsedLine = ParsingHelper.ParseIniLine(line);
                if (parsedLine.LineType == IniLineType.BlankLine || parsedLine.LineType == IniLineType.Comment)
                    continue;

                if (parsedLine.LineType != IniLineType.Item)
                    throw new TextSerializationException("Attempting to add something other than a list item to an IList");

                // If there is a custom formatter, then use that to deserialize the string, otherwise use the default .NET behvavior.
                object fieldObj = null;
                if (field.Formatter != null)
                    fieldObj = field.Formatter.Deserialize(parsedLine.Key);
                else
                {
                    Type listType = field.GetNativeType();
                    Type innerType = listType.GenericTypeArguments[0];
                    fieldObj = Convert.ChangeType(parsedLine.Key, innerType);
                }
                list.Add(fieldObj);
            }
        }

        private static void DeserializeDictionary(IniField field, string text, object returnObj, ValueType returnStruct)
        {
            object objDictionary;
            // Get the object from the field
            if (field.Member is PropertyInfo)
                objDictionary = ((PropertyInfo)field.Member).GetValue(returnObj.GetType().IsValueType ? returnStruct : returnObj, null);
            else if (field.Member is FieldInfo)
                objDictionary = ((FieldInfo)field.Member).GetValue(returnObj.GetType().IsValueType ? returnStruct : returnObj);
            else
                throw new TextSerializationException("Invalid MemberInfo type encountered");

            IDictionary dictionary = objDictionary as IDictionary;

            foreach (string line in text.Split(Environment.NewLine.ToCharArray()))
            {
                var parsedLine = ParsingHelper.ParseIniLine(line);
                if (parsedLine.LineType == IniLineType.BlankLine || parsedLine.LineType == IniLineType.Comment)
                    continue;

                if (parsedLine.LineType != IniLineType.KeyValuePair)
                    throw new TextSerializationException("Attempting to add something other than a key value pair to an IDictionary");

                // If there is a custom formatter, then use that to deserialize the string, otherwise use the default .NET behvavior.
                object fieldObj = null;
                if (field.Formatter != null)
                    fieldObj = field.Formatter.Deserialize(parsedLine.Value);
                else
                {
                    Type dictionaryType = field.GetNativeType();
                    Type innerType = dictionaryType.GenericTypeArguments[1];
                    fieldObj = Convert.ChangeType(parsedLine.Value, innerType);
                }
                dictionary.Add(parsedLine.Key, fieldObj);
            }
        }
    }
}
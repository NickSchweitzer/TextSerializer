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
        private Dictionary<string, IniSection> Sections { get; set; }

        /// <summary>The type which will be created for each record in the file.</summary>
        public Type TargetType { get; private set; }

        /// <summary>Initializes a new instance of the IniSerializer class with default values, using Attributes on the target type
        /// to determine the configuration of fields and properties.</summary>
        public IniSerializer()
        {
            Sections = new Dictionary<string, IniSection>();

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
            // Double check that the TextSerializableAttribute has been attached to the TargetType
            object[] serAttrs = TargetType.GetCustomAttributes(typeof(TextSerializableAttribute), false);
            if (serAttrs.Length == 0)
                throw new TextSerializationException("TargetType must have a TextSerializableAttribute attached");

            InitializeClassFromAttributes(TargetType);
        }

        private void InitializeClassFromAttributes(Type type)
        {
            IniSection section = new IniSection();
            section.InitializeClassFromAttributes(type);
            Sections.Add(section.Name, section);

            // Special case for the Parent Type - This can also be a no-section name section
            if (TargetType == type)
                Sections.Add(string.Empty, section);
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

            // Create objects that maintain what Ini Section we're currently operating in
            // Until we get a Section declaration line, we'll assume that all lines should get deserialized into return object
            IniSection currentSection = Sections[string.Empty];
            object sectionObj = returnObj;
            ValueType sectionStruct = null;
            if (sectionObj.GetType().IsValueType)
            {
                object tempSectionObj = sectionObj;
                sectionStruct = (ValueType)tempSectionObj;
            }

            bool bContinue = true;
            while (bContinue)
            {
                string strRow = reader.ReadLine();
                if (!string.IsNullOrEmpty(strRow))
                {
                    var parsedLine = ParsingHelper.ParseIniLine(strRow);
                    if (parsedLine.Item1 == IniLineType.BlankLine || parsedLine.Item1 == IniLineType.Comment)
                        continue;

                    // TODO - IList and IDictionary code assumes that the List and Dictionary were already created by the Model creator. Should handle the case wehre the Serializer creates it.

                    if (parsedLine.Item1 == IniLineType.KeyValuePair)
                    {
                        IniField field = currentSection.GetFieldByName(parsedLine.Item2);
                        if (field != null)
                        {
                            // If there is a custom formatter, then use that to deserialize the string, otherwise use the default .NET behvavior.
                            object fieldObj = field.Formatter != null ? field.Formatter.Deserialize(parsedLine.Item3) : Convert.ChangeType(parsedLine.Item3, field.GetNativeType());

                            // Depending on whether the TargetType is a class or struct, you have to populate the fields differently
                            if (sectionObj.GetType().IsValueType)
                                Utilities.ReflectionHelper.AssignToStruct(returnStruct, fieldObj, field.Member);
                            else
                                Utilities.ReflectionHelper.AssignToClass(returnObj, fieldObj, field.Member);
                        }
                        else
                        {
                            // Check to see if one of the properties is a Dictionary and add it to that
                            IniField dictField = currentSection.GetDictionaryField();
                            if (dictField != null)
                            {
                                object objDictionary;
                                // Get the object from the field
                                if (dictField.Member is PropertyInfo)
                                    objDictionary = ((PropertyInfo)dictField.Member).GetValue(sectionObj.GetType().IsValueType ? sectionStruct : sectionObj, null);
                                else if (dictField.Member is FieldInfo)
                                    objDictionary = ((FieldInfo)dictField.Member).GetValue(sectionObj.GetType().IsValueType ? sectionStruct : sectionObj);
                                else
                                    throw new TextSerializationException("Invalid MemberInfo type encountered");

                                IDictionary dictionary = objDictionary as IDictionary;
                                if (dictionary == null)
                                    throw new TextSerializationException($"Field mapped for {parsedLine.Item2} does not implement IDictionary");

                                // If there is a custom formatter, then use that to deserialize the string, otherwise use the default .NET behvavior.
                                object fieldObj = null;
                                if (dictField.Formatter != null)
                                    fieldObj = dictField.Formatter.Deserialize(parsedLine.Item3);
                                else
                                {
                                    Type listType = dictField.GetNativeType();
                                    Type innerType = listType.GenericTypeArguments[1];
                                    fieldObj = Convert.ChangeType(parsedLine.Item3, innerType);
                                }
                                dictionary.Add(parsedLine.Item2, fieldObj);
                            }
                            else
                                throw new TextSerializationException($"Missing property {currentSection.Name}/{parsedLine.Item2}");

                        }
                    }
                    else if (parsedLine.Item1 == IniLineType.Item)
                    {
                        Field field = currentSection.GetListField();
                        object objList;
                        // Get the object from the field
                        if (field.Member is PropertyInfo)
                            objList = ((PropertyInfo)field.Member).GetValue(sectionObj.GetType().IsValueType ? sectionStruct : sectionObj, null);
                        else if (field.Member is FieldInfo)
                            objList = ((FieldInfo)field.Member).GetValue(sectionObj.GetType().IsValueType ? sectionStruct : sectionObj);
                        else
                            throw new TextSerializationException("Invalid MemberInfo type encountered");

                        IList list = objList as IList;
                        if (list == null)
                            throw new TextSerializationException($"Field mapped for {parsedLine.Item2} does not implement IList");

                        // If there is a custom formatter, then use that to deserialize the string, otherwise use the default .NET behvavior.
                        object fieldObj = null;
                        if (field.Formatter != null)
                            fieldObj = field.Formatter.Deserialize(parsedLine.Item2);
                        else
                        {
                            Type listType = field.GetNativeType();
                            Type innerType = listType.GenericTypeArguments[0];
                            fieldObj = Convert.ChangeType(parsedLine.Item2, innerType);
                        }
                        list.Add(fieldObj);
                    }
                    else if (parsedLine.Item1 == IniLineType.Section)
                    {
                        if (Sections.ContainsKey(parsedLine.Item2))
                        {
                            IniSection newSection = Sections[parsedLine.Item2];
                            if (newSection != currentSection)
                            {
                                // TODO - Do some object assignments here and create new section objections
                            }
                        }
                        else
                        {
                            // Check to see if this is a section acting as a list or dictionary property
                            IniField field = currentSection.GetFieldByName(parsedLine.Item2);
                            if (field == null || (!field.IsList && !field.IsDictionary))
                                throw new TextSerializationException($"Invalid Section Encountered {parsedLine.Item2}");
                        }
                    }
                }
                else if (strRow == null)    // EOF
                    bContinue = false;
            }

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
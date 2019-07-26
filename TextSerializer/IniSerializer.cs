using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TheCodingMonkey.Serialization.Utilities;
using TheCodingMonkey.Serialization.Configuration;
using System.Linq;

namespace TheCodingMonkey.Serialization
{
    /// <summary>Used to serialize a TargetType object to an INI file.</summary>
    /// <typeparam name="TTargetType">The type of object that will be serialized.  TargetType either must have the 
    /// <see cref="TextSerializableAttribute">TextSerializable attribute</see> applied, and any fields contained must have the 
    /// <see cref="IniFieldAttribute">IniField attribute</see>/<see cref="IniSectionAttribute">IniSection attribute</see> applied to them, or <see cref="IniConfiguration{TTargetType}">Fluent Configuration</see>
    /// must be used.</typeparam>
    public class IniSerializer<TTargetType> : BaseSerializer<TTargetType>
        where TTargetType : new()
    {
        /// <summary>Initializes a new instance of the IniSerializer class with default values, using Attributes on the target type
        /// to determine the configuration of fields and properties.</summary>
        public IniSerializer()
        { }

        /// <summary>Initializes a new instance of the CSVSerializer class using Fluent configuration.</summary>
        ///<param name="config">Fluent configuration for the serializer.</param>
        public IniSerializer(Action<IniConfiguration<TTargetType>> config)
        {
            IniConfiguration<TTargetType> completedConfig = new IniConfiguration<TTargetType>(this);
            config.Invoke(completedConfig);
            Fields.Sort((x, y) => x.Position.CompareTo(y.Position));

            
        }

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

            // Create objects that maintain what Ini Section we're currently operating in
            // Until we get a Section declaration line, we'll assume that all lines should get deserialized into return object
            string currentSectionName = string.Empty;
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
                    var parsedLine = ParseLine(strRow);
                    if (parsedLine.Item1 == IniLineType.BlankLine || parsedLine.Item1 == IniLineType.Comment)
                        continue;

                    if (parsedLine.Item1 == IniLineType.KeyValuePair)
                    {
                        IniField field = GetFieldByName(currentSectionName, parsedLine.Item2);
                        if (field == null)
                            throw new TextSerializationException($"Missing property {currentSectionName}/{parsedLine.Item2}");
                        else
                        {
                            // If there is a custom formatter, then use that to deserialize the string, otherwise use the default .NET behvavior.
                            object fieldObj = field.Formatter != null ? field.Formatter.Deserialize(parsedLine.Item2) : Convert.ChangeType(parsedLine.Item2, field.GetNativeType());

                            // Depending on whether the TargetType is a class or struct, you have to populate the fields differently
                            if (sectionObj.GetType().IsValueType)
                                AssignToStruct(returnStruct, fieldObj, field.Member);
                            else
                                AssignToClass(returnObj, fieldObj, field.Member);
                        }
                    }
                    else if (parsedLine.Item1 == IniLineType.Item)
                    {
                        Field field = GetListField(currentSectionName);
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
                        object fieldObj = field.Formatter != null ? field.Formatter.Deserialize(parsedLine.Item3) : Convert.ChangeType(parsedLine.Item3, field.GetNativeType());
                        list.Add(fieldObj);
                    }
                    else if (parsedLine.Item1 == IniLineType.Section)
                    {
                        // TODO - Handle Changing Sections
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

        private IniField GetFieldByName(string sectionName, string propertyName)
        {
            return Fields.Where(f => ((IniField)f).Name == propertyName && ((IniField)f).SectionName == sectionName).FirstOrDefault() as IniField;
        }

        private Field GetListField(string sectionName)
        {
            return Fields.Where(f => ((IniField)f).IsList && ((IniField)f).SectionName == sectionName).FirstOrDefault();
        }

        private Tuple<IniLineType, string, string> ParseLine(string text)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return new Tuple<IniLineType, string, string>(IniLineType.BlankLine, null, null);

            if (text.StartsWith(";"))
                return new Tuple<IniLineType, string, string>(IniLineType.Comment, text, null);

            if (text.StartsWith("[") && text.EndsWith("]"))
                return new Tuple<IniLineType, string, string>(IniLineType.Section, text.Substring(1, text.Length - 2), null);

            List<string> parsedLine = ParsingHelper.ParseDelimited(text, '\"', '=');
            if (parsedLine.Count == 1)
                return new Tuple<IniLineType, string, string>(IniLineType.Item, parsedLine[0].Trim(), null);
            else if (parsedLine.Count == 2)
                return new Tuple<IniLineType, string, string>(IniLineType.KeyValuePair, parsedLine[0].Trim(), parsedLine[1].Trim());

            throw new TextSerializationException($"Cannot parse line in INI file '{text}'");
        }

        private enum IniLineType
        {
            BlankLine,
            Comment,
            Section,
            Item,
            KeyValuePair
        }
    }
}
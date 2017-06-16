using System;
using System.Linq.Expressions;
using System.Reflection;
using TheCodingMonkey.Serialization.Formatters;
using TheCodingMonkey.Serialization.Utilities;

namespace TheCodingMonkey.Serialization.Configuration
{
    /// <summary>Fluent Configuration class for the <see cref="CsvSerializer{TTargetType}">CsvSerializer</see> class.</summary>
    public class CsvConfiguration<TTargetType> : TextConfiguration<TTargetType>
        where TTargetType : new()
    {
        private CsvSerializer<TTargetType> CsvSerializer { get; set; }

        internal CsvConfiguration(CsvSerializer<TTargetType> serializer)
        : base(serializer)
        {
            CsvSerializer = serializer;

            AlwaysWriteQualifier(true);
            Delimiter(',');
            Qualifier('"');
        }

        /// <summary>Automatically configures serializer based on the class definition. All properties and fields are added as serializable in the order they appear in the class.</summary>
        /// <exception cref="TextSerializationConfigurationException">ByConvention fails if a class or structure contains a mix of Properties and Fields. Because of the limitations
        /// of the .NET Reflection API, the order cannot be properly determined at runtime if both appear in the same class.</exception>
        public CsvConfiguration<TTargetType> ByConvention()
        {
            MemberInfo[] members = Serializer.TargetType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty);
            int position = 0;
            bool foundProperty = false;
            bool foundField = false;
            foreach (MemberInfo member in members)
            {
                Type memberType = null;
                if (member is FieldInfo)
                {
                    memberType = ((FieldInfo)member).FieldType;
                    foundField = true;
                }
                else if (member is PropertyInfo)
                {
                    memberType = ((PropertyInfo)member).PropertyType;
                    foundProperty = true;
                }

                if (foundProperty && foundField)
                    throw new TextSerializationConfigurationException("Cannot configure classes by Convention with both Properties and Fields because cannot get order correct automatically. Use explicit configuration methods.");

                if (memberType != null)
                {
                    CsvField textField = new CsvField
                    {
                        Name = member.Name,
                        Member = member,
                        Position = position++
                    };

                    if (memberType.IsEnum)
                        textField.Formatter = new EnumFormatter(memberType);

                    Serializer.Fields.Add(textField);
                }
            }

            return this;
        }

        /// <summary>True if should wrap every field in the <see cref="Qualifier">Qualifier</see> during serialization.  If false, then
        /// the qualifier is only written if the field contains the <see cref="Delimiter">Delimiter</see>.</summary>
        public CsvConfiguration<TTargetType> AlwaysWriteQualifier(bool alwaysWrite = true)
        {
            CsvSerializer.AlwaysWriteQualifier = alwaysWrite;
            return this;
        }

        /// <summary>Character which is used to delimit fields in the record.</summary>
        public CsvConfiguration<TTargetType> Delimiter(char delim)
        {
            CsvSerializer.Delimiter = delim;
            return this;
        }

        /// <summary>Character used to wrap a field if the field contins the <see cref="Delimiter">Delimiter</see>.</summary>
        public CsvConfiguration<TTargetType> Qualifier(char qualifier)
        {
            CsvSerializer.Qualifier = qualifier;
            return this;
        }

        /// <summary>Sets the serialization properties of a member of the class. If ByConvention was called first, this will override those inferred settings.</summary>
        /// <param name="field">Field in the class to configure.</param>
        /// <param name="opt">Serialization options to set on the field.</param>
        public CsvConfiguration<TTargetType> ForMember(Expression<Func<TTargetType, object>> field, Action<CsvFieldConfiguration> opt)
        {
            var member = ReflectionHelper.FindProperty(field);
            var foundField = GetField(member);

            CsvField textField;
            if (foundField == null)
            {
                textField = new CsvField
                {
                    Member = member,
                    Name = member.Name
                };
            }
            else
            {
                textField = (CsvField)foundField;
                Serializer.Fields.Remove(foundField);
            }

            CsvFieldConfiguration fieldConfig = new CsvFieldConfiguration(textField);
            opt.Invoke(fieldConfig);
            CsvSerializer.Fields.Add(fieldConfig.Field);
            return this;
        }

        /// <summary>Marks a field as not serializable.</summary>
        /// <param name="field">Field to ignore.</param>
        public CsvConfiguration<TTargetType> Ignore(Expression<Func<TTargetType, object>> field)
        {
            InternalIgnore(field);
            return this;
        }
    }
}
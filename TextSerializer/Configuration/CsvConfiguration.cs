using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using TheCodingMonkey.Serialization.Utilities;

namespace TheCodingMonkey.Serialization.Configuration
{
    public class CsvConfiguration<TTargetType> : TextConfiguration<TTargetType>
        where TTargetType : new()
    {
        private CsvSerializer<TTargetType> CsvSerializer { get; set; }

        public CsvConfiguration(CsvSerializer<TTargetType> serializer)
        : base(serializer)
        {
            CsvSerializer = serializer;

            AlwaysWriteQualifier(true);
            Delimiter(',');
            Qualifier('"');
        }

        public CsvConfiguration<TTargetType> ByConvention()
        {
            MemberInfo[] members = Serializer.TargetType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty);
            int position = 0;
            foreach (MemberInfo member in members)
            {
                Type memberType = null;
                if (member is FieldInfo)
                    memberType = ((FieldInfo)member).FieldType;
                else if (member is PropertyInfo)
                    memberType = ((PropertyInfo)member).PropertyType;

                if (memberType != null)
                {
                    TextFieldAttribute textField = new TextFieldAttribute
                    {
                        Name = member.Name,
                        Member = member,
                        Position = position
                    };

                    Serializer.Fields.Add(position++, textField);
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

        public CsvConfiguration<TTargetType> ForMember(Expression<Func<TTargetType, object>> field, Action<TextFieldConfiguration> opt)
        {
            var member = ReflectionHelper.FindProperty(field);
            var kvp = GetAttributePair(member);

            TextFieldAttribute attr;
            if (kvp == null)
            {
                attr = new TextFieldAttribute
                {
                    Member = member,
                    Name = member.Name
                };
            }
            else
            {
                attr = kvp.Value.Value;
                CsvSerializer.Fields.Remove(kvp.Value.Key);
            }

            TextFieldConfiguration fieldConfig = new TextFieldConfiguration(attr);
            opt.Invoke(fieldConfig);
            CsvSerializer.Fields.Add(attr.Position, attr);
            return this;
        }

        public CsvConfiguration<TTargetType> Ignore(Expression<Func<TTargetType, object>> field)
        {
            var member = ReflectionHelper.FindProperty(field);
            var kvp = GetAttributePair(member);
            if (kvp != null)
                Serializer.Fields.Remove(kvp.Value.Key);

            return this;
        }

        private KeyValuePair<int, TextFieldAttribute>? GetAttributePair(MemberInfo member)
        {
            KeyValuePair<int, TextFieldAttribute>? foundKvp = null;
            foreach (var kvp in CsvSerializer.Fields)
            {
                if (kvp.Value.Member.Name == member.Name)
                {
                    foundKvp = kvp;
                    break;
                }
            }
            return foundKvp;
        }
    }
}
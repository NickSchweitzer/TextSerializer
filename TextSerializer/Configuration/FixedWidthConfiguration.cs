using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TheCodingMonkey.Serialization.Utilities;

namespace TheCodingMonkey.Serialization.Configuration
{
    /// <summary>Fluent Configuration class for the <see cref="FixedWidthSerializer{TTargetType}">FixedWidthSerializer</see> class.</summary>
    public class FixedWidthConfiguration<TTargetType> : TextConfiguration<TTargetType>
        where TTargetType : new()
    {
        internal FixedWidthConfiguration(FixedWidthSerializer<TTargetType> serializer)
        : base(serializer)
        { }

        /// <summary>Sets the serialization properties of a member of the class. If ByConvention was called first, this will override those inferred settings.</summary>
        /// <param name="field">Field in the class to configure.</param>
        /// <param name="opt">Serialization options to set on the field.</param>
        public FixedWidthConfiguration<TTargetType> ForMember(Expression<Func<TTargetType, object>> field, Action<FixedWidthFieldConfiguration> opt)
        {
            var member = ReflectionHelper.FindProperty(field);
            var kvp = GetFieldPair(member);

            FixedWidthField textField;
            if (kvp == null)
            {
                textField = new FixedWidthField
                {
                    Member = member,
                };
            }
            else
            {
                textField = (FixedWidthField)kvp.Value.Value;
                Serializer.Fields.Remove(kvp.Value.Key);
            }

            FixedWidthFieldConfiguration fieldConfig = new FixedWidthFieldConfiguration(textField);
            opt.Invoke(fieldConfig);
            Serializer.Fields.Add(fieldConfig.Field.Position, fieldConfig.Field);
            return this;
        }

        /// <summary>Marks a field as not serializable.</summary>
        /// <param name="field">Field to ignore.</param>
        public FixedWidthConfiguration<TTargetType> Ignore(Expression<Func<TTargetType, object>> field)
        {
            InternalIgnore(field);
            return this;
        }
    }
}
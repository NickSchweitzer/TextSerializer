﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using TheCodingMonkey.Serialization.Utilities;

namespace TheCodingMonkey.Serialization.Configuration
{
    /// <summary>Base class for Fluent Configuration classes</summary>
    public abstract class TextConfiguration<TTargetType>
        where TTargetType : new()
    {
        /// <summary>Serializer which is being configured by this class.</summary>
        protected BaseSerializer<TTargetType> Serializer { get; set; }
        
        /// <summary>Constructs a Configuration object using the given serializer</summary>
        /// <param name="serializer">TextSerializer which is being configured by this object.</param>
        protected TextConfiguration(BaseSerializer<TTargetType> serializer)
        {
            Serializer = serializer;
        }

        /// <summary>Marks a field as not serializable.</summary>
        /// <param name="field">Field to ignore.</param>
        protected void InternalIgnore(Expression<Func<TTargetType, object>> field)
        {
            var member = ReflectionHelper.FindProperty(field);
            var foundField = GetField(member);
            if (foundField != null)
                Serializer.Fields.Remove(foundField);
        }

        /// <summary>Retrieves the Field configuration for the specified member, along with the position in the file where it should be serialized.</summary>
        /// <param name="member">The Reflection MemberInfo for the field to find in this configuration</param>
        /// <returns>A KeyValuePair containing the configured member if its already been configured, otherwise null.</returns>
        protected Field GetField(MemberInfo member)
        {
            return Serializer.Fields.Where(f => f.Member.Name == member.Name).FirstOrDefault();
        }
    }
}
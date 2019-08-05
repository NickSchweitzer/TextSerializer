using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using TheCodingMonkey.Serialization.Configuration;

namespace TheCodingMonkey.Serialization.Utilities
{
    internal static class ReflectionHelper
    {
        public static bool IsUserDefinedStruct(this Type type)
        {
            return type.IsValueType && !type.IsPrimitive && !type.IsEnum;
        }

        public static bool IsUserDefinedClass(this Type type)
        {
            return type.IsClass && type.FullName != "System.String";
        }

        public static MemberInfo FindProperty(LambdaExpression lambdaExpression)
        {
            Expression expressionToCheck = lambdaExpression;
            var done = false;

            while (!done)
            {
                switch (expressionToCheck.NodeType)
                {
                    case ExpressionType.Convert:
                        expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;
                        break;
                    case ExpressionType.Lambda:
                        expressionToCheck = ((LambdaExpression)expressionToCheck).Body;
                        break;
                    case ExpressionType.MemberAccess:
                        var memberExpression = ((MemberExpression)expressionToCheck);

                        if (memberExpression.Expression.NodeType != ExpressionType.Parameter &&
                            memberExpression.Expression.NodeType != ExpressionType.Convert)
                        {
                            throw new ArgumentException(
                                $"Expression '{lambdaExpression}' must resolve to top-level member and not any child object's properties.",
                                nameof(lambdaExpression));
                        }

                        var member = memberExpression.Member;

                        return member;
                    default:
                        done = true;
                        break;
                }
            }

            throw new TextSerializationConfigurationException(
                "Custom configuration for members is only supported for top-level individual members on a type.");
        }

        /// <summary>Helper method which uses reflection to assign a value to a property of a class.</summary>
        /// <param name="obj">Class object which contains the property receiving the value</param>
        /// <param name="val">Value which is bieng assigned</param>
        /// <param name="member">MemberInfo of the Property or Field being assigned to.</param>
        private static void AssignToClass(object obj, object val, MemberInfo member)
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
        private static void AssignToStruct(ValueType obj, object val, MemberInfo member)
        {
            if (member is PropertyInfo)
                ((PropertyInfo)member).SetValue(obj, val, null);
            else if (member is FieldInfo)
                ((FieldInfo)member).SetValue(obj, val);
            else
                throw new TextSerializationException("Invalid MemberInfo type encountered");
        }

        /// <summary>Helper method which uses reflection to assign a value to a property of a structure or class.</summary>
        /// <param name="parentObj">Object which contains the property receiving the value</param>
        /// <param name="parentStruct">ValueType version of object which contains the property receiving the value if a struct</param>
        /// <param name="fieldObj">Object to assign to the property</param>
        /// <param name="field">Field definition which contains the member to assign to</param>
        public static void AssignValue(object parentObj, ValueType parentStruct, object fieldObj, Field field)
        {
            if (parentObj.GetType().IsValueType)
                AssignToStruct(parentStruct, fieldObj, field.Member);
            else
                AssignToClass(parentObj, fieldObj, field.Member);
        }

        public static object GetPropertyFieldValue(MemberInfo member, object obj, ValueType valueStruct)
        {
            object returnObj;
            // Get the object from the field
            if (member is PropertyInfo)
                returnObj = ((PropertyInfo)member).GetValue(obj.GetType().IsValueType && valueStruct != null ? valueStruct : obj, null);
            else if (member is FieldInfo)
                returnObj = ((FieldInfo)member).GetValue(obj.GetType().IsValueType && valueStruct != null ? valueStruct : obj);
            else
                throw new TextSerializationException("Invalid MemberInfo type encountered");

            return returnObj;
        }

        public static object GetPropertyFieldValue(Field field, object obj, ValueType valueStruct)
        {
            return GetPropertyFieldValue(field.Member, obj, valueStruct);
        }

        public static AttributeType GetCustomAttribute<AttributeType>(this Type type)
            where AttributeType : Attribute
        {
            object[] attrs = type.GetCustomAttributes(typeof(AttributeType), false);
            if (attrs.Length > 0)
                return (AttributeType)attrs[0];
            else
                return null;
        }

        public static bool IsList(this Type type)
        {
            if (type.GetInterface("System.Collections.IList") != null)
                return true;
            else if (type.Name == "System.Collections.IList")
                return true;
            // The given type actually is IList<>
            else if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))
                return true;
            else
            {
                foreach (var testInterface in type.GetInterfaces())
                    if (testInterface.IsGenericType && testInterface.GetGenericTypeDefinition() == typeof(IList<>))
                        return true;
            }

            return false;
        }

        public static bool IsDictionary(this Type type)
        {
            if (type.GetInterface("System.Collections.IDictionary") != null)
                return true;
            else if (type.Name == "System.Collections.IDictionary")
                return true;
            // The given type actually is IDictionary<>
            else if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                return true;
            else
            {
                foreach (var testInterface in type.GetInterfaces())
                    if (testInterface.IsGenericType && testInterface.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                        return true;
            }

            return false;
        }
    }
}
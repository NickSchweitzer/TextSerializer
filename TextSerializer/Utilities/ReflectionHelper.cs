using System;
using System.Linq.Expressions;
using System.Reflection;

using TheCodingMonkey.Serialization.Configuration;

namespace TheCodingMonkey.Serialization.Utilities
{
    /// <summary>Static utility class which has common reflection code used throughout the library.</summary>
    public static class ReflectionHelper
    {
        /// <summary>Used by Fluent Configuration methods to return the Property or Attribute that a particular lambda expression is oeprating on.</summary>
        /// <param name="lambdaExpression">Lambda expression defined for a configuration of a serializer.</param>
        /// <returns>MemberInfo for the Property or Field being operated on</returns>
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
    }
}
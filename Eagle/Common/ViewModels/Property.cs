using System;
using System.Linq.Expressions;

namespace Eagle.Common.ViewModels
{
    public class Property
    {
        public static string Name<T>(Expression<Func<T, object>> property)
        {
            var expression = property.Body as MemberExpression;
            if (expression != null)
            {
                return expression.Member.Name;
            }

            var unaryExpression = property.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                return ((MemberExpression)unaryExpression.Operand).Member.Name;
            }

            throw new InvalidOperationException("Invalid property expression: " + property);
        }
    }
}

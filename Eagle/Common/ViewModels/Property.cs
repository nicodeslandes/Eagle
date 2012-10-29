using System;
using System.Linq.Expressions;

namespace Eagle.Common.ViewModel
{
    public class Property
    {
        public static string Name<T>(Expression<Func<T, object>> property)
        {
            return ((MemberExpression)property.Body).Member.Name;
        }
    }
}

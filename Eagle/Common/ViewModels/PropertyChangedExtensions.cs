using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;

namespace Eagle.Common.ViewModels
{
    public static class PropertyChangedExtensions
    {
        public static IObservable<TProperty> ObserveProperty<TSrc, TProperty>(this TSrc src, Expression<Func<TSrc, TProperty>> property) where TSrc : INotifyPropertyChanged
        {
            string propertyName;
            var expression = property.Body as MemberExpression;
            if (expression != null)
            {
                propertyName = expression.Member.Name;
            }
            else
            {
                var unaryExpression = property.Body as UnaryExpression;
                if (unaryExpression != null)
                {
                    propertyName = ((MemberExpression)unaryExpression.Operand).Member.Name;
                }
                else
                {
                    throw new InvalidOperationException("Invalid property expression: " + property);
                }
            }

            var getter = property.Compile();

            return Observable.Defer(() => Observable.Return(getter(src)))
                .Concat(
                Observable
                 .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(e => src.PropertyChanged += e, e => src.PropertyChanged -= e)
                 .Where(prop => prop.EventArgs.PropertyName == propertyName)
                 .Select(_ => getter(src)))
                 .DistinctUntilChanged();
        }

        public static IObservable<Unit> ObserveProperty(this INotifyPropertyChanged src, string propertyName)
        {
            return Observable
                 .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(e => src.PropertyChanged += e, e => src.PropertyChanged -= e)
                 .Where(prop => prop.EventArgs.PropertyName == propertyName)
                 .Select(_ => Unit.Default);
        }

        public static IObservable<TProperty> ObserveLatest<TSrc, TProperty>(this IObservable<TSrc> src, Expression<Func<TSrc, TProperty>> property) where TSrc : INotifyPropertyChanged
        {
            return src
                .Select(v => v == null ? Observable.Never<TProperty>() : v.ObserveProperty(property))
                .Switch();
        }
   }
}
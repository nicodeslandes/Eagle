using Caliburn.Micro;
using Eagle.Common.ViewModels;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Eagle.FilePicker.ViewModels
{
    public abstract class PropertyValue : PropertyChangedBase
    {
        public abstract string PropertyName { get; }
    }

    public class PropertyValue<T> : PropertyValue
    {
        private static readonly string ValueProperty = Property.Name<PropertyValue<T>>(vm => vm.Value);

        private readonly string _propertyName;
        private readonly Func<T> _propertyGetter;
        private readonly Action<T> _propertySetter;
        private readonly INotifyPropertyChanged _source;

        public PropertyValue(INotifyPropertyChanged source, PropertyInfo property)
        {
            _source = source;
            _propertyName = property.Name;

            var getterMethodInfo = property.GetGetMethod();
            var setterMethodInfo = property.GetSetMethod();
            _propertyGetter = () => (T)getterMethodInfo.Invoke(_source, null);
            _propertySetter = value => setterMethodInfo.Invoke(_source, new object[] { value });

            source.PropertyChanged += (s, ea) => this.NotifyOfPropertyChange(ValueProperty);
        }

        /// <summary>
        /// Design-Time constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public PropertyValue(string name, T value)
        {
            _propertyName = name;
            _propertyGetter = () => value;
        }

        public override string PropertyName
        {
            get { return _propertyName; }
        }

        public T Value
        {
            get { return _propertyGetter(); }
            set { _propertySetter(value); }
        }
    }
}

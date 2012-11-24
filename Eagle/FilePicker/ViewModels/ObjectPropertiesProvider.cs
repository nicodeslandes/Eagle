using System;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Reflection;

namespace Eagle.FilePicker.ViewModels
{
    [Export(typeof(IObjectPropertiesProvider))]
    public class ObjectPropertiesProvider : IObjectPropertiesProvider
    {
        public PropertyCollection GetProperties(INotifyPropertyChanged obj)
        {
            var visibleProperties =
                from p in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where p.IsDefined(typeof(UIVisibleAttribute))
                select p;

            var collection = new PropertyCollection();

            foreach (var item in visibleProperties)
            {
                var propertyValue = (PropertyValue)Activator.CreateInstance(
                    typeof(PropertyValue<>).MakeGenericType(item.PropertyType),
                    obj,
                    item);

                collection.Items.Add(propertyValue);
            }

            return collection;
        }
    }
}

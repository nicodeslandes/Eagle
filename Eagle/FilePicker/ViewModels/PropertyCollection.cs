using Caliburn.Micro;
using Eagle.Common.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace Eagle.FilePicker.ViewModels
{
    public class PropertyCollection : PropertyChangedBase
    {
        private static readonly string ItemsProperty = Property.Name<PropertyCollection>(vm => vm.Items);

        private ObservableCollection<PropertyValue> _items;

        public ObservableCollection<PropertyValue> Items
        {
            get { return _items; }

            set
            {
                _items = value;
                this.NotifyOfPropertyChange(ItemsProperty);
            }
        }
    }
}

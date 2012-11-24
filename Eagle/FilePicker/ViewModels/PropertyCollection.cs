using Caliburn.Micro;
using System;
using System.Collections.ObjectModel;

namespace Eagle.FilePicker.ViewModels
{
    public class PropertyCollection : PropertyChangedBase
    {
        /// <summary>
        /// Initializes a new instance of the PropertyCollection class.
        /// </summary>
        public PropertyCollection()
        {
            this.Items = new ObservableCollection<PropertyValue>();
        }

        public ObservableCollection<PropertyValue> Items {get; private set; }
    }
}

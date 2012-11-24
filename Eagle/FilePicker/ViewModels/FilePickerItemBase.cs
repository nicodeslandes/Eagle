using Caliburn.Micro;
using System;
using Eagle.Common.ViewModels;

namespace Eagle.FilePicker.ViewModels
{
    using System.Collections.ObjectModel;

    public class FilePickerItemBase : PropertyChangedBase, IFilePickerItem
    {
        private static readonly string NameProperty = Property.Name<FilePickerItemBase>(vm => vm.Name);

        public virtual void Invoke()
        {
        }

        public FilePickerItemBase(string name = null)
        {
            _name = name;
            this.ChildItems = new ObservableCollection<IFilePickerItem>();
        }

        private string _name;

        public string Name
        {
            get { return _name; }

            protected set
            {
                _name = value;
                this.NotifyOfPropertyChange(NameProperty);
            }
        }

        public ObservableCollection<IFilePickerItem> ChildItems { get; private set; }
    }
}

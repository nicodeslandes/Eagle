using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Eagle.FilePicker.ViewModels
{
    public interface IFilePickerItem : INotifyPropertyChanged
    {
        string Name { get; }

        ObservableCollection<IFilePickerItem> ChildItems { get; }

        void Invoke();
    }
}

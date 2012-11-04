using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Eagle.FilePicker.ViewModels
{
    public interface IFilePickerItem : INotifyPropertyChanged
    {
        string Name { get; set; }

        ObservableCollection<IFilePickerItem> ChildItems { get; }
    }
}

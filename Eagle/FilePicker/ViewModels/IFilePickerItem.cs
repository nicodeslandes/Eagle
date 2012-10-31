namespace Eagle.FilePicker.ViewModels
{
    using System.Collections.ObjectModel;

    public interface IFilePickerItem
    {
        string Name { get; set; }

        ObservableCollection<IFilePickerItem> ChildItems { get; }
    }
}

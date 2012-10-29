using System.Collections.ObjectModel;
using System;

namespace Eagle.FilePicker.ViewModels
{
    public class FolderViewModel : FilePickerItemBase
    {
        public FolderViewModel(string name)
            : base(name)
        {
            this.ChildItems = new ObservableCollection<IFilePickerItem>();           
        }

        public FolderViewModel()
            : this(null)
        {
        }

        public ObservableCollection<IFilePickerItem> ChildItems { get; private set; }
    }
}

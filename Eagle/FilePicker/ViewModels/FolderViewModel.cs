using System.Collections.ObjectModel;
using System;

namespace Eagle.FilePicker.ViewModels
{
    public class FolderViewModel : FilePickerItemBase
    {
        public FolderViewModel(string name)
            : base(name)
        {
        }

        public FolderViewModel()
            : this(null)
        {
        }
    }
}

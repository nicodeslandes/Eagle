using System.Collections.ObjectModel;
using System;
using Caliburn.Micro;

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
            if (Execute.InDesignMode)
            {
                this.Name = "Custom Folder";
            }
        }
    }
}

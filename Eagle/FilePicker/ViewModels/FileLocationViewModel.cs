using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Eagle.Common.ViewModel;

namespace Eagle.FilePicker.ViewModels
{
    public class FileLocationViewModel : FilePickerItemBase
    {
#if DEBUG
        public FileLocationViewModel() : this(@"\\Server\Path\Folder\File.log")
        {
        }

#endif
        public FileLocationViewModel(string name) : base(name)
        {
            this.SubLocations = new ObservableCollection<FileLocationViewModel>();
            this.ContextMenuItems = new ObservableCollection<MenuItemViewModel>()
            {
                new MenuItemViewModel("Add",
                    new MenuItemViewModel("Add Child", new DelegateCommand(this.AddChild))),
                new MenuItemViewModel("Edit Item",
                    new MenuItemViewModel("Edit Item", new DelegateCommand(this.AddChild)),
                    new MenuItemViewModel("Delete Item M W", new DelegateCommand(this.AddChild)),
                    new MenuItemViewModel("Sub Items", 
                        new MenuItemViewModel("Test1", new DelegateCommand(this.AddChild)),
                        new MenuItemViewModel("Test1", new DelegateCommand(this.AddChild)),
                        new MenuItemViewModel("Test1", new DelegateCommand(this.AddChild)),
                        new MenuItemViewModel("Test1", new DelegateCommand(this.AddChild)))),
                new MenuItemViewModel("Delete Item M W", new DelegateCommand(this.AddChild)),
                new MenuItemViewModel("Test1",
                    new MenuItemViewModel("Edit Item", new DelegateCommand(this.AddChild)),
                    new MenuItemViewModel("Delete Item M W", new DelegateCommand(this.AddChild)),
                    new MenuItemViewModel("Sub Items", 
                        new MenuItemViewModel("Test1", new DelegateCommand(this.AddChild)),
                        new MenuItemViewModel("Test1", new DelegateCommand(this.AddChild)),
                        new MenuItemViewModel("Test1", new DelegateCommand(this.AddChild)),
                        new MenuItemViewModel("Test1", new DelegateCommand(this.AddChild))))
            };
        }

        public ObservableCollection<FileLocationViewModel> SubLocations { get; private set; }

        public ObservableCollection<MenuItemViewModel> ContextMenuItems { get; private set; }

        private void AddChild()
        {
            System.Windows.MessageBox.Show("Hello from " + this.Name);
        }
    }
}

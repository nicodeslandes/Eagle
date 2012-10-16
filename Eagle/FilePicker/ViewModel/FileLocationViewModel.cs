using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;

namespace Eagle.FilePicker.ViewModel
{
    public class FileLocationViewModel : ViewModelBase
    {
        public FileLocationViewModel(string name)
        {
            this.Name = name;
            this.SubLocations = new ObservableCollection<FileLocationViewModel>();
            this.ContextMenuItems = new ObservableCollection<MenuItemViewModel>()
            {
                new MenuItemViewModel("Add",
                    new MenuItemViewModel("Add Child", new RelayCommand(this.AddChild))),
                new MenuItemViewModel("Edit Item",
                    new MenuItemViewModel("Edit Item", new RelayCommand(this.AddChild)),
                    new MenuItemViewModel("Delete Item", new RelayCommand(this.AddChild)),
                    new MenuItemViewModel("Sub Items", 
                        new MenuItemViewModel("Test1", new RelayCommand(this.AddChild)),
                        new MenuItemViewModel("Test1", new RelayCommand(this.AddChild)),
                        new MenuItemViewModel("Test1", new RelayCommand(this.AddChild)),
                        new MenuItemViewModel("Test1", new RelayCommand(this.AddChild)))),
                new MenuItemViewModel("Test1",
                    new MenuItemViewModel("Edit Item", new RelayCommand(this.AddChild)),
                    new MenuItemViewModel("Delete Item", new RelayCommand(this.AddChild)),
                    new MenuItemViewModel("Sub Items", 
                        new MenuItemViewModel("Test1", new RelayCommand(this.AddChild)),
                        new MenuItemViewModel("Test1", new RelayCommand(this.AddChild)),
                        new MenuItemViewModel("Test1", new RelayCommand(this.AddChild)),
                        new MenuItemViewModel("Test1", new RelayCommand(this.AddChild))))
            };
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }

            private set
            {
                if (_name != value)
                {
                    _name = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        public ObservableCollection<FileLocationViewModel> SubLocations { get; private set; }

        public ObservableCollection<MenuItemViewModel> ContextMenuItems { get; private set; }

        private void AddChild()
        {
            System.Windows.MessageBox.Show("Hello from " + this.Name);
        }
    }
}

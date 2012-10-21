using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Eagle.FilePicker.ViewModels
{
    public class FilePickerViewModel : PropertyChangedBase
    {
        /// <summary>
        /// Initializes a new instance of the FilePickerViewModel class.
        /// </summary>
        public FilePickerViewModel()
        {
            this.Locations = new ObservableCollection<FileLocationViewModel>();
            this.AddLocationCommand = new DelegateCommand(this.AddLocation);
            this.ContextMenuItems = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel("Add root location", this.AddLocationCommand)
            };

            if (Execute.InDesignMode)
            {
                this.Locations.Add(new FileLocationViewModel("Documents") { SubLocations = { new FileLocationViewModel("File1"), new FileLocationViewModel("File2"), new FileLocationViewModel("File3") } });
                this.Locations.Add(new FileLocationViewModel("Projects"));
                this.Locations.Add(new FileLocationViewModel("Logs"));
            }
        }

        public ObservableCollection<FileLocationViewModel> Locations { get; private set; }

        public ObservableCollection<MenuItemViewModel> ContextMenuItems { get; private set; }

        public ICommand AddLocationCommand { get; private set; }

        private void AddLocation()
        {
            MessageBox.Show("Hello");
        }
    }
}

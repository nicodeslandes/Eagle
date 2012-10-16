using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Eagle.FilePicker.ViewModel
{
    public class FilePickerViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the FilePickerViewModel class.
        /// </summary>
        public FilePickerViewModel()
        {
            this.Locations = new ObservableCollection<FileLocationViewModel>();
            this.AddLocationCommand = new RelayCommand(this.AddLocation);
            this.ContextMenuItems = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel("Add root location", this.AddLocationCommand)
            };
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

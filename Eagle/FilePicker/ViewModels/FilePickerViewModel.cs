using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Eagle.Common.ViewModel;

namespace Eagle.FilePicker.ViewModels
{
    public class FilePickerViewModel : PropertyChangedBase
    {
        /// <summary>
        /// Initializes a new instance of the FilePickerViewModel class.
        /// </summary>
        public FilePickerViewModel()
        {
            this.Items = new ObservableCollection<IFilePickerItem>();
            this.AddLocationCommand = new DelegateCommand(this.AddLocation);
            this.ContextMenuItems = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel("Add Folder", this.AddLocationCommand)
            };

            this.Items.Add(new RecentItemsFolderViewModel() { ChildItems = { new FileLocationViewModel(@"\\LONS00108577\DATA\Cortex\Test.log") } });

            if (Execute.InDesignMode || true)
            {
                this.Items.Add(new FolderViewModel("Documents") { ChildItems = { new FileLocationViewModel("File1"), new FileLocationViewModel("File2"), new FileLocationViewModel("File3") } });
                this.Items.Add(new FolderViewModel("Projects"));
                this.Items.Add(new FolderViewModel("Logs"));
            }

            new FilePickerItemBase().Name = "Hello";
        }

        public ObservableCollection<IFilePickerItem> Items { get; private set; }

        public ObservableCollection<MenuItemViewModel> ContextMenuItems { get; private set; }

        public ICommand AddLocationCommand { get; private set; }

        private void AddLocation()
        {
            MessageBox.Show("Hello");
        }
    }
}

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Eagle.Common.ViewModel;
using System.ComponentModel.Composition;

namespace Eagle.FilePicker.ViewModels
{
    [Export]
    public class FilePickerViewModel : PropertyChangedBase
    {
        /// <summary>
        /// Initializes a new instance of the FilePickerViewModel class.
        /// </summary>
        [ImportingConstructor]
        public FilePickerViewModel(IObjectPropertiesProvider objectPropertyProvider)
        {
            _objectPropertyProvider = objectPropertyProvider;

            this.Items = new ObservableCollection<IFilePickerItem>();
            this.AddLocationCommand = new DelegateCommand(this.AddLocation);
            this.ContextMenuItems = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel("Add Folder", this.AddLocationCommand)
            };

            this.Items.Add(new RecentItemsFolderViewModel() { ChildItems = { new FileLocationViewModel(@"\\LONS00108577\DATA\Cortex\Test.log") } });

            new FilePickerItemBase().Name = "Hello";
        }

        public FilePickerViewModel() : this(null)
        {
            if (Execute.InDesignMode)
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

        private readonly IObjectPropertiesProvider _objectPropertyProvider;
        private static readonly string SelectedItemProperty = Property.Name<FilePickerViewModel>(vm => vm.SelectedItem);

        private IFilePickerItem _selectedItem;

        public IFilePickerItem SelectedItem
        {
            get { return _selectedItem; }

            set
            {
                _selectedItem = value;
                this.NotifyOfPropertyChange(SelectedItemProperty);
            }
        }

        private void AddLocation()
        {
            MessageBox.Show("Hello");
        }
    }
}

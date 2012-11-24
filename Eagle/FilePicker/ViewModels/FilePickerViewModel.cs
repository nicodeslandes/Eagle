using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Eagle.Common.ViewModels;
using System.ComponentModel.Composition;
using System.Reactive.Linq;
using System;
using System.Reactive.Disposables;
using Eagle.ViewModels;

namespace Eagle.FilePicker.ViewModels
{
    [Export]
    public class FilePickerViewModel : PropertyChangedBase, IDisposable
    {
        public static readonly string ShowItemPropertiesProperty = Property.Name<FilePickerViewModel>(x => x.ShowItemProperties);
        private bool _showItemProperties;
        public static readonly string SelectedItemPropertiesProperty = Property.Name<FilePickerViewModel>(x => x.SelectedItemProperties);
        private static readonly string SelectedItemProperty = Property.Name<FilePickerViewModel>(vm => vm.SelectedItem);

        private PropertyCollection _selectedItemProperties;
        private readonly IObjectPropertiesProvider _objectPropertyProvider;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        /// <summary>
        /// Initializes a new instance of the FilePickerViewModel class.
        /// </summary>
        [ImportingConstructor]
        public FilePickerViewModel(IObjectPropertiesProvider objectPropertyProvider, IFileManager fileManager, RecentItemsFolderViewModel recentItemsFolderViewModel)
        {
            _objectPropertyProvider = objectPropertyProvider;
            this.Items = new ObservableCollection<IFilePickerItem>();
            this.AddLocationCommand = new DelegateCommand(this.AddLocation);
            this.ContextMenuItems = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel("Add Folder", this.AddLocationCommand)
            };

            if (recentItemsFolderViewModel != null)
                this.Items.Add(recentItemsFolderViewModel);

            _disposables.Add(this
                .ObserveProperty(v => v.SelectedItemProperties)
                .ObserveLatest(properties => properties.Items)
                .ObserveLatest(items => items.Count)
                .Subscribe(count => this.ShowItemProperties = count > 0));
        }

        public FilePickerViewModel()
            : this(null, null, null)
        {
            if (Execute.InDesignMode)
            {
                this.Items.Add(new FolderViewModel("Documents") { ChildItems = { new FileLocationViewModel("File1"), new FileLocationViewModel("File2"), new FileLocationViewModel("File3") } });
                this.Items.Add(new FolderViewModel("Projects"));
                this.Items.Add(new FolderViewModel("Logs"));

                this.SelectedItemProperties = new PropertyCollection
                {
                    Items =
                    {
                        new PropertyValue<string>("Name", "File Picker Item"),
                        new PropertyValue<int>("Size", 1235)
                    }
                };
            }
        }
            

        public ObservableCollection<IFilePickerItem> Items { get; private set; }

        public ObservableCollection<MenuItemViewModel> ContextMenuItems { get; private set; }

        public ICommand AddLocationCommand { get; private set; }

        private IFilePickerItem _selectedItem;

        public IFilePickerItem SelectedItem
        {
            get { return _selectedItem; }

            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    this.NotifyOfPropertyChange(SelectedItemProperty);
                    this.SelectedItemProperties = _objectPropertyProvider.GetProperties(value);
                }
            }
        }

        public PropertyCollection SelectedItemProperties
        {
            get { return _selectedItemProperties; }
            set
            {
                if (_selectedItemProperties != value)
                {
                    _selectedItemProperties = value;
                    this.NotifyOfPropertyChange(SelectedItemPropertiesProperty);
                }
            }
        }

        public bool ShowItemProperties
        {
            get { return _showItemProperties; }

            set
            {
                if (_showItemProperties != value)
                {
                    _showItemProperties = value;
                    this.NotifyOfPropertyChange(ShowItemPropertiesProperty);
                }
            }
        }

        public void InvokeItem(IFilePickerItem item)
        {
            item.Invoke();
        }

        private void AddLocation()
        {
            MessageBox.Show("Hello");
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}

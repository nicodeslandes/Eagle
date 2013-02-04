using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Eagle.Common.ViewModels;
using Eagle.ViewModels;

namespace Eagle.FilePicker.ViewModels
{
    public class FileLocationViewModel : FilePickerItemBase, IEquatable<FileLocationViewModel>
    {
        private readonly IFileManager _fileManager;
        public static readonly string PathProperty = Property.Name<FileLocationViewModel>(x => x.Path);

        private string _path;

#if DEBUG
        public FileLocationViewModel() : this(@"\\Server\Path\Folder\File.log")
        {
        }

#endif
        public FileLocationViewModel(string name, IFileManager fileManager = null)
            : base(name)
        {
            _fileManager = fileManager;
            _path = name;
            this.SubLocations = new ObservableCollection<FileLocationViewModel>();
            this.ContextMenuItems = new ObservableCollection<MenuItemViewModel>()
            {
                new MenuItemViewModel("Add",
                    new MenuItemViewModel("Add Child", this.AddChild)),
                new MenuItemViewModel("Edit Item",
                    new MenuItemViewModel("Edit Item", this.AddChild),
                    new MenuItemViewModel("Delete Item M W", this.AddChild),
                    new MenuItemViewModel("Sub Items", 
                        new MenuItemViewModel("Test1", this.AddChild, () => false),
                        new MenuItemViewModel("Test1", this.AddChild, () => false),
                        new MenuItemViewModel("Test1", this.AddChild),
                        new MenuItemViewModel("Test1", this.AddChild))),
                new MenuItemViewModel("Delete Item M W", this.AddChild),
                new MenuItemViewModel("Test1",
                    new MenuItemViewModel("Edit Item", this.AddChild),
                    new MenuItemViewModel("Delete Item M W", this.AddChild),
                    new MenuItemViewModel("Sub Items", 
                        new MenuItemViewModel("Test1", this.AddChild),
                        new MenuItemViewModel("Test1", this.AddChild),
                        new MenuItemViewModel("Test1", this.AddChild),
                        new MenuItemViewModel("Test1", this.AddChild)))
            };
        }

        public override void Invoke()
        {
            _fileManager.OpenFile(this.Path);
        }

        public ObservableCollection<FileLocationViewModel> SubLocations { get; private set; }

        public ObservableCollection<MenuItemViewModel> ContextMenuItems { get; private set; }

        [UIVisible]
        public string Path
        {
            get { return _path; }

            set
            {
                if (_path != value)
                {
                    _path = value;
                    this.NotifyOfPropertyChange(PathProperty);
                    this.Name = value;
                }
            }
        }
        

        private void AddChild()
        {
            System.Windows.MessageBox.Show("Hello from " + this.Name);
        }

        public bool Equals(FileLocationViewModel other)
        {
            return this.Path == other.Path;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;

            var other = obj as FileLocationViewModel;
            if (other == null) return false;
            
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return _path.GetHashCode();
        }
    }
}

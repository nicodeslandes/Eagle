using Caliburn.Micro;
using Eagle.FilePicker.ViewModels;
using Microsoft.Win32;
using System.ComponentModel.Composition;
using System.Text;
using System;

namespace Eagle.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    [Export(typeof(IShell))]
    public class ShellViewModel : Screen, IShell
    {
        private readonly IFileManager _fileManager;
        private bool _showLineNumbers = false;
        private bool _followTail;
        private bool _isFileOpen = false;
        private FileViewModel _file;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        [ImportingConstructor]
        public ShellViewModel(IFileManagerEventSource fileManagerEventSource, IFileManager fileManager)
        {
            fileManagerEventSource.OpenFileEventStream.Subscribe(this.OpenFile);
            _fileManager = fileManager;

            this.OpenCommand = new DelegateCommand(this.Open);
            this.ReloadCommand = new DelegateCommand(this.Reload, () => this.File != null);
            this.RefreshFileCommand = new DelegateCommand(this.RefreshFile, () => this.File != null);
            this.ClearCommand = new DelegateCommand(this.Clear, () => this.File != null);
            this.CloseCommand = new DelegateCommand(this.Close, () => this.File != null);

            this.DisplayName = "Eagle";

            this.FollowTail = true;

            if (Execute.InDesignMode)
            {
                this.IsFileOpen = true;
                this.File = new FileViewModel("Test File");
            }
            else
            {
                //this.FilePicker.Items.Add(new FileLocationViewModel("Documents") { SubLocations = { new FileLocationViewModel("File1"), new FileLocationViewModel("File2"), new FileLocationViewModel("File3") } });
                //this.FilePicker.Items.Add(new FileLocationViewModel("Projects"));
                //this.FilePicker.Items.Add(new FileLocationViewModel("Logs"));
            }
        }
        
        public bool IsFileOpen
        {
            get
            {
                return _isFileOpen;
            }
            set
            {
                if (_isFileOpen != value)
                {
                    _isFileOpen = value;
                    this.NotifyOfPropertyChange(() => this.IsFileOpen);
                }
            }
        }

        public FileViewModel File
        {
            get
            {
                return _file;
            }
            set
            {
                if (_file != value)
                {
                    _file = value;
                    this.NotifyOfPropertyChange(() => this.File);
                }
            }
        }

        [Import]
        public FilePickerViewModel FilePicker { get; private set; }

        public DelegateCommand OpenCommand { get; private set; }

        public DelegateCommand CloseCommand { get; private set; }

        public DelegateCommand RefreshFileCommand { get; private set; }

        public DelegateCommand ReloadCommand { get; private set; }

        public DelegateCommand ClearCommand { get; private set; }

        public bool FollowTail
        {
            get
            {
                return _followTail;
            }

            set
            {
                if (_followTail != value)
                {
                    _followTail = value;
                    this.NotifyOfPropertyChange(() => this.FollowTail);
                }
            }
        }

        public bool ShowLineNumbers
        {
            get
            {
                return _showLineNumbers;
            }

            set
            {
                if (_showLineNumbers != value)
                {
                    _showLineNumbers = value;
                    this.NotifyOfPropertyChange(() => this.ShowLineNumbers);
                }
            }
        }

        public void OpenFile(string fileName)
        {
            this.File = new FileViewModel(fileName, Encoding.UTF8);
            this.IsFileOpen = true;
            this.File.LoadFileContent();
        }

        private void Open()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                _fileManager.OpenFile(dialog.FileName);
            }
        }

        private void Close()
        {
            using (this.File)
            {
                this.File = null;
            }
        }

        private void RefreshFile()
        {
            if (this.File != null)
            {
                this.File.Refresh();
            }
        }

        private void Reload()
        {
            if (this.File != null)
            {
                this.File.LoadFileContent();
            }
        }

        private void Clear()
        {
            if (this.File != null)
            {
                this.File.ClearContent();
            }
        }
    }
}
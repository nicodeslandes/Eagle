using System;
using System.Collections;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Eagle.FilePicker.ViewModels;
using Eagle.Services;
using Microsoft.Win32;

namespace Eagle.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Screen, IShell
    {
        private readonly IFileManager _fileManager;
        private bool _showLineNumbers = false;
        private bool _followTail;
        private bool _isFileOpen = false;
        private FileViewModel _file;
        private readonly IStateService _stateService;
        private readonly IClipboardService _clipboardService;
        
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        [ImportingConstructor]
        public ShellViewModel(
            IFileManagerEventSource fileManagerEventSource,
            IFileManager fileManager,
            IStateService stateService,
            IClipboardService clipboardService)
        {
            _clipboardService = clipboardService;
            _stateService = stateService;
            _stateService.SavingEvent.Subscribe(this.SaveState);
            _fileManager = fileManager;
            fileManagerEventSource.OpenFileEventStream.Subscribe(this.OpenFile);

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

        private void SaveState(IStateCaptureContext context)
        {
            context.SaveState("Shell",
                new ShellState
                {
                    HasFile = _file != null,
                    OpenFile = (_file == null) ? null : _file.FileName
                });
        }

        public void CopySelectedLines(IEnumerable selection)
        {
            if (selection != null)
            {
                var lines = selection.Cast<LineViewModel>();
                if (lines.Any())
                {
                    var str = new StringBuilder();
                    foreach (var line in lines.OrderBy(l => l.LineNumber))
                    {
                        str.AppendLine(line.Text);
                    }

                    _clipboardService.CopyText(str.ToString());
                }
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

                    // Fire change notifications for depend properties
                    this.NotifyOfPropertyChange(() => this.CanCloseFile);
                    this.NotifyOfPropertyChange(() => this.CanRefreshFile);
                    this.NotifyOfPropertyChange(() => this.CanReload);
                    this.NotifyOfPropertyChange(() => this.CanClear);
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

        public void SaveState()
        {
            _stateService.MarkAsDirty();
        }

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

        public void Open()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                _fileManager.OpenFile(dialog.FileName);
            }
        }

        public void RefreshFile()
        {
            if (this.File != null)
            {
                this.File.RefreshFile();
            }
        }

        public bool CanRefreshFile { get { return this.IsFileOpen; } }

        public void CloseFile()
        {
            using (this.File)
            {
                this.File = null;
                this.IsFileOpen = false;
            }
        }

        public bool CanCloseFile { get { return this.IsFileOpen; } }

        public void Reload()
        {
            if (this.File != null)
            {
                this.File.LoadFileContent();
            }
        }

        public bool CanReload { get { return this.IsFileOpen; } }

        public void Clear()
        {
            if (this.File != null)
            {
                this.File.ClearContent();
            }
        }

        public bool CanClear { get { return this.IsFileOpen; } }

        public void RestoreState(ShellState state)
        {
            if (state.OpenFile != null)
            {
                _fileManager.OpenFile(state.OpenFile);
            }
            else
            {
                this.CloseFile();
            }
        }
    }
}
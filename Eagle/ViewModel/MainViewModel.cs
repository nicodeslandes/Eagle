using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Text;

namespace Eagle.ViewModel
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
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            this.OpenCommand = new RelayCommand(this.Open);
            this.CloseCommand = new RelayCommand(this.Close, () => this.File != null);
            this.RefreshCommand = new RelayCommand(this.Refresh, () => this.File != null);

            this.FollowTail = true;

            if (this.IsInDesignMode)
            {
                this.IsFileOpen = true;
                this.File = new FileViewModel("Test File");
            }
        }

        private bool _showLineNumbers = false;
        private bool _followTail;
        private bool _isFileOpen = false;
        private FileViewModel _file;

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
                    this.RaisePropertyChanged("IsFileOpen", !_isFileOpen, value, true);
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
                    this.RaisePropertyChanged("File");
                }
            }
        }

        public RelayCommand OpenCommand { get; private set; }

        public RelayCommand CloseCommand { get; private set; }

        public RelayCommand RefreshCommand { get; private set; }

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
                    var old = _followTail;
                    _followTail = value;
                    this.RaisePropertyChanged("FollowTail", old, value, true);
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
                    this.RaisePropertyChanged("ShowLineNumbers", !!value, value, true);
                }
            }
        }

        private void Open()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                this.File = new FileViewModel(dialog.FileName, Encoding.UTF8);
                this.IsFileOpen = true;
                this.File.LoadFileContent();
            }
        }

        private void Close()
        {
            using (this.File)
            {
                this.File = null;
            }
        }

        private void Refresh()
        {
            if (this.File != null)
            {
                this.File.LoadFileContent();
            }
        }
    }
}
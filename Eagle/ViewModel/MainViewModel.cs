using System.Collections.ObjectModel;
using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;

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
            this.Logs = new ObservableCollection<string>();

            this.OpenCommand = new RelayCommand(this.Open);
            this.CloseCommand = new RelayCommand(this.Close, () => this.FileName != null);
            this.RefreshCommand = new RelayCommand(this.Refresh, () => this.FileName != null);

            if (IsInDesignMode)
            {
                this.Logs.Add("Hello, World!");
                this.Logs.Add("Hello, World!");
                this.Logs.Add("Hello, World!");
                this.Logs.Add("Hello, World!");
                this.Logs.Add("Hello, World!");
                this.FollowTail = true;
                this.FileName = @"C:\Users\Nicolas\Desktop\Test.txt";
            }
            else
            {
                this.Logs.Add("Press Open to open a new file");
            }
        }

        public ObservableCollection<string> Logs { get; private set; }

        private string _fileName;
        private bool _followTail;
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

                    this.Logs.Add(string.Format("FollowTail changed to {0}", _followTail));
                }
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                if (_fileName != value)
                {
                    var old = _fileName;
                    _fileName = value;
                    this.RaisePropertyChanged("FileName", old, value, true);
                }
            }
        }
        public RelayCommand OpenCommand { get; private set; }

        public RelayCommand CloseCommand { get; private set; }

        public RelayCommand RefreshCommand { get; private set; }

        private void Open()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                this.FileName = dialog.FileName;
                LoadFileContent(this.FileName);
            }
        }

        private void Close()
        {
            this.Logs.Clear();
            this.FileName = null;
        }

        private void Refresh()
        {
            this.LoadFileContent(this.FileName);
        }

        private void LoadFileContent(string file)
        {
            this.Logs.Clear();
            try
            {
                foreach (var line in File.ReadAllLines(file))
                {
                    this.Logs.Add(line);
                }
            }
            catch (Exception ex)
            {
                this.Logs.Clear();
                this.Logs.Add(string.Format("Error opening file: {0}", ex));
            }
        }
    }
}
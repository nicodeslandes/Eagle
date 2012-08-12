using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using GalaSoft.MvvmLight;
using System.Windows;
using System.Diagnostics;
using System.Threading;

namespace Eagle.ViewModel
{
    public class FileViewModel : ViewModelBase
    {
        private string _fileName;
        private int _currentReadPosition = 0;
        private int _currentLineNumber = 1;
        private int _currentLineIndex = 0;
        private FileStream _fileStream;
        private ObservableCollection<LineViewModel> _lines = new ObservableCollection<LineViewModel>();
        private byte _previousReadLastByte = 0;
        private SynchronizationContext _syncContext;

        public ObservableCollection<LineViewModel> Lines
        {
            get
            {
                return _lines;
            }
            set
            {
                if (_lines != value)
                {
                    _lines = value;
                    this.RaisePropertyChanged("Lines");
                }
            }
        }

        public FileViewModel(string fileName)
        {
            this.Lines = new ObservableCollection<LineViewModel>();
            this.FileName = fileName;
            _syncContext = SynchronizationContext.Current;

            if (IsInDesignMode)
            {
                this.Lines.Add(new LineViewModel("Hello, World!"));
                this.Lines.Add(new LineViewModel("Hello, World!"));
                this.Lines.Add(new LineViewModel("Hello, World!"));
                this.Lines.Add(new LineViewModel("Hello, World!"));
                this.Lines.Add(new LineViewModel("Hello, World!"));
                this.FileName = @"C:\Users\Nicolas\Desktop\Test.txt";
            }
            else
            {
                this.Lines.Add(new LineViewModel("Press Open to open a new file"));

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

        public void LoadFileContent()
        {
            this.Lines.Clear();
            try
            {
                _fileStream = File.Open(_fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                _currentLineNumber = 1;
                _currentLineIndex = 0;
                _currentReadPosition = 0;
                _previousReadLastByte = 0;
                //var sw = Stopwatch.StartNew();
                var lines = new ObservableCollection<LineViewModel>(ReadLines());
                //MessageBox.Show(string.Format("{0} lines read in {1} ms", lines.Count, sw.ElapsedMilliseconds));
                this.Lines = lines;
            }
            catch (Exception ex)
            {
                this.Lines.Clear();
                this.Lines.Add(new LineViewModel(string.Format("Error opening file: {0}", ex)));
            }

            // Setup File change watcher
            var w = new FileSystemWatcher(Path.GetDirectoryName(_fileName), Path.GetFileName(_fileName))
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
            };
            w.Changed += OnFileChanged;
            w.EnableRaisingEvents = true;
        }

        private IEnumerable<LineViewModel> ReadLines()
        {
            var buffer = new byte[20480];
            while (true)
            {
                var read = _fileStream.Read(buffer, 0, buffer.Length);
                if (read == 0)
                    break;

                for (int i = 0; i < read; i++, _currentReadPosition++)
                {
                    if (buffer[i] == '\n')
                    {
                        int length = _currentReadPosition - _currentLineIndex;
                        if (i > 0 && buffer[i - 1] == '\r' || i == 0 && _previousReadLastByte == 'é')
                        {
                            length--;
                        }

                        var line = new LineViewModel(_currentLineNumber++, _currentLineIndex, length, _fileStream);
                        _currentLineIndex = _currentReadPosition + 1;
                        yield return line;
                    }

                }

                if (read > 0) _previousReadLastByte = buffer[read - 1];
            }
        }

        void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    break;
                case WatcherChangeTypes.Deleted:
                    break;
                case WatcherChangeTypes.Changed:
                    _fileStream.Seek(_currentReadPosition, SeekOrigin.Begin);
                    var lines = ReadLines().ToList();

                    _syncContext.Post(_ =>
                        {
                            foreach (var line in lines)
                            {
                                this.Lines.Add(line);
                            }
                        }, null);
                    break;
                case WatcherChangeTypes.Renamed:
                    break;
                case WatcherChangeTypes.All:
                    break;
            }
        }
    }
}

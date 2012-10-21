using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Text;
using System.Windows.Data;
using Eagle.Common.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Eagle.ViewModels
{
    public class FileViewModel : PropertyChangedBase, IDisposable
    {
        private LineViewModel _currentUnfinishedLine;
        private readonly Encoding _encoding = Encoding.Default;
        private string _fileName;
        private int _currentReadPosition = 0;
        private int _currentLineNumber = 1;
        private int _currentLineIndex = 0;
        private FileSystemWatcher _fileWatcher;
        private ObservableCollection<LineViewModel> _lines = new ObservableCollection<LineViewModel>();
        private byte _previousReadLastByte = 0;
        private readonly SynchronizationContext _syncContext;
        private readonly object _linesSync = new object();
        private readonly SingleTaskRunner _readNewLinesTaskRunner;

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
                    this.NotifyOfPropertyChange(() => this.Lines);
                }
            }
        }

        public FileViewModel(string fileName, Encoding encoding = null)
        {
            _encoding = encoding ?? Encoding.Default;

            this.Lines = new ObservableCollection<LineViewModel>();
            this.FileName = fileName;
            _syncContext = SynchronizationContext.Current;
            _readNewLinesTaskRunner = new SingleTaskRunner(ReadNewLinesFromFile);

            if (Execute.InDesignMode)
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
                    _fileName = value;
                    this.NotifyOfPropertyChange(() => this.FileName);
                }
            }
        }

        public async void LoadFileContent()
        {
            this.InitializeReadParameters();
            this.Lines.Clear();

            // Wait for any pending read
            await _readNewLinesTaskRunner.Stop();

            try
            {
                // Check we can open the file
                using (OpenFile()) { }

                this.Lines.Add(new LineViewModel("Loading..."));

                // Trigger a new read
                _readNewLinesTaskRunner.Start();
                _readNewLinesTaskRunner.TriggerExecution();
            }
            catch (Exception ex)
            {
                this.Lines.Clear();
                this.Lines.Add(new LineViewModel(string.Format("Error opening file: {0}", ex)));
            }

            // Setup File change watcher
            _fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(_fileName), Path.GetFileName(_fileName))
                                    {
                                        NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.Attributes
                                    };
            _fileWatcher.Changed += OnFileChanged;
            _fileWatcher.EnableRaisingEvents = true;
        }

        private void InitializeReadParameters()
        {
            _currentLineNumber = 1;
            _currentLineIndex = 0;
            _currentReadPosition = 0;
            _previousReadLastByte = 0;
            _currentUnfinishedLine = null;
        }

        private IEnumerable<LineViewModel> ReadLines(out bool fullReadPerformed)
        {
            var fileStream = OpenFile();
            if (fileStream == null)
            {
                this.InitializeReadParameters();
                fullReadPerformed = true;
                return Enumerable.Empty<LineViewModel>();
            }

            // Has the file shrunk? Or is this the first read?
            var diff = fileStream.Length - _currentReadPosition;
            if (diff < 0 || _currentReadPosition == 0)
            {
                // Yes it has; perform a new full read
                fullReadPerformed = true;
                this.InitializeReadParameters();
            }
            else
            {
                fullReadPerformed = false;
            }

            return this.DoReadLines(fileStream);
        }

        private FileStream OpenFile()
        {
            return File.Open(_fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
        }

        private IEnumerable<LineViewModel> DoReadLines(FileStream fileStream)
        {
            using (fileStream)
            {
                fileStream.Seek(_currentReadPosition, SeekOrigin.Begin);
                var buffer = new byte[20480];
                int read = 0;
                while (true)
                {
                    read = fileStream.Read(buffer, 0, buffer.Length);
                    if (read == 0)
                        break;

                    for (int i = 0; i < read; i++, _currentReadPosition++)
                    {
                        if (buffer[i] == '\n')
                        {
                            var lastLineCharacter = (i > 0) ? buffer[i - 1] : _previousReadLastByte;
                            LineViewModel newLine = ReadNewLine(lastLineCharacter, false);
                            if (newLine != null)
                                yield return newLine;
                        }

                    }

                    if (read > 0) _previousReadLastByte = buffer[read - 1];
                }

                // End-of-file reached; if the last character read wasn't '\n', add the unfinished line
                if (_previousReadLastByte != '\n' && _previousReadLastByte != '\0')
                {
                    var line = ReadNewLine(_previousReadLastByte, true);
                    if (line != null)
                        yield return line;
                }
            }
        }

        private LineViewModel ReadNewLine(byte lastLineCharacter, bool isLineUnfinished)
        {
            int length = _currentReadPosition - _currentLineIndex;
            if (lastLineCharacter == '\r')
            {
                length--;
            }

            // New line reached
            LineViewModel newLine = null;
            // Check if an unfinished line was being read
            if (_currentUnfinishedLine != null)
            {
                // Just update the current line with a new length
                _currentUnfinishedLine.Length = length;
            }
            else
            {
                newLine = new LineViewModel(_currentLineNumber++, _currentLineIndex, length, _fileName, _encoding);
            }

            if (isLineUnfinished)
            {
                _currentUnfinishedLine = newLine ?? _currentUnfinishedLine;
            }
            else
            {
                _currentLineIndex = _currentReadPosition + 1;
                _currentUnfinishedLine = null;
            }

            return newLine;
        }

        async void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    break;
                case WatcherChangeTypes.Deleted:
                    break;
                case WatcherChangeTypes.Changed:
                    await Task.Delay(TimeSpan.FromMilliseconds(50));
                    // Trigger a new read
                    _readNewLinesTaskRunner.TriggerExecution();
                    break;
                case WatcherChangeTypes.Renamed:
                    break;
                case WatcherChangeTypes.All:
                    break;
            }
        }

        private void ReadNewLinesFromFile()
        {
            bool fullReadPerformed;
            var lines = ReadLines(out fullReadPerformed).ToList();

            if (fullReadPerformed)
            {
                _syncContext.Post(_ =>
                    {
                        this.Lines = new ObservableCollection<LineViewModel>(lines);
                        BindingOperations.EnableCollectionSynchronization(this.Lines, this._linesSync);
                    }, null);
            }
            else
            {
                foreach (var line in lines)
                {
                    this.Lines.Add(line);
                }
            }
        }
        public void Dispose()
        {
            using (_fileWatcher)
            {
                _fileWatcher = null;
            }
        }

        public async void ClearContent()
        {
            // Wait for pending reads
            await _readNewLinesTaskRunner.Stop();
            this.Lines.Clear();
            _readNewLinesTaskRunner.Start();
            _readNewLinesTaskRunner.TriggerExecution();
        }

        public void RefreshFile()
        {
            _readNewLinesTaskRunner.TriggerExecution();
        }
    }
}

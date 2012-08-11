using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using GalaSoft.MvvmLight;

namespace Eagle.ViewModel
{
    public class FileViewModel : ViewModelBase
    {
        private string _fileName;
        private ObservableCollection<LineViewModel> _lines = new ObservableCollection<LineViewModel>();

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
                var stream = File.Open(_fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                var buffer = new byte[20480];
                var currentLineNumber = 1;
                var currentLineIndex = 0;
                var lines = new ObservableCollection<LineViewModel>();
                var position = 0;
                byte previousReadLastByte = 0;
                while (true)
                {
                    var read = stream.Read(buffer, 0, buffer.Length);
                    if (read == 0)
                        break;

                    for (int i = 0; i < read; i++, position++)
                    {
                        if (buffer[i] == '\n')
                        {
                            int length = position - currentLineIndex;
                            if (i > 0 && buffer[i-1] == '\r' || i == 0 && previousReadLastByte == '\r')
                            {
                                length--;
                            }

                            lines.Add(new LineViewModel(currentLineNumber++, currentLineIndex, length, stream));
                            currentLineIndex = position + 1;
                        }
                    }

                    if (read > 0) previousReadLastByte= buffer[read - 1];
                }

                this.Lines = lines;
            }
            catch (Exception ex)
            {
                this.Lines.Clear();
                this.Lines.Add(new LineViewModel(string.Format("Error opening file: {0}", ex)));
            }
        }
    }
}

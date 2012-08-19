using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;
using GalaSoft.MvvmLight;

namespace Eagle.ViewModel
{
    public class LineViewModel : ViewModelBase
    {
        private Encoding _encoding;
        private string _text;
        private readonly string _path;
        private readonly int _startIndex;
        private int _length;

        public string Text
        {
            get
            {
                if (_text == null)
                {
                    using (var stream = File.Open(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                    {
                        if (stream == null)
                        {
                            return null;
                        }

                        stream.Seek(_startIndex, SeekOrigin.Begin);
                        var buffer = new byte[_length];
                        int read = stream.Read(buffer, 0, _length);
                        _text = _encoding.GetString(buffer, 0, read);
                    }
                }
                return _text;
            }
        }

        public int LineNumber { get; private set; }

        public int Length
        {
            get { return _length; }
            set
            {
                if (_length != value)
                {
                    _length = value;
                    RefreshText();
                }
            }
        }

        public LineViewModel(string text)
        {
            _encoding = Encoding.Default;
            this._text = text;
        }

        public LineViewModel(int lineNumber, int startIndex, int length, string path, Encoding encoding)
        {
            _encoding = encoding;
            LineNumber = lineNumber;
            _startIndex = startIndex;
            _length = length;
            _path = path;
        }

        private void RefreshText()
        {
            _text = null;
            this.RaisePropertyChanged("Text");
        }

        public override string ToString()
        {
            return string.Format("{0} (from {1}, length: {2})", this.LineNumber, _startIndex, _length);
        }
    }
}

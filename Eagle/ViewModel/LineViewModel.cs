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
        private string _text;
        private readonly Stream _stream;
        private readonly int _startIndex;
        private readonly int _length;
        public string Text
        {
            get
            {
                if (_text == null)
                {
                    _stream.Seek(_startIndex, SeekOrigin.Begin);
                    var buffer = new byte[_length];
                    _stream.Read(buffer, 0, _length);
                    _text = Encoding.UTF8.GetString(buffer);
                }
                return _text;
            }
        }
        public int LineNumber { get; private set; }

        public LineViewModel(string text)
        {
            this._text = text;
        }

        public LineViewModel(int lineNumber, int startIndex, int length, Stream stream)
        {
            LineNumber = lineNumber;
            _startIndex = startIndex;
            _length = length;
            _stream = stream;
        }

        public override string ToString()
        {
            return string.Format("{0} (from {1}, length: {2})", this.LineNumber, _startIndex, _length);
        }
    }
}

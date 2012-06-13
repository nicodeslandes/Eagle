using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Eagle.ViewModel
{
    public class FileViewModel : ViewModelBase
    {
        private string _fileName;

        public ObservableCollection<string> Lines { get; private set; }

        public FileViewModel(string fileName)
        {
            this.Lines = new ObservableCollection<string>();
            this.FileName = fileName;

            if (IsInDesignMode)
            {
                this.Lines.Add("Hello, World!");
                this.Lines.Add("Hello, World!");
                this.Lines.Add("Hello, World!");
                this.Lines.Add("Hello, World!");
                this.Lines.Add("Hello, World!");
                this.FileName = @"C:\Users\Nicolas\Desktop\Test.txt";
            }
            else
            {
                this.Lines.Add("Press Open to open a new file");

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
                foreach (var line in File.ReadAllLines(this.FileName))
                {
                    this.Lines.Add(line);
                }
            }
            catch (Exception ex)
            {
                this.Lines.Clear();
                this.Lines.Add(string.Format("Error opening file: {0}", ex));
            }
        }
    }
}

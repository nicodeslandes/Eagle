using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace Eagle.FilePicker.Views
{
    /// <summary>
    /// Interaction logic for FilePickerView.xaml
    /// </summary>
    public partial class FilePickerView : UserControl
    {
        public FilePickerView()
        {
            InitializeComponent();
        }

        private void Items_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
    }
}

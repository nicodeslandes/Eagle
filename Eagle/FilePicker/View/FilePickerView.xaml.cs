using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;
using System.Xml;

namespace Eagle.FilePicker.View
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

        private void TreeView_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            // We want our xaml of be properly indented, ohterwise
            // we would not be able to indent them.
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            MenuItem m;

            // Make the string builder
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb, xmlSettings);
            System.Windows.Markup.XamlWriter.Save(App.Current.FindResource(new ComponentResourceKey(typeof(MenuItem), "SubmenuHeaderTemplateKey")), writer);
            MessageBox.Show(sb.ToString());
            //this.DataContext = sb.ToString();
            Clipboard.SetText(sb.ToString());
        }
    }
}

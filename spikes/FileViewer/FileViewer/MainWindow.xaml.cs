using System.Windows;

namespace FileViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddTextButton_Click(object sender, RoutedEventArgs e)
        {
            FileViewControl.AddText("Hello!");
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            FileViewControl.Random();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Eagle
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the App class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            var b = this.Resources["bootstrapper"];
        }
    }
}

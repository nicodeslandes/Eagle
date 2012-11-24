using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            Debug.WriteLine("Calling App.InitializeComponents");
            var start = Stopwatch.StartNew();
            this.InitializeComponent();
            Debug.WriteLine("App.InitializeComponent: {0} ms", start.ElapsedMilliseconds);
        }
    }
}

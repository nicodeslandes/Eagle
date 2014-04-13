using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace FileViewer
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:FileViewer"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:FileViewer;assembly=FileViewer"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:FileView/>
    ///
    /// </summary>
    public class FileView : Control
    {
        private AutoGeneratingPanel _linesPanel;
        private int _lineCount;

        static FileView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileView), new FrameworkPropertyMetadata(typeof(FileView)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var lineProvider = new MockLineProvider();
            _linesPanel = GetTemplateChild("LinesContainer") as AutoGeneratingPanel;
            if (_linesPanel != null)
            {
                _linesPanel.ItemGenerator = () =>
                {
                    var nextLine = lineProvider.GetNextLine();
                    return nextLine != null ? new LineElement { Text = nextLine } : null;
                };
            }
        }
        
        public void AddText(string text)
        {
            foreach (LineElement lineElement in _linesPanel.Children)
            {
                lineElement.Text += "1";
            }

            for (int i = 0; i < 20; i++)
            {
                _linesPanel.Children.Add(new LineElement { Text = text });
            }
        }

        public void Random()
        {
            _lineCount = _linesPanel.Children.Count;
            var rd = new Random();
            var bytes = new byte[150];
            var syncContext = SynchronizationContext.Current;

            Task.Run(async () =>
            {
                var total = Stopwatch.StartNew();
                var times = new List<double>();
                int count = 1000;
                var lines = new List<string>(_lineCount);
                for (int i = 0; i < _lineCount; i++)
                {
                    lines.Add(null);
                }

                TaskCompletionSource<object> tcs = null;
                for (int i = 0; i < count; i++)
                {
                    for (int j = 0; j < lines.Count; j++)
                    {
                        rd.NextBytes(bytes);
                        lines[j] = Convert.ToBase64String(bytes);
                    }

                    var totalSize = lines.Sum(l => l.Length);
                    if (tcs != null)
                        await tcs.Task;

                    var sw = Stopwatch.StartNew();

                    tcs = new TaskCompletionSource<object>();
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        times.Add(sw.ElapsedTicks);
                        for (int j = 0; j < lines.Count; j++)
                        {
                            ((LineElement) _linesPanel.Children[j]).Text = lines[j];
                        }
                        tcs.SetResult(null);
                    }), DispatcherPriority.Background);

                }

                if (tcs != null)
                    await tcs.Task;

                syncContext.Post(_ =>
                {
                    var message = string.Format("Total time: {0} s = {1} updates/second", total.Elapsed.TotalSeconds,
                        count / total.Elapsed.TotalSeconds);
                    MessageBox.Show(message);
                }, null);
                //MessageBox.Show(string.Join("\n", times));
            });
        }
    }
}

using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace FileViewer
{
    public class LineElement : FrameworkElement
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                InvalidateVisual();
            }
        }

        public LineElement()
        {
            Height = 12;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var formattedText = new FormattedText(Text, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                new Typeface("Courier New"), 12, Brushes.DarkBlue);
            drawingContext.DrawText(formattedText, new Point());
        }
    }
}
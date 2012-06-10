using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Eagle.Common.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool))
            {
                throw new InvalidOperationException("Boolean value expected");
            }

            return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is Visibility))
            {
                throw new InvalidOperationException("Visibility value expected");
            }

            return (Visibility)value == Visibility.Visible;
        }
    }
}

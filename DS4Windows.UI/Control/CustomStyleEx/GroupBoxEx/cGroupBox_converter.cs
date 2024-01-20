using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DS4WinWPF.UI.Control.CustomStyleEx.GroupBoxEx
{
    internal class cGroupBox_converter_header2string : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && str.Length > 0)
            {
                return new TextBlock()
                {
                    Text = str,
                    Foreground = new SolidColorBrush(Colors.Crimson)
                };
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

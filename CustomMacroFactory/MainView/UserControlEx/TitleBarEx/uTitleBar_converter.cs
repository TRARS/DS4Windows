using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomMacroFactory.MainView.UserControlEx.TitleBarEx
{
    public class uTitleBar_converter_bool2opacity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1.0 : 0.5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

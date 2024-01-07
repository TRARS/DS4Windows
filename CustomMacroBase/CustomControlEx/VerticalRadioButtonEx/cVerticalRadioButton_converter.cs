using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomMacroBase.CustomControlEx.VerticalRadioButtonEx
{
    internal class cVerticalRadioButton_converter_mix2colorfultextvisibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var EnableColorfulText = (bool)values[0];
            var IsChecked = (bool)values[1];

            return (EnableColorfulText && IsChecked) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

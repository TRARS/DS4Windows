using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomMacroBase.CustomControlEx.VerticalRadioButtonEx
{
    internal class cVerticalRadioButton_converter_doublenullcheck : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double.NaN)
            {
                return 0.0;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cVerticalRadioButton_converter_boolbool2visibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var ColorfulText = (bool)values[0];
            var IsChecked = (bool)values[1];

            return (ColorfulText && IsChecked) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

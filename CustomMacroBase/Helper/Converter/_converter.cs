using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomMacroBase.Helper.Converter
{
    class cSlider_converter_doublebool2double : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var slidervalue = (double)values[0];
            var enable = (bool)values[1];

            return enable ? slidervalue : double.NaN;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

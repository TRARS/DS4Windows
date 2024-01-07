using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomMacroBase.CustomControlEx.ColorfulTextBlockEx
{
    internal class cColorfulTextBlock_converter_doublenullcheck : IValueConverter
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
}

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CustomMacroBase.CustomControlEx.ValueIndicatorEx
{
    internal class cValueIndicator_converter_colorswitcher : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //var propValue = 0d;//propValue
                var propValueColor = (SolidColorBrush)values[1];//propValueColor
                var propValueColorSwitcher = (Func<double, SolidColorBrush>?)values[2];//propValueColorSwitcher.Invoke(x)

                if (double.TryParse($"{values[0]}", out var propValue) && propValueColorSwitcher is not null)
                {
                    return propValueColorSwitcher.Invoke(propValue);
                }
                if (bool.TryParse($"{values[0]}", out var flag) && propValueColorSwitcher is not null)
                {
                    return propValueColorSwitcher.Invoke(flag ? 1 : 0);
                }

                return propValueColor;
            }
            catch
            {
                return new SolidColorBrush(Colors.White);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

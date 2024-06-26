using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomMacroBase.CustomControlEx.ToggleButtonEx
{
    class cToggleButton_converter_radius2cornerradius : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var ActualHeight = (double)values[0];
            var CornerRadius = (CornerRadius)values[1];

            return new CornerRadius(Math.Min(CornerRadius.TopLeft + 0.5, ActualHeight / 2));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cToggleButton_converter_diameter2height : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var BorderThickness = (Thickness)values[0];
            var Diameter = (double)values[1];

            return Diameter + BorderThickness.Top + BorderThickness.Bottom;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cToggleButton_converter_diameter2width : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return cToggleButton_math.Instance.WidthCalculator((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cToggleButton_converter_diameterDiet : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var old = (double)values[0];
            var diet = (double)values[1];
            return Math.Max(old - diet * 2, 1d);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cToggleButton_converter_transformXcalculator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is double transformX)
            {
                var diet = (double)values[1];

                return Math.Max(transformX + diet, 0d);
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cToggleButton_converter_visibility2minwidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility v)
            {
                return v is Visibility.Visible ? 10 : 0;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

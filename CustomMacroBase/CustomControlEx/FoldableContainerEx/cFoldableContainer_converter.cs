using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomMacroBase.CustomControlEx.FoldableContainerEx
{
    class cFoldableContainer_converter_height2height : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height && height > 0)
            {
                return Math.Max(height - 2, 0);
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cFoldableContainer_converter_scale2visibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse($"{values[0]}", out var scaleX) || double.TryParse($"{values[1]}", out var scaleY))
            {
                return Binding.DoNothing;
            }

            return (scaleX + scaleY <= 0) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cFoldableContainer_converter_visibility2visibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is Visibility ArrowVisibility && values[1] is Visibility BodyVisibility)
            {
                if (ArrowVisibility is Visibility.Visible && BodyVisibility is Visibility.Visible)
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cFoldableContainer_converter_visibility2ishittestvisible : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is Visibility ArrowVisibility && values[1] is Visibility BodyVisibility)
            {
                if (ArrowVisibility is Visibility.Visible && BodyVisibility is Visibility.Visible)
                {
                    return true;
                }
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

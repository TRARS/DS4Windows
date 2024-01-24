using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomMacroBase.CustomControlEx.FoldableContainerEx
{
    class cFoldableContainer_converter_visibility2angle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility v)
            {
                return v == Visibility.Visible ? 90 : 0;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

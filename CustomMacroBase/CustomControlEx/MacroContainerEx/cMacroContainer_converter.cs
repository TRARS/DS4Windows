using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using TrarsUI.Shared.DTOs;

namespace CustomMacroBase.CustomControlEx.MacroContainerEx
{
    internal class cMacroContainer_converter_content2scrollbarvisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ToggleTreeViewNode) { return ScrollBarVisibility.Visible; }
            return ScrollBarVisibility.Disabled;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

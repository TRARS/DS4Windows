using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomMacroBase.CustomControlEx.ComboBoxEx
{
    class cComboBox_converter_content2string : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{value}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

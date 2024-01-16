using System;
using System.Globalization;
using System.Windows.Data;

namespace DS4WinWPF.UI.Control.CustomStyleEx.CheckBoxEx
{
    class cCheckBox_converter_content2string : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var result = ($"{value.GetType()}") switch
            //{
            //    _ => $"{value}"
            //};

            return $"{value}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

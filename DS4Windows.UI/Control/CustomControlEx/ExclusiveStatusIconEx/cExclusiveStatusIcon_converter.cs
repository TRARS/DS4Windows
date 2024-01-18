using System;
using System.Globalization;
using System.Windows.Data;

namespace DS4WinWPF.UI.Control.CustomControlEx.ExclusiveStatusIconEx
{
    internal class cExclusiveStatusIcon_converter_accesstype2bool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{value}".Contains("Resources/key-solid");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

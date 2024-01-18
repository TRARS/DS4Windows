using System;
using System.Globalization;
using System.Windows.Data;

namespace DS4WinWPF.UI.Control.CustomControlEx.BluetoothUSBStatusIconEx
{
    internal class cBluetoothUSBStatusIcon_converter_devicetype2bool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{value}".Contains("Resources/USB");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

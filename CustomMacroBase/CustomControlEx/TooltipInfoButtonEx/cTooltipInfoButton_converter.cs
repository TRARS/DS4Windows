using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace CustomMacroBase.CustomControlEx.TooltipInfoButtonEx
{
    internal class cTooltipInfoButton_converter_test : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Task.Run(() =>
            {
                System.Windows.MessageBox.Show($"value = {value}\n\nPlacementTarget = {((ContextMenu)value).PlacementTarget}");
            });

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

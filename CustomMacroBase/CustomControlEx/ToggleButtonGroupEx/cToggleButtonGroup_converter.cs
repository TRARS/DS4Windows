using CustomMacroBase.PreBase;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CustomMacroBase.CustomControlEx.ToggleButtonGroupEx
{
    class cToggleButtonGroup_converter_bool2visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class cToggleButtonGroup_converter_bool2opacity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1.0 : double.Parse($"{parameter}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cToggleButtonGroup_converter_boolbool2bool : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool parent_view_enable = (bool)values[0];
            bool parent_model_enable = (bool)values[1];

            return !(!parent_view_enable || !parent_model_enable);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class cToggleButtonGroup_converter_delegate2uielement : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is Func<UIElement> delegateFunc)
                {
                    return new ObservableCollection<UIElement>() { delegateFunc.Invoke() };
                }
            }
            catch (Exception ex)
            {
                Task.Run(() =>
                {
                    System.Windows.MessageBox.Show($"{ex.Message}");
                });
            }


            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cToggleButtonGroup_converter_string2bool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string.IsNullOrWhiteSpace((string)value)) ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cToggleButtonGroup_converter_count2expander : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<GateBase> list)
            {
                return list.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class cToggleButtonGroup_converter_count2arrowvisibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var count = 0;

            if (values[0] is ObservableCollection<GateNode> list)
            {
                count += list.Where(node => node.Type is GateNodeType.GateBase)
                             .Select(node => (GateBase)node.Content)
                             .ToList()
                             .Count(gate => !gate.HideSelf);

                //count += list.Where(node => node.Type is not GateNodeType.GateBase)
                //             .Count();
            }

            if ((double.TryParse($"{values[1]}", out var childrenActualHeight) && childrenActualHeight > 0))
            {
                count++;
            }

            return count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cToggleButtonGroup_converter_tooltip : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var pre = $"{values[0]}";
            var suf = $"{values[1]}";

            return $"{pre}" + (string.IsNullOrWhiteSpace(suf) ? string.Empty : $" {suf}");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using CustomMacroBase.PreBase;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomMacroBase.CustomControlEx.ContextMenuEx
{
    class cMenuItemForHotkey_converter_enum2string : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum)
            {
                return value.ToString() ?? "error"; // 将枚举值转换为字符串
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class cMenuItemForHotkey_converter_datacontext2hotkeyinfolist : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GateBase gb)
            {
                return new cMenuItemForHotkey_viewmodel(gb.Text, gb.Feature, gb.EnableReverse).HotkeyInfoList;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

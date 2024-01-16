﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace DS4WinWPF.UI.Control.CustomStyleEx.ComboBoxEx
{
    class cComboBox_converter_content2string : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = ($"{value.GetType()}") switch
            {
                "DS4WinWPF.DS4Forms.ViewModels.LangPackItem" => $"{((dynamic)value).NativeName}",
                "DS4WinWPF.DS4Forms.ViewModels.MonitorChoiceListing" => $"{((dynamic)value).DisplayItemString}",
                "DS4WinWPF.DS4Control.GamepadPreset" => $"{((dynamic)value).Name}",
                "DS4WinWPF.DS4Forms.ViewModels.Util.EnumChoiceSelection`1[DS4Windows.AutoProfileDisplayProfileSwitchChoices]" => $"{((dynamic)value).DisplayName}",
                "DS4WinWPF.DS4Forms.ViewModels.Util.EnumChoiceSelection`1[DS4WinWPF.DS4Control.PresetOption+OutputContChoice]" => $"{((dynamic)value).DisplayName}",
                "DS4WinWPF.DS4Forms.ViewModels.Util.EnumChoiceSelection`1[DS4Windows.Mouse+TouchButtonActivationMode]" => $"{((dynamic)value).DisplayName}",
                "DS4WinWPF.DS4Forms.ViewModels.Util.EnumChoiceSelection`1[DS4Windows.DS4TriggerOutputMode]" => $"{((dynamic)value).DisplayName}",
                "DS4WinWPF.DS4Forms.ViewModels.TwoStageChoice" => $"{((dynamic)value).DisplayName}",
                "DS4WinWPF.DS4Forms.ViewModels.TriggerEffectChoice" => $"{((dynamic)value).DisplayName}",
                "DS4WinWPF.ProfileEntity" => $"{((dynamic)value).Name}",
                _ => ((Func<string>)(() =>
                {
                    if (value is not string)
                    {
                        Debug.WriteLine(value);
                    }
                    return $"{value}";
                })).Invoke()
            };



            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

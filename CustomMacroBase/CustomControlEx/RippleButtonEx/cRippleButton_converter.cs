using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CustomMacroBase.CustomControlEx.RippleButtonEx
{
    internal class cRippleButton_converter_clickpos2xy : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var x = double.Parse($"{values[0]}");
                var wh = double.Parse($"{values[1]}");
                var result = x - wh / 2;
                return result;
            }
            catch
            {
                return Binding.DoNothing;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    internal class cRippleButton_converter_size2brush : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var width = double.Parse($"{values[0]}");
            var height = double.Parse($"{values[1]}");
            var delta = double.Parse($"{parameter}");

            // 创建一个线性渐变画刷
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();

            // 设置渐变的起始点和结束点
            linearGradientBrush.StartPoint = new(0.5, 0);
            linearGradientBrush.EndPoint = new(0.5, 1);

            // 创建一个 TransformGroup，并添加需要的变换
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform());
            transformGroup.Children.Add(new SkewTransform());
            transformGroup.Children.Add(new RotateTransform()
            {
                CenterX = width / 2,
                CenterY = height / 2
            });
            transformGroup.Children.Add(new TranslateTransform());

            // 将 TransformGroup 设置到 LinearGradientBrush 中
            linearGradientBrush.Transform = transformGroup;

            // 添加渐变色
            linearGradientBrush.GradientStops.Add(new GradientStop(Colors.Gray, 0 - delta));
            linearGradientBrush.GradientStops.Add(new GradientStop(Colors.Gray, 0.5));
            linearGradientBrush.GradientStops.Add(new GradientStop(Colors.Gray, 1 + delta));

            return linearGradientBrush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cRippleButton_converter_minradius : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var w = double.Parse($"{values[0]}");
                var h = double.Parse($"{values[1]}");
                var tk = (Thickness)values[2];
                var result = Math.Min(w - (tk.Left + tk.Right), h - (tk.Top + tk.Bottom));

                if (result is 0 || result is double.NaN || result < 0) { return Binding.DoNothing; }

                return result;//选 Max 会导致圆形被切一刀不好看所以取 Min，放大倍数取(长边*2/短边)
            }
            catch { return Binding.DoNothing; }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    internal class cRippleButton_converter_opacity2scale : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var op = (double)values[0];//1 to 0
            var w = (double)values[1];
            var h = (double)values[2];
            var r = Math.Min(w, h) / 2;
            var max = w + h;
            return (1 - op) * (max / r);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cRippleButton_converter_cornerradius2double : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((CornerRadius)value).TopLeft;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    internal class cRippleButton_converter_size2rect : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var w = (double)values[0];
                var h = (double)values[1];
                var tk = (Thickness)values[2];
                return new Rect(0 - tk.Left, 0 - tk.Top, w, h);
            }
            catch
            {
                return Binding.DoNothing;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }







    internal class cRippleButton_converter_thickness2margin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tk = (Thickness)value;
            return new Thickness(-tk.Left, -tk.Top, -tk.Right, -tk.Bottom);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    internal class cRippleButton_converter_anm2radius : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var w = double.Parse($"{values[0]}");
                var h = double.Parse($"{values[1]}");
                var tk = (Thickness)values[2];
                var op = double.Parse($"{values[3]}");
                var result = Math.Min(w - (tk.Left + tk.Right), h - (tk.Top + tk.Bottom));
                result = (w + h) * ((1 - op) / 1);

                if (result is 0 || result is double.NaN || result < 0) { return Binding.DoNothing; }

                return result;//选 Max 会导致圆形被切一刀不好看所以取 Min，放大倍数取(长边*2/短边)
            }
            catch { return Binding.DoNothing; }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

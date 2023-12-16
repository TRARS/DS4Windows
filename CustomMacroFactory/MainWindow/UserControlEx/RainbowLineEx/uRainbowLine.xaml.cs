using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomMacroFactory.MainWindow.UserControlEx.RainbowLineEx
{
    public partial class uRainbowLine : UserControl
    {
        private static LinearGradientBrush _gradient
        {
            get
            {
                return new Func<LinearGradientBrush>(() =>
                {
                    LinearGradientBrush result = new() { StartPoint = new(0, 0), EndPoint = new(1, 0) };
                    result.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#009fd9"), 1.000));
                    result.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#65b849"), 0.834));
                    result.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#f7b423"), 0.667));
                    result.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#f58122"), 0.500));
                    result.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#de3a3c"), 0.334));
                    result.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#943f96"), 0.137));
                    result.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#009fd9"), 0.000));
                    return result;
                })();
            }
        }

        public Brush BrushColor
        {
            get { return (Brush)GetValue(BrushColorProperty); }
            set { SetValue(BrushColorProperty, value); }
        }
        public static readonly DependencyProperty BrushColorProperty = DependencyProperty.Register(
            name: "BrushColor",
            propertyType: typeof(Brush),
            ownerType: typeof(uRainbowLine),
            typeMetadata: new FrameworkPropertyMetadata(_gradient, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public new double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        public static new readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            name: "Width",
            propertyType: typeof(double),
            ownerType: typeof(uRainbowLine),
            typeMetadata: new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public new double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }
        public static new readonly DependencyProperty HeightProperty = DependencyProperty.Register(
            name: "Height",
            propertyType: typeof(double),
            ownerType: typeof(uRainbowLine),
            typeMetadata: new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }

    public partial class uRainbowLine : UserControl
    {
        public uRainbowLine()
        {
            InitializeComponent();
            this.Height = 1;
        }
    }
}

using CustomMacroBase.Helper;
using CustomMacroFactory.MainView.Interfaces;
using System.Windows.Media;

namespace CustomMacroFactory.MainView.UserControlEx.RainbowLineEx
{
    public partial class uRainbowLine_viewmodel : NotificationObject, IViewModel
    {
        private readonly uRainbowLine_model model = new();

        public Brush BrushColor
        {
            get { return model.BrushColor; }
            set
            {
                model.BrushColor = value;
                NotifyPropertyChanged();
            }
        }
        public double Width
        {
            get { return model.Width; }
            set
            {
                model.Width = value;
                NotifyPropertyChanged();
            }
        }
        public double Height
        {
            get { return model.Height; }
            set
            {
                model.Height = value;
                NotifyPropertyChanged();
            }
        }
    }

    public partial class uRainbowLine_viewmodel
    {
        private readonly LinearGradientBrush defBrush = new LinearGradientBrush()
        {
            StartPoint = new(0, 0),
            EndPoint = new(1, 0),
            GradientStops = new()
            {
                new GradientStop((Color)ColorConverter.ConvertFromString("#009fd9"), 1.000),
                new GradientStop((Color)ColorConverter.ConvertFromString("#65b849"), 0.834),
                new GradientStop((Color)ColorConverter.ConvertFromString("#f7b423"), 0.667),
                new GradientStop((Color)ColorConverter.ConvertFromString("#f58122"), 0.500),
                new GradientStop((Color)ColorConverter.ConvertFromString("#de3a3c"), 0.334),
                new GradientStop((Color)ColorConverter.ConvertFromString("#943f96"), 0.137),
                new GradientStop((Color)ColorConverter.ConvertFromString("#009fd9"), 0.000),
            }
        };

        public uRainbowLine_viewmodel()
        {
            this.BrushColor = defBrush;
            this.Width = double.NaN;
            this.Height = 1;
        }
    }
}

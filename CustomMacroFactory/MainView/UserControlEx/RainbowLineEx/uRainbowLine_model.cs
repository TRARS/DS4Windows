using System.Windows.Media;

namespace CustomMacroFactory.MainView.UserControlEx.RainbowLineEx
{
    public class uRainbowLine_model
    {
        public Brush BrushColor { get; set; } = new SolidColorBrush(Colors.Yellow);
        public double Width { get; set; } = double.NaN;
        public double Height { get; set; } = double.NaN;
    }
}

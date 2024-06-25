using System.Windows;
using System.Windows.Media;

namespace CustomMacroFactory.MainView.UserControlEx.PixelPicker
{
    public class uPixelPicker_model
    {
        public Visibility Visibility { get; set; } = Visibility.Collapsed;
        public UIElement CurrentScreenshot { get; set; } = new();
        public UIElement AdditionalInfo { get; set; } = new();
        public UIElement Magnifier { get; set; } = new();

        public ImageSource? ImageSource = null;//BitmapImage即可
        public Thickness ImageMargin { get; set; } = new();
        public Thickness ImageMarginWhenLeftDown { get; set; } = new();
        public PixelDetail BGRA { get; set; } = new();

        public Rect ViewboxRect { get; set; } = new();

        public string SizeInfo { get; set; } = string.Empty;
        public string ClickPointInfo { get; set; } = string.Empty;
    }
}

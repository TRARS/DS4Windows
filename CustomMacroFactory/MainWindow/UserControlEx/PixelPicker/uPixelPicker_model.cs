using System.Windows;
using System.Windows.Media;

namespace CustomMacroFactory.MainWindow.UserControlEx.PixelPicker
{
    public class uPixelPicker_model
    {
        public class PixelDetail
        {
            public int Stride;
            public byte[]? Values;
        }

        public ImageSource? ImageSource = null;//BitmapImage即可
        public Thickness ImageMargin { get; set; } = new();
        public Thickness ImageMarginWhenLeftDown { get; set; } = new();
        public PixelDetail BGRA { get; set; } = new();

        public Visibility Visibility { get; set; } = Visibility.Collapsed;

        public Rect ViewboxRect { get; set; } = new();

        public string SizeInfo { get; set; } = string.Empty;
        public string ClickPointInfo { get; set; } = string.Empty;
    }
}

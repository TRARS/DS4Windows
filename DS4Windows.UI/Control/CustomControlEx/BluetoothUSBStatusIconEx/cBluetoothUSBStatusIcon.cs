using System.Windows;
using System.Windows.Media;

namespace DS4WinWPF.UI.Control.CustomControlEx.BluetoothUSBStatusIconEx
{
    public partial class cBluetoothUSBStatusIcon : System.Windows.Controls.Control
    {
        static cBluetoothUSBStatusIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cBluetoothUSBStatusIcon), new FrameworkPropertyMetadata(typeof(cBluetoothUSBStatusIcon)));
        }
    }

    public partial class cBluetoothUSBStatusIcon
    {
        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            name: "Source",
            propertyType: typeof(string),
            ownerType: typeof(cBluetoothUSBStatusIcon),
            typeMetadata: new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public SolidColorBrush PathColor
        {
            get { return (SolidColorBrush)GetValue(PathColorProperty); }
            set { SetValue(PathColorProperty, value); }
        }
        public static readonly DependencyProperty PathColorProperty = DependencyProperty.Register(
            name: "PathColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cBluetoothUSBStatusIcon),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("White")), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Thickness PathMargin
        {
            get { return (Thickness)GetValue(PathMarginProperty); }
            set { SetValue(PathMarginProperty, value); }
        }
        public static readonly DependencyProperty PathMarginProperty = DependencyProperty.Register(
            name: "PathMargin",
            propertyType: typeof(Thickness),
            ownerType: typeof(cBluetoothUSBStatusIcon),
            typeMetadata: new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

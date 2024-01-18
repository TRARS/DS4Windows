using System.Windows;
using System.Windows.Media;

namespace DS4WinWPF.UI.Control.CustomControlEx.ExclusiveStatusIconEx
{
    public partial class cExclusiveStatusIcon : System.Windows.Controls.Control
    {
        static cExclusiveStatusIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cExclusiveStatusIcon), new FrameworkPropertyMetadata(typeof(cExclusiveStatusIcon)));
        }
    }

    public partial class cExclusiveStatusIcon
    {
        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            name: "Source",
            propertyType: typeof(string),
            ownerType: typeof(cExclusiveStatusIcon),
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
            ownerType: typeof(cExclusiveStatusIcon),
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
            ownerType: typeof(cExclusiveStatusIcon),
            typeMetadata: new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

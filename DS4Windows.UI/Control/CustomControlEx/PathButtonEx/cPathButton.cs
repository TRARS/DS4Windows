using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DS4WinWPF.UI.Control.CustomControlEx.PathButtonEx
{
    public partial class cPathButton : Button
    {
        static cPathButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cPathButton), new FrameworkPropertyMetadata(typeof(cPathButton)));
        }
    }

    public partial class cPathButton
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: "Text",
            propertyType: typeof(string),
            ownerType: typeof(cPathButton),
            typeMetadata: new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public SolidColorBrush TextColor
        {
            get { return (SolidColorBrush)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
            name: "TextColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cPathButton),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.White), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public string PathData
        {
            get { return (string)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }
        public static readonly DependencyProperty PathDataProperty = DependencyProperty.Register(
            name: "PathData",
            propertyType: typeof(string),
            ownerType: typeof(cPathButton),
            typeMetadata: new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public SolidColorBrush PathColor
        {
            get { return (SolidColorBrush)GetValue(PathColorProperty); }
            set { SetValue(PathColorProperty, value); }
        }
        public static readonly DependencyProperty PathColorProperty = DependencyProperty.Register(
            name: "PathColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cPathButton),
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
            ownerType: typeof(cPathButton),
            typeMetadata: new FrameworkPropertyMetadata(new Thickness(4), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public SolidColorBrush PathBackgroundColor
        {
            get { return (SolidColorBrush)GetValue(PathBackgroundColorProperty); }
            set { SetValue(PathBackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty PathBackgroundColorProperty = DependencyProperty.Register(
            name: "PathBackgroundColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cPathButton),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#80808080")), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

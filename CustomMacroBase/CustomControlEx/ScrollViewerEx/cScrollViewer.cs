using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomMacroBase.CustomControlEx.ScrollViewerEx
{
    public partial class cScrollViewer : ScrollViewer
    {
        static cScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cScrollViewer), new FrameworkPropertyMetadata(typeof(cScrollViewer)));
        }
    }

    public partial class cScrollViewer
    {
        public Brush BackgroundColor
        {
            get { return (Brush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
            name: "BackgroundColor",
            propertyType: typeof(Brush),
            ownerType: typeof(cScrollViewer),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public CornerRadius BackgroundCornerRadius
        {
            get { return (CornerRadius)GetValue(BackgroundCornerRadiusProperty); }
            set { SetValue(BackgroundCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BackgroundCornerRadiusProperty = DependencyProperty.Register(
            name: "BackgroundCornerRadius",
            propertyType: typeof(CornerRadius),
            ownerType: typeof(cScrollViewer),
            typeMetadata: new FrameworkPropertyMetadata(new CornerRadius(0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Brush ScrollBarColor
        {
            get { return (Brush)GetValue(ScrollBarColorProperty); }
            set { SetValue(ScrollBarColorProperty, value); }
        }
        public static readonly DependencyProperty ScrollBarColorProperty = DependencyProperty.Register(
            name: "ScrollBarColor",
            propertyType: typeof(Brush),
            ownerType: typeof(cScrollViewer),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Gray), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Brush ScrollBarBackgroundColor
        {
            get { return (Brush)GetValue(ScrollBarBackgroundColorProperty); }
            set { SetValue(ScrollBarBackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty ScrollBarBackgroundColorProperty = DependencyProperty.Register(
            name: "ScrollBarBackgroundColor",
            propertyType: typeof(Brush),
            ownerType: typeof(cScrollViewer),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public bool HideContent
        {
            get { return (bool)GetValue(HideContentProperty); }
            set { SetValue(HideContentProperty, value); }
        }
        public static readonly DependencyProperty HideContentProperty = DependencyProperty.Register(
            name: "HideContent",
            propertyType: typeof(bool),
            ownerType: typeof(cScrollViewer),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

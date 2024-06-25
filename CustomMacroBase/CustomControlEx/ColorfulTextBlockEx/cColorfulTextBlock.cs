using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomMacroBase.CustomControlEx.ColorfulTextBlockEx
{
    public partial class cColorfulTextBlock : Control
    {
        static cColorfulTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cColorfulTextBlock), new FrameworkPropertyMetadata(typeof(cColorfulTextBlock)));
        }
    }

    public partial class cColorfulTextBlock
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: "Text",
            propertyType: typeof(string),
            ownerType: typeof(cColorfulTextBlock),
            typeMetadata: new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Thickness TextMargin
        {
            get { return (Thickness)GetValue(TextMarginProperty); }
            set { SetValue(TextMarginProperty, value); }
        }
        public static readonly DependencyProperty TextMarginProperty = DependencyProperty.Register(
            name: "TextMargin",
            propertyType: typeof(Thickness),
            ownerType: typeof(cColorfulTextBlock),
            typeMetadata: new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public SolidColorBrush TextBaseColor
        {
            get { return (SolidColorBrush)GetValue(TextBaseColorProperty); }
            set { SetValue(TextBaseColorProperty, value); }
        }
        public static readonly DependencyProperty TextBaseColorProperty = DependencyProperty.Register(
            name: "TextBaseColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cColorfulTextBlock),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Gray), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
            name: "BackgroundColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cColorfulTextBlock),
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
            ownerType: typeof(cColorfulTextBlock),
            typeMetadata: new FrameworkPropertyMetadata(new CornerRadius(0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public HorizontalAlignment TextHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(TextHorizontalAlignmentProperty); }
            set { SetValue(TextHorizontalAlignmentProperty, value); }
        }
        public static readonly DependencyProperty TextHorizontalAlignmentProperty = DependencyProperty.Register(
            name: "TextHorizontalAlignment",
            propertyType: typeof(HorizontalAlignment),
            ownerType: typeof(cColorfulTextBlock),
            typeMetadata: new FrameworkPropertyMetadata(HorizontalAlignment.Center, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public VerticalAlignment TextVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(TextVerticalAlignmentProperty); }
            set { SetValue(TextVerticalAlignmentProperty, value); }
        }
        public static readonly DependencyProperty TextVerticalAlignmentProperty = DependencyProperty.Register(
            name: "TextVerticalAlignment",
            propertyType: typeof(VerticalAlignment),
            ownerType: typeof(cColorfulTextBlock),
            typeMetadata: new FrameworkPropertyMetadata(VerticalAlignment.Center, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public SolidColorBrush MaskBackgroundColor
        {
            get { return (SolidColorBrush)GetValue(MaskBackgroundColorProperty); }
            set { SetValue(MaskBackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty MaskBackgroundColorProperty = DependencyProperty.Register(
            name: "MaskBackgroundColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cColorfulTextBlock),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

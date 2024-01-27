using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.RippleButtonEx
{
    public partial class cRippleButton : Button
    {
        static cRippleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cRippleButton), new FrameworkPropertyMetadata(typeof(cRippleButton)));
        }
    }

    public partial class cRippleButton
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: "Text",
            propertyType: typeof(string),
            ownerType: typeof(cRippleButton),
            typeMetadata: new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public ButtonType Type
        {
            get { return (ButtonType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            name: "Type",
            propertyType: typeof(ButtonType),
            ownerType: typeof(cRippleButton),
            typeMetadata: new FrameworkPropertyMetadata(ButtonType.Noamal, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register(
            name: "BorderCornerRadius",
            propertyType: typeof(CornerRadius),
            ownerType: typeof(cRippleButton),
            typeMetadata: new FrameworkPropertyMetadata(new CornerRadius(1), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }

    public partial class cRippleButton
    {
        public Point ClickPos
        {
            get { return (Point)GetValue(ClickPosProperty); }
            set { SetValue(ClickPosProperty, value); }
        }
        public static readonly DependencyProperty ClickPosProperty = DependencyProperty.Register(
            name: "ClickPos",
            propertyType: typeof(Point),
            ownerType: typeof(cRippleButton),
            typeMetadata: new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Point EnterPos
        {
            get { return (Point)GetValue(EnterPosProperty); }
            set { SetValue(EnterPosProperty, value); }
        }
        public static readonly DependencyProperty EnterPosProperty = DependencyProperty.Register(
            name: "EnterPos",
            propertyType: typeof(Point),
            ownerType: typeof(cRippleButton),
            typeMetadata: new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

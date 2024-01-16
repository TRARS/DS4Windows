using System.Windows;

namespace CustomMacroBase.CustomControlEx.TitleBarButtonEx
{
    public partial class cTitleBarButton : System.Windows.Controls.Button
    {
        static cTitleBarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cTitleBarButton), new FrameworkPropertyMetadata(typeof(cTitleBarButton)));
        }
    }

    public partial class cTitleBarButton
    {
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
            name: "IsActive",
            propertyType: typeof(bool),
            ownerType: typeof(cTitleBarButton),
            typeMetadata: new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: "Text",
            propertyType: typeof(string),
            ownerType: typeof(cTitleBarButton),
            typeMetadata: new FrameworkPropertyMetadata("cTitleBarButton", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public ButtonType Type
        {
            get { return (ButtonType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            name: "Type",
            propertyType: typeof(ButtonType),
            ownerType: typeof(cTitleBarButton),
            typeMetadata: new FrameworkPropertyMetadata(ButtonType.EmptyBtn, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register(
            name: "BorderCornerRadius",
            propertyType: typeof(CornerRadius),
            ownerType: typeof(cTitleBarButton),
            typeMetadata: new FrameworkPropertyMetadata(new CornerRadius(0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

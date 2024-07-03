using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.VerticalRadioButtonEx
{
    public partial class cVerticalRadioButton : RadioButton
    {
        static cVerticalRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cVerticalRadioButton), new FrameworkPropertyMetadata(typeof(cVerticalRadioButton)));
        }
    }

    public partial class cVerticalRadioButton
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: "Text",
            propertyType: typeof(string),
            ownerType: typeof(cVerticalRadioButton),
            typeMetadata: new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public bool EnableColorfulText
        {
            get { return (bool)GetValue(EnableColorfulTextProperty); }
            set { SetValue(EnableColorfulTextProperty, value); }
        }
        public static readonly DependencyProperty EnableColorfulTextProperty = DependencyProperty.Register(
            name: "EnableColorfulText",
            propertyType: typeof(bool),
            ownerType: typeof(cVerticalRadioButton),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public bool Hide
        {
            get { return (bool)GetValue(HideProperty); }
            set { SetValue(HideProperty, value); }
        }
        public static readonly DependencyProperty HideProperty = DependencyProperty.Register(
            name: "Hide",
            propertyType: typeof(bool),
            ownerType: typeof(cVerticalRadioButton),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

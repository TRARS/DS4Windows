using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.NormalButtonEx
{
    public partial class cNormalButton : Button
    {
        static cNormalButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cNormalButton), new FrameworkPropertyMetadata(typeof(cNormalButton)));
        }
    }

    public partial class cNormalButton
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: "Text",
            propertyType: typeof(string),
            ownerType: typeof(cNormalButton),
            typeMetadata: new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

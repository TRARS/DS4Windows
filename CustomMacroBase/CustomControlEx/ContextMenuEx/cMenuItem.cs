using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.ContextMenuEx
{
    public partial class cMenuItem : MenuItem
    {
        static cMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cMenuItem), new FrameworkPropertyMetadata(typeof(cMenuItem)));
        }
        public cMenuItem() { }
    }

    public partial class cMenuItem
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: "Text",
            propertyType: typeof(string),
            ownerType: typeof(cMenuItem),
            typeMetadata: new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}
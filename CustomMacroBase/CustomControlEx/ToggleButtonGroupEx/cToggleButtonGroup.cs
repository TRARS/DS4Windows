using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.ToggleButtonGroupEx
{
    public partial class cToggleButtonGroup : Control
    {
        static cToggleButtonGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cToggleButtonGroup), new FrameworkPropertyMetadata(typeof(cToggleButtonGroup)));
        }
    }

    public partial class cChildrenContainer : ItemsControl
    {
        static cChildrenContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cChildrenContainer), new FrameworkPropertyMetadata(typeof(cChildrenContainer)));
        }

        public bool EnableChildren
        {
            get { return (bool)GetValue(EnableChildrenProperty); }
            set { SetValue(EnableChildrenProperty, value); }
        }
        public static readonly DependencyProperty EnableChildrenProperty = DependencyProperty.Register(
            name: "EnableChildren",
            propertyType: typeof(bool),
            ownerType: typeof(cChildrenContainer),
            typeMetadata: new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }

    public partial class cChildrenExContainer : ItemsControl
    {
        static cChildrenExContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cChildrenExContainer), new FrameworkPropertyMetadata(typeof(cChildrenExContainer)));
        }
    }
}

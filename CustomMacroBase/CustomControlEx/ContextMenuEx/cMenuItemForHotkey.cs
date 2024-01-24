using CustomMacroBase.PreBase;
using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.ContextMenuEx
{
    public partial class cMenuItemForHotkey : MenuItem
    {
        static cMenuItemForHotkey()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cMenuItemForHotkey), new FrameworkPropertyMetadata(typeof(cMenuItemForHotkey)));
        }

        public cMenuItemForHotkey()
        {
            this.StaysOpenOnClick = true;
        }
    }

    public partial class cMenuItemForHotkey
    {
        public GateBase? Source
        {
            get { return (GateBase)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            name: "Source",
            propertyType: typeof(GateBase),
            ownerType: typeof(cMenuItemForHotkey),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

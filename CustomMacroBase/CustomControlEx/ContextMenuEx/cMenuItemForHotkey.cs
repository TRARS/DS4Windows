using System.Windows;
using System.Windows.Controls;
using TrarsUI.Shared.DTOs;
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
        public ToggleTreeViewNode? Source
        {
            get { return (ToggleTreeViewNode)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            name: "Source",
            propertyType: typeof(ToggleTreeViewNode),
            ownerType: typeof(cMenuItemForHotkey),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

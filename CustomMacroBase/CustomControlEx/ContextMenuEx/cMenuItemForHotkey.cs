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
        public cMenuItemForHotkey(GateBase gb)
        {
            this.StaysOpenOnClick = true;
            this.DataContext = new cMenuItemForHotkey_viewmodel(gb.Text, gb.Feature, gb.EnableReverse);
        }
    }
}

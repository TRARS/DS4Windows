using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.ContextMenuEx
{
    public partial class cContextMenu : ContextMenu
    {
        static cContextMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cContextMenu), new FrameworkPropertyMetadata(typeof(cContextMenu)));
        }
        public cContextMenu()
        {
            this.StaysOpen = true;
            this.Focusable = true;
        }
    }
}

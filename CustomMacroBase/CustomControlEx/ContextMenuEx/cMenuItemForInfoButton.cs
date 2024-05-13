using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.ContextMenuEx
{
    public partial class cMenuItemForInfoButton : MenuItem
    {
        static cMenuItemForInfoButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cMenuItemForInfoButton), new FrameworkPropertyMetadata(typeof(cMenuItemForInfoButton)));
        }

        public cMenuItemForInfoButton()
        {
            this.StaysOpenOnClick = true;
        }
    }
}

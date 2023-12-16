using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.ComboBoxEx
{
    public partial class cComboBox : ComboBox
    {
        static cComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cComboBox), new FrameworkPropertyMetadata(typeof(cComboBox)));
        }
    }
}

using CustomMacroBase.Helper;
using CustomMacroBase.PixelMatcher;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CustomMacroFactory.MainView.UserControlEx.PixelPicker
{
    public partial class uPixelPicker
    {
        private static int count = 0;
    }

    public partial class uPixelPicker : UserControl
    {
        public uPixelPicker()
        {
            InitializeComponent();

            if (count++ == 0)
            {
                PixelMatcherHost.TryInit();
            }
            else
            {
                Task.Run(() =>
                {
                    var msg = "An exception occurred in uPixelPicker, resulting in duplicate loading.";
                    Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, msg);

                    var designmode = (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
                    if (designmode is false) { MessageBox.Show($"{DateTime.Now.ToString("HH:mm:ss")} -> {msg}"); }
                });
            }
        }
    }
}

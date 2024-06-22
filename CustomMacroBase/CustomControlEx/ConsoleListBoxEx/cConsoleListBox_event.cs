using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomMacroBase.CustomControlEx.ConsoleListBoxEx
{
    partial class cConsoleListBox_event
    {
        private void Item_PreviewMouseLeftButtonDown(object s, MouseButtonEventArgs e)
        {
            SetTextToClipboard(((ListBoxItem)s).DataContext.ToString());
        }

        private void Item_PreviewMouseRightButtonDown(object s, MouseButtonEventArgs e)
        {
            SetTextToClipboard(((ListBoxItem)s).DataContext.ToString());
        }

        private void SetTextToClipboard(string? obj)
        {
            if (string.IsNullOrEmpty(obj)) { return; }

            var sta_thread = new System.Threading.Thread(() =>
            {
                try { Clipboard.SetText(obj); }
                catch (Exception ex) { MessageBox.Show($"{ex.Message}"); }
            });
            sta_thread.SetApartmentState(System.Threading.ApartmentState.STA);
            sta_thread.Start();
        }
    }
}

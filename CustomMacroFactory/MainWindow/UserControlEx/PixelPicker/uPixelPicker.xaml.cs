using CustomMacroBase.Helper;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomMacroFactory.MainWindow.UserControlEx.PixelPicker
{
    public partial class uPixelPicker
    {
        static readonly uPixelPicker_viewmodel viewmodel = new();
        static int count = 0;
    }

    public partial class uPixelPicker : UserControl
    {
        private static readonly Lazy<uPixelPicker> lazyObject = new(() => new uPixelPicker());
        public static uPixelPicker Instance => lazyObject.Value;

        private uPixelPicker()
        {
            InitializeComponent();

            if (count++ == 0)
            {
                this.DataContext = viewmodel;

                this.Loaded += OnLoaded;
                this.PreviewMouseRightButtonUp += OnPreviewMouseRightButtonUp;
                this.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
                this.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
                this.MouseEnter += OnMouseEnter;
                this.MouseLeave += OnMouseLeave;
                this.MouseMove += OnMouseMove;
            }
            else
            {
                Task.Run(() =>
                {
                    var msg = "An exception occurred in uPixelPicker, resulting in duplicate loading.";
                    Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, msg);
                    MessageBox.Show($"{DateTime.Now.ToString("HH:mm:ss")} -> {msg}");
                });
            }
        }
    }

    public partial class uPixelPicker
    {
        private void OnLoaded(object s, RoutedEventArgs e) => viewmodel.Loaded?.Invoke(s, e);
        private void OnPreviewMouseRightButtonUp(object s, MouseButtonEventArgs e) => viewmodel.PreviewMouseRightButtonUp?.Invoke(s, e);
        private void OnPreviewMouseLeftButtonDown(object s, MouseButtonEventArgs e) => viewmodel.PreviewMouseLeftButtonDown?.Invoke(s, e);
        private void OnPreviewMouseLeftButtonUp(object s, MouseButtonEventArgs e) => viewmodel.PreviewMouseLeftButtonUp?.Invoke(s, e);
        private void OnMouseEnter(object s, MouseEventArgs e) => viewmodel.MouseEnter?.Invoke(s, e);
        private void OnMouseLeave(object s, MouseEventArgs e) => viewmodel.MouseLeave?.Invoke(s, e);
        private void OnMouseMove(object s, MouseEventArgs e) => viewmodel.MouseMove?.Invoke(s, e);
    }
}

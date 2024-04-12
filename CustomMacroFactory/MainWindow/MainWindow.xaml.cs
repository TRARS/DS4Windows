using CustomMacroBase.GamePadState;
using CustomMacroBase.Helper;
using CustomMacroBase.Helper.Extensions;
using CustomMacroFactory.MacroFactory;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace CustomMacroFactory.MainWindow
{
    //UserControl 在自己的 view.xaml 中引用style资源
    //CustomControl 在 Themes/Generic.xaml 中引用style资源

    //无边框相关处理
    public partial class MainWindow : Window
    {
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var handle = new WindowInteropHelper(this).Handle;
            if (handle != IntPtr.Zero)
            {
                var style = Win32.GetWindowLong(handle, (int)Win32.GetWindowLongIndex.GWL_STYLE);
                style |= (int)Win32.WindowStyles.WS_CAPTION;
                Win32.SetWindowLong(handle, (int)Win32.GetWindowLongIndex.GWL_STYLE, style);
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(this.WindowProc));
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            var handle = new WindowInteropHelper(this).Handle;
            if (handle != IntPtr.Zero)
            {
                HwndSource.FromHwnd(handle).RemoveHook(this.WindowProc);
            }
            base.OnClosing(e);
        }

        private IntPtr WindowProc(IntPtr handle, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == (int)Win32.WindowMessages.WM_NCHITTEST)
            {
                var result = this.OnNcHitTest(handle, wParam, lParam);
                if (result != null)
                {
                    handled = true;
                    return result.Value;
                }
            }
            if (msg == (int)Win32.WindowMessages.WM_SIZE)
            {
                //this.LayoutRoot.Margin = new Thickness(0);
            }
            if (msg == (int)Win32.WindowMessages.WM_DPICHANGED)
            {
                //handled = true;
            }
            return IntPtr.Zero;
        }
        private IntPtr? OnNcHitTest(IntPtr handle, IntPtr wParam, IntPtr lParam)
        {
            var screenPoint = new Point((int)lParam & 0xFFFF, ((int)lParam >> 16) & 0xFFFF);
            var clientPoint = this.PointFromScreen(screenPoint);
            var borderHitTest = this.GetBorderHitTest(clientPoint);
            if (borderHitTest != null)
            {
                return (IntPtr)borderHitTest;
            }
            clientPoint.Y -= this.BorderThickness.Top;// 边框补正
            clientPoint.X -= this.BorderThickness.Left;
            var chromeHitTest = this.GetChromeHitTest(clientPoint);
            if (chromeHitTest != null)
            {
                return (IntPtr)chromeHitTest;
            }
            return null;
        }
        private Win32.HitTestResult? GetBorderHitTest(Point point)
        {
            if (this.WindowState != WindowState.Normal) return null;
            if (this.ResizeMode == ResizeMode.NoResize) return null;

            var 边距 = (Math.Max(this.BorderThickness.Left * 2, 4));//MainWindow.BorderThickness
            var top = (point.Y <= 边距);
            var bottom = (point.Y >= this.Height - 边距);
            var left = (point.X <= 边距);
            var right = (point.X >= this.Width - 边距);

            if (top && left) return Win32.HitTestResult.HTTOPLEFT;
            if (top && right) return Win32.HitTestResult.HTTOPRIGHT;
            if (top) return Win32.HitTestResult.HTTOP;

            if (bottom && left) return Win32.HitTestResult.HTBOTTOMLEFT;
            if (bottom && right) return Win32.HitTestResult.HTBOTTOMRIGHT;
            if (bottom) return Win32.HitTestResult.HTBOTTOM;

            if (left) return Win32.HitTestResult.HTLEFT;
            if (right) return Win32.HitTestResult.HTRIGHT;

            return null;
        }
        private Win32.HitTestResult? GetChromeHitTest(Point point)
        {
            var result = VisualTreeHelper.HitTest(this.Chrome, point);
            if (result != null)
            {
                var button = result.VisualHit.FindVisualAncestor<Button>();
                var checkbox = result.VisualHit.FindVisualAncestor<CheckBox>();
                if (button == null && checkbox == null)
                {
                    return Win32.HitTestResult.HTCAPTION;
                }
            }
            return null;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = (this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized) { this.WindowState = WindowState.Normal; }
        }
    }

    //构造函数
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Left = this.Top = 5;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                await Task.Delay(500);
                await this.Dispatcher.BeginInvoke(() =>
                {
                    string str = $"{this.Width},{this.Height}";
                    Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, str);
                });
            });

            this.PreviewKeyDown += (s, e) =>
            {
                Mediator.Instance.NotifyColleagues(MessageType.WindowKeyDown, e.Key);
            };

            this.TryMoveToPrimaryMonitor(new(0, 0));
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = (this.WindowState == WindowState.Minimized ? WindowState.Normal : WindowState.Minimized);
        }
    }

    //Entry
    public partial class MainWindow
    {
        public void DS4StateCustomUpdateMain(in int ind, in DS4StateLite _realState, in DS4StateLite _virtualState)
        {
            MacroManager.Entry(in ind, in _realState, in _virtualState);
        }
    }
}

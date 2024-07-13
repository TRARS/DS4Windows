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

namespace CustomMacroFactory.MainView
{
    //UserControl 在自己的 view.xaml 中引用style资源
    //CustomControl 在 Themes/Generic.xaml 中引用style资源

    //无边框相关处理
    public partial class MainWindow
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
    }

    //Initializer
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); Left = Top = 5; this.Opacity = 0;
        }
    }

    //Method
    public partial class MainWindow
    {
        //Entry
        public void MacroEntry(in int ind, in DS4StateLite _realState, in DS4StateLite _virtualState)
        {
            MacroManager.Entry(in ind, in _realState, in _virtualState);
        }

        //Exit
        public void Exit()
        {
            Mediator.Instance.UnRegister(MessageType.WindowClose);
            Mediator.Instance.UnRegister(MainWindowMessageType.Closing);
            //Environment.Exit(0);
        }

        //HideToTray
        public void HideToTray()
        {
            Mediator.Instance.NotifyColleagues(MainWindowMessageType.HideToTray, null);
        }

        //Init
        public void Init(Func<dynamic> getDs4Host, Func<dynamic> getRootHub)
        {
            Mediator.Instance.Register(MainWindowMessageType.Loaded, _ =>
            {
                Task.Run(async () =>
                {
                    await Task.Delay(512);
                    this.Dispatcher.Invoke(() =>
                    {
                        string str = $"{this.Width},{this.Height}";
                        Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, str);
                        this.SizeToContent = SizeToContent.WidthAndHeight;
                        this.Opacity = 1;
                    });
                });

                this.TryMoveToPrimaryMonitor(new(0, 0));
            });
            Mediator.Instance.Register(MainWindowMessageType.Closing, _ =>
            {
                this.WindowState = (this.WindowState == WindowState.Minimized ? WindowState.Normal : WindowState.Minimized);
            });
            Mediator.Instance.Register(MainWindowMessageType.HideToTray, _ =>
            {
                var isMinimized = this.WindowState is WindowState.Minimized;
                {
                    this.WindowState = isMinimized ? (this.ShowInTaskbar ? this.WindowState : WindowState.Normal) : WindowState.Minimized;
                    this.ShowInTaskbar = isMinimized ? (!this.ShowInTaskbar) : false;
                }
            });

            Mediator.Instance.Register(MessageType.WindowPosReset, para =>
            {
                this.TryMoveToPrimaryMonitor((Vector)para);
            });
            Mediator.Instance.Register(MessageType.WindowClose, _ =>
            {
                //this.Close(); //直接Close()会导致Hook被移除
                this.WindowState = WindowState.Minimized;
            });
            Mediator.Instance.Register(MessageType.Ds4Disconnect, _ =>
            {
                dynamic? ds4 = getDs4Host?.Invoke();
                ds4?.CloseBT();
            });
            Mediator.Instance.Register(MessageType.Ds4Rumble, para =>
            {//byte rightLightFastMotor, byte leftHeavySlowMotor
                dynamic? rootHub = getRootHub?.Invoke();
                dynamic? d = rootHub?.DS4Controllers[0];
                byte LightRumble = (bool)para ? byte.MinValue : byte.MaxValue;
                byte HeavyRumble = (bool)para ? byte.MaxValue : byte.MinValue;
                if (d is not null)
                {
                    Task.Run(() =>
                    {
                        d.setRumble(LightRumble, HeavyRumble);//LightRumble //HeavyRumble
                        Task.Delay(200).Wait();
                        d.setRumble(0, 0);
                        Task.Delay(200).Wait();
                    });
                }
            });
            Mediator.Instance.Register(MessageType.Ds4Latency, para =>
            {
                dynamic? rootHub = getRootHub?.Invoke();
                dynamic? d = rootHub?.DS4Controllers[0];
                ((double[])para)[0] = (d is not null ? d.Latency : 0);
            });

            this.Show();
        }
    }
}

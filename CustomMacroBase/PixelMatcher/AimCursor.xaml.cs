using CustomMacroBase.Helper;
using System;
using System.Windows;
using System.Windows.Interop;

namespace CustomMacroBase.PixelMatcher
{
    public sealed partial class AimCursor : Window
    {
        #region 消息处理
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var handle = new WindowInteropHelper(this).Handle;
            if (handle != IntPtr.Zero)
            {
                var style = Win32.GetWindowLong(handle, (int)Win32.GetWindowLongIndex.GWL_STYLE);
                style = (int)Win32.WindowStyles.WS_EX_NOACTIVATE;
                Win32.SetWindowLong(handle, (int)Win32.GetWindowLongIndex.GWL_EXSTYLE, style);
            }
        }
        #endregion
    }

    public sealed partial class AimCursor
    {
        private static Func<Window, double> GetScalingFactor = (Window para) =>
        {
            return 1 / PresentationSource.FromVisual(para).CompositionTarget.TransformToDevice.M11;
        };

        private static readonly Lazy<AimCursor> lazyObject = new(() =>
        {
            var obj = new AimCursor();
            {
                obj.Show();
                obj.Topmost = false;
            }
            return obj;
        });
        public static AimCursor Instance => lazyObject.Value;

        private AimCursor()
        {
            InitializeComponent();
            this.Opacity = 0;
            this.ShowInTaskbar = false;
            this.IsHitTestVisible = false;
            this.ShowActivated = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Win32_Ex_Transparency.SetTransparency(this);
        }
    }

    public sealed partial class AimCursor
    {
        public void MoveTo(System.Drawing.Point p)
        {
            var gsf = GetScalingFactor.Invoke(this);
            Instance.Left = p.X * gsf - Instance.Width / 2;
            Instance.Top = p.Y * gsf - Instance.Height / 2;
        }

        public void SetWindowOpacity(bool flag)
        {
            if (Instance.Topmost != flag)
            {
                Instance.Opacity = flag ? 1 : 0;
                Instance.Topmost = flag;
            }
        }
    }
}

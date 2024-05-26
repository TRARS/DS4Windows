using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace CustomMacroBase.Helper
{
    public static class WindowExt
    {
        public static Screen? pMonitor => Screen.AllScreens.Where(s => s.Primary).FirstOrDefault();
        public static Screen? sMonitor => Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();

        private static readonly double ratioX = 0;//0.152;
        private static readonly double ratioY = 0;//0.8;
        private static Func<Window, double> GetDeviceScaleFactor = (Window para) =>
        {
            return 1 / PresentationSource.FromVisual(para).CompositionTarget.TransformToDevice.M11;
        };



        #region TryMoveToOtherMonitor
        public static void TryMoveToSecondaryMonitor(this Window window, Vector? _vector = null)
        {
            MoveToTargetMonitor(window, pMonitor, sMonitor, _vector);
        }

        public static void TryMoveToPrimaryMonitor(this Window window, Vector? _vector = null)
        {
            MoveToTargetMonitor(window, pMonitor, null, _vector);
        }

        private static void MoveToTargetMonitor(Window window, Screen? primaryScreen, Screen? secondaryScreen, Vector? _pos = null)
        {
            if (window is null || window.Visibility != Visibility.Visible) { return; };

            //承接目标坐标
            Vector ratio = _pos is null ? new Vector(ratioX, ratioY) : (Vector)_pos;

            //承接窗体当前不透明度
            double opacity = 0d;

            //据观察子窗体在创建时依赖于主窗体当前所在屏的DPI，若主窗体过早地移动会导致子窗体生成时是以副屏DPI为基准创建 会歪掉，故延迟移动
            Task.Run(async () =>
            {
                await window.Dispatcher.BeginInvoke((Action)delegate { opacity = window.Opacity; window.Opacity = 0; });
                {
                    //延时
                    await Task.Delay(32);
                    //先移动到主/副屏左上角，然后再按ratio偏移
                    await window.Dispatcher.BeginInvoke((Action)delegate
                    {
                        //获取目标屏
                        Screen? TargetScreen = secondaryScreen is null ? primaryScreen : secondaryScreen;

                        if (TargetScreen is not null)
                        {
                            //移动到目标屏
                            SetPos(window,
                                   TargetScreen.Bounds.Left,
                                   TargetScreen.Bounds.Top);

                            //再移动到ratio指定位置
                            SetPos(window,
                                   ratio.X * TargetScreen.Bounds.Width,
                                   ratio.Y * TargetScreen.Bounds.Height,
                                   true);
                        }
                    });
                }
                window.Dispatcher.BeginInvoke((Action)delegate { window.Opacity = opacity; });
            });
        }
        private static void SetPos(Window window, double goalX, double goalY, bool _isRelative = false)
        {
            if (_isRelative is false)
            {
                //单次位移于跨窗体时可能遭受DPI变动导致未抵达正确位置
                window.Left = goalX * GetDeviceScaleFactor(window);
                window.Top = goalY * GetDeviceScaleFactor(window);

                //故二次位移保险点
                window.Left = goalX * GetDeviceScaleFactor(window);
                window.Top = goalY * GetDeviceScaleFactor(window);
            }
            else
            {
                window.Left += goalX * GetDeviceScaleFactor(window);
                window.Top += goalY * GetDeviceScaleFactor(window);
            }
        }
        #endregion

        #region TryReSetWindowSize
        public static void TryReSetWindowSize(this Window window, Rect size)
        {
            ReSetSizeFunc(window, size);
        }
        private static void ReSetSizeFunc(this Window window, Rect size)
        {
            //承接缩放倍率
            double multiple = GetDeviceScaleFactor(window);

            window.Width = (size.Width * multiple);
            window.Height = (size.Height * multiple);
        }
        #endregion
    }
}

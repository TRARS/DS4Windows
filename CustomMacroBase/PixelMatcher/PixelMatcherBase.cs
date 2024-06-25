using CustomMacroBase.Helper;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CustomMacroBase.PixelMatcher
{
    public abstract partial class PixelMatcherBase
    {
        private partial class WindowManager
        {
            public IntPtr Handle { get; set; } = IntPtr.Zero;
            public string Title { get; set; } = string.Empty;
            public string ClassName { get; set; } = string.Empty;

            public WindowManager(IntPtr hwnd)
            {
                this.Handle = hwnd;
                this.Init();
            }
        }
        private partial class WindowManager
        {
            private bool can_update = false;
            private bool task_is_running = false;

            private void Init()
            {
                Mediator.Instance.Register(MessageType.CanUpdateFrames, (para) =>
                {
                    can_update = (bool)para;
                });
            }
            private void Print(string str = "")
            {
                Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, str);
            }
            private void UpdateToSnapshotArea(Func<Bitmap?> func, string? msg = null)
            {
                if (can_update is false) { return; } //截图区域未展开时不更新

                if (task_is_running is false)
                {
                    task_is_running = true;

                    Task.Run(async () =>
                    {
                        if (func.Invoke() is Bitmap source)
                        {
                            using (source)
                            {
                                await MediatorAsync.Instance.NotifyColleagues(AsyncMessageType.AsyncSnapshot, source);

                                if (msg is not null) { Print($"{msg}"); }
                            }
                        }
                    }).ContinueWith(_ => { task_is_running = false; });
                }
            }
        }
        private partial class WindowManager
        {
            private enum PWnFlag
            {
                PW_CLIENTONLY = 0x1,
                PW_RENDERFULLCONTENT = 0x2,
                PW_CLIENTONLY_OR_RENDERFULLCONTENT = 0x3
            }
            private struct WindowRect
            {
                private Win32.RECT rect;

                public int X => rect.Left;
                public int Y => rect.Top;
                public int Width => rect.Right - rect.Left;
                public int Height => rect.Bottom - rect.Top;

                public WindowRect(Win32.RECT _rect)
                {
                    rect = _rect;
                }
            }

            private Bitmap? screenshot = null;

            private bool featureEnable = true;
            private Stopwatch refreshLimiter = Stopwatch.StartNew();
            private Stopwatch retryLimiter = Stopwatch.StartNew();
            private bool canRetry => retryLimiter.IsRunning is false || retryLimiter.Elapsed.TotalSeconds > 2;

            private bool IsNotWindow(bool print = false, [CallerMemberName] string caller = "")
            {
                if (this.Handle == IntPtr.Zero || (Win32.IsWindow(this.Handle) is false))
                {
                    if (print)
                    {
                        Print($"{caller} Error: Target Window not found");
                    }
                    return true;
                }

                return false;
            }
            private bool IsWindowMinimized(bool print = false, [CallerMemberName] string caller = "")
            {
                if (Win32.IsIconic(this.Handle))
                {
                    if (print)
                    {
                        Print($"{caller} Error: Target Window is minimized");
                    }
                    return true;
                }

                return false;
            }
            private bool IsWindowVisible([CallerMemberName] string caller = "")
            {
                if (Win32.ShowWindow(this.Handle, (int)Win32.ShowWindowOptions.SHOWDEFAULT))
                {
                    Print($"{caller} message: Target window has been restored");
                    return true;
                }

                return false;
            }
            private void TryRestoreMinimizedWindow(ref bool flag)
            {
                if (flag) { return; }
                if (this.IsNotWindow()) { return; }

                if ((flag is false) && this.IsWindowMinimized() && canRetry)
                {
                    retryLimiter.Restart();

                    if (this.IsWindowVisible())
                    {
                        retryLimiter.Stop(); flag = true;
                    }
                }
            }

            private WindowRect GetWindowRect()
            {
                Win32.GetWindowRect(this.Handle, out Win32.RECT lpRect);
                return new WindowRect(lpRect);
            }
            private WindowRect GetClientRect()
            {
                Win32.GetClientRect(this.Handle, out Win32.RECT lpRect);
                return new WindowRect(lpRect);
            }
            private void MoveWindow(int cw, int ch)
            {
                var fullRect = this.GetWindowRect();
                var clientRect = this.GetClientRect();

                int delta_width = fullRect.Width - clientRect.Width;//边框宽度（不包含客户区）
                int delta_height = fullRect.Height - clientRect.Height;//边框高度（不包含客户区）

                Win32.MoveWindow(this.Handle, fullRect.X, fullRect.Y, cw + delta_width, ch + delta_height, true);
            }
            private bool PrintWindow(nint hDC, uint nFlag)
            {
                return User32.PrintWindow(this.Handle, hDC, nFlag);
            }

            private Bitmap? GetWindowCapture()
            {
                var clientRect = this.GetClientRect();

                var img = new Bitmap(clientRect.Width, clientRect.Height);
                var memg = Graphics.FromImage(img);
                IntPtr dc = memg.GetHdc();
                bool success = this.PrintWindow(dc, (uint)(PWnFlag.PW_CLIENTONLY | PWnFlag.PW_RENDERFULLCONTENT));//3最好使，虽然没得标题栏，但截RemotePlay不黑屏（有时候会有蜜汁镂空）
                memg.ReleaseHdc(dc);
                memg.Dispose();
                return success ? img : null;
            }
        }
        private partial class WindowManager
        {
            public Bitmap? GetScreenshot()
            {
                this.TryRestoreMinimizedWindow(ref featureEnable);

                if (this.IsNotWindow(true)) { featureEnable = false; return null; }
                if (this.IsWindowMinimized(true)) { featureEnable = false; return null; }

                // 限制刷新频率
                if (refreshLimiter.Elapsed.TotalMilliseconds > 100)
                {
                    refreshLimiter.Restart();

                    screenshot?.Dispose();
                    screenshot = this.GetWindowCapture();
                }

                return this.screenshot;
            }

            public void GetWindowSnapshot()
            {
                if (this.IsNotWindow(true)) { featureEnable = false; return; }
                if (this.IsWindowMinimized(true)) { featureEnable = false; return; }

                this.UpdateToSnapshotArea(GetWindowCapture, "GetWindowCapture Done");
            }
            public void SetWindowSize(int cw, int ch)
            {
                if (this.IsNotWindow(true)) { featureEnable = false; return; }
                if (this.IsWindowMinimized(true)) { featureEnable = false; return; }

                this.MoveWindow(cw, ch);
            }
            public void SetWindowHandle(IntPtr _hwnd, string _title, string _classname)
            {
                this.Handle = _hwnd;
                this.Title = _title;
                this.ClassName = _classname;

                featureEnable = true;
            }
        }

        #region 私有字段/属性/方法
        private static WindowManager targetWindow = new(Process.GetProcessesByName("RemotePlay").ToList().FirstOrDefault()?.MainWindowHandle ?? IntPtr.Zero);

        private protected static void GetTargetWindowSnapshot()
        {
            targetWindow.GetWindowSnapshot();
        }
        private protected static void SetTargetWindowHandle(IntPtr _hwnd, string _title, string _classname)
        {
            targetWindow.SetWindowHandle(_hwnd, _title, _classname);
        }
        private protected static void SetTargetWindowSize(int cw = 1920, int ch = 1080)
        {
            targetWindow.SetWindowSize(cw, ch);
        }

        private protected static void Init()
        {
            targetWindow.ToString();
        }
        #endregion

        #region 范围找色_old
        /// <summary>
        /// 范围找色_判断区域内某颜色数量是否大于阈值
        /// </summary>
        //private protected static Point? MatchColor(int argb, int threshold, Rectangle rc = new(), int tolerance = 40)
        //{
        //    Point? result = null;
        //    Color c2 = Color.FromArgb(argb);

        //    if (screenshot is not null)
        //    {
        //        if (rc.IsEmpty) { rc = new Rectangle(0, 0, screenshot.Width, screenshot.Height); }//缺省范围设定为全图
        //        if (rc.Width <= 0 || rc.Height <= 0) { return result; }//没有指定范围或设置了错误范围

        //        unsafe
        //        {
        //            BitmapData data = screenshot.LockBits(new Rectangle(0, 0, screenshot.Width, screenshot.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        //            byte* ptr = (byte*)data.Scan0.ToPointer();
        //            bool[] ck = new bool[3] { false, false, false };
        //            int hit_count = 0;
        //            int offset_start = (rc.Top * data.Width + rc.Left) << 2;//区域起点映射到一维数组
        //            int offset_end = ((rc.Bottom + 1) * data.Width + rc.Right) << 2;//区域终点映射到一维数组

        //            for (int i = 0 + offset_start; i < Math.Clamp(offset_end, offset_start, (data.Width * data.Height) << 2); i += 4)
        //            {
        //                int pos_y = i / data.Stride;
        //                int pos_x = Math.Abs((pos_y * data.Stride - i) >> 2);
        //                if (pos_x >= rc.Left && pos_x < rc.Left + rc.Width &&
        //                    pos_y >= rc.Top && pos_y < rc.Top + rc.Height)
        //                {
        //                    ck[0] = Math.Abs(ptr[i + 2] - c2.R) <= tolerance;
        //                    ck[1] = Math.Abs(ptr[i + 1] - c2.G) <= tolerance;
        //                    ck[2] = Math.Abs(ptr[i + 0] - c2.B) <= tolerance;

        //                    if (ck[0] && ck[1] && ck[2]) { hit_count++; }
        //                    if (hit_count >= threshold) { result = new Point(pos_x, pos_y); break; };
        //                }
        //            }

        //            screenshot.UnlockBits(data);
        //        }
        //    }

        //    return result;
        //}
        #endregion


        #region 范围找色
        /// <summary>
        /// 范围找色_获取区域内与指定颜色相似的颜色数量
        /// </summary>
        private protected static int MatchColor(int argb, Rectangle? rect, int? tolerance)
        {
            if (targetWindow.GetScreenshot() is Bitmap bmp)
            {
                return OpenCV.Instance.MatchColor(bmp, argb, rect, tolerance);
            }

            return 0;
        }
        #endregion

        #region 范围找图
        /// <summary>
        /// 范围找图_判断区域内是否包含小图 (OpenCV)
        /// </summary>
        private protected static Point? MatchImage(ref Bitmap bitmap, Rectangle? rect, double? tolerance)
        {
            if (targetWindow.GetScreenshot() is Bitmap bmp)
            {
                return OpenCV.Instance.MatchImage(bmp, bitmap, rect, tolerance);
            }

            return null;
        }
        /// <summary>
        /// 范围找图_判断区域内是否包含小图 (OpenCV)
        /// </summary>
        private protected static Point? MatchImage(string path, Rectangle? rect, double? tolerance)
        {
            if (targetWindow.GetScreenshot() is Bitmap bmp)
            {
                return OpenCV.Instance.MatchImage(bmp, path, rect, tolerance);
            }

            return null;
        }
        #endregion

        #region 范围找数字
        /// <summary>
        /// 范围找字_尝试识别区域内的数字 (PaddleSharp)
        /// </summary>
        private protected static string MatchNumber(Rectangle rect, bool isWhiteText, OpenCV.DeviceType deviceType, double zoomratio)
        {
            if (targetWindow.GetScreenshot() is Bitmap bmp)
            {
                return OpenCV.Instance.MatchNumber(bmp, rect, isWhiteText, deviceType, OpenCV.ModelType.EnglishV3, zoomratio);
            }

            return string.Empty;
        }
        #endregion

        #region 范围找字
        /// <summary>
        /// 范围找字_尝试识别区域内的文字，需指定语言 (PaddleSharp)
        /// </summary>
        private protected static string MatchText(Rectangle rect, bool isWhiteText, OpenCV.DeviceType deviceType, OpenCV.ModelType language, double zoomratio)
        {
            if (targetWindow.GetScreenshot() is Bitmap bmp)
            {
                return OpenCV.Instance.MatchText(bmp, rect, isWhiteText, deviceType, language, zoomratio);
            }

            return string.Empty;
        }
        #endregion
    }
}

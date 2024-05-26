using CustomMacroBase.Helper;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static CustomMacroBase.PixelMatcher.OpenCV;

namespace CustomMacroBase.PixelMatcher
{
    public abstract partial class PixelMatcherBase
    {
        private protected class MediatorProxy
        {
            bool task_is_running = false;

            public void Print([CallerMemberName] string str = "", int delay = 0)
            {
                Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, str);
            }

            /// <summary>
            /// 将指定的Bitmap推送至MacroWindow
            /// </summary>
            public void UpdateFrames(Bitmap? source)
            {
                if (task_is_running is false && source is not null)
                {
                    task_is_running = true;

                    Task.Run(async () =>
                    {
                        await MediatorAsync.Instance.NotifyColleagues(AsyncMessageType.AsyncSnapshot, source);
                        source.Dispose();
                    }).ContinueWith(_ => { task_is_running = false; Print("GetWindowCapture Done"); });
                }
            }
        }

        //私有字段/属性/方法
        private protected static bool Enable = true;
        private protected static MediatorProxy mediatorProxy = new();
        private protected static Bitmap? screenshot = null;
        private protected static Stopwatch timer = Stopwatch.StartNew();

        private protected static Action UpdateFrames = delegate
        {
            if (Enable is false) { return; }

            // 限制刷新频率
            if (timer.Elapsed.TotalMilliseconds > 100)
            {
                timer.Restart();
            }
            else
            {
                return;
            }

            screenshot?.Dispose();
            screenshot = GetTargetWindowCaptureByHandle();
        };
        private protected static Action CopyToClipboard = delegate
        {
            mediatorProxy.UpdateFrames(GetTargetWindowCaptureByHandle());//需要同步，否则会被过早释放

            {
                //System.Threading.Thread sta_thread = new System.Threading.Thread(() =>
                //{
                //    try
                //    {
                //        using (var temp = GetTargetWindowCaptureByHandle())
                //        {
                //            if (temp is not null)
                //            {
                //                {
                //                    var bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                //                    using (System.IO.MemoryStream ms = new())
                //                    {
                //                        temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //                        bitmapImage.BeginInit();
                //                        bitmapImage.StreamSource = ms;
                //                        bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                //                        bitmapImage.EndInit();
                //                        bitmapImage.Freeze();
                //                    }
                //                    System.Windows.Clipboard.SetImage(bitmapImage);
                //                }//弄到剪贴板必然发生内存泄漏，无解
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        mp.PrintHint($"error: {ex.Message}");
                //    }
                //});
                //sta_thread.SetApartmentState(System.Threading.ApartmentState.STA);
                //sta_thread.Start();
            } //剪贴板相关_不再使用
        };

        #region 通过窗体句柄获取窗体截图，顺便改改窗体尺寸
        private protected static IntPtr TargetWindowHandle = Process.GetProcessesByName("RemotePlay").ToList().FirstOrDefault()?.MainWindowHandle ?? IntPtr.Zero;
        private protected static string TargetWindowTitle = string.Empty;
        private protected static string TargetWindowClassName = string.Empty;

        private protected static void SetTargetWindowHandle(IntPtr _hwnd, string _title, string _classname)
        {
            TargetWindowHandle = _hwnd;
            TargetWindowTitle = _title;
            TargetWindowClassName = _classname;
            Enable = true;
        }
        private protected static void SetTargetWindowSizeByHandle(int x = 1920, int y = 1080)//1920 + 26, 1080 + 71
        {
            IntPtr _handle = TargetWindowHandle;
            if (_handle == IntPtr.Zero || (Win32.IsWindow(_handle) is false))
            {
                mediatorProxy.Print("SetTargetWindowSize Error: Target Window not found");
                return;
            }
            if (Win32.IsIconic(_handle))
            {
                mediatorProxy.Print("SetTargetWindowSize Error: Target Window is minimized");
                return;
            }

            Win32.GetWindowRect(_handle, out Win32.RECT lpRect);
            int orginal_width = lpRect.Right - lpRect.Left;
            int orginal_height = lpRect.Bottom - lpRect.Top; //获得left top width height

            Win32.GetClientRect(_handle, out Win32.RECT clientRect);
            int client_width = clientRect.Right - clientRect.Left;
            int client_height = clientRect.Bottom - clientRect.Top;

            int delta_width = orginal_width - client_width;//边框宽度（不包含客户区）
            int delta_height = orginal_height - client_height;//边框高度（不包含客户区）

            Point BaseSize = new Point() { X = x + delta_width, Y = y + delta_height };
            Win32.MoveWindow(_handle, lpRect.Left, lpRect.Top, BaseSize.X, BaseSize.Y, true);
        }
        private protected static Bitmap? GetTargetWindowCaptureByHandle()
        {
            IntPtr _handle = TargetWindowHandle;
            if (_handle == IntPtr.Zero || (Win32.IsWindow(_handle) is false))
            {
                mediatorProxy.Print("GetTargetWindowFrame Error: Target Window not found");
                Enable = false; return null;
            }
            if (Win32.IsIconic(_handle))
            {
                mediatorProxy.Print("GetTargetWindowFrame Error: Target Window is minimized");
                Enable = false; return null;
            }


            //IntPtr hdcSrc = User32.GetWindowDC(_handle);
            //Win32.RECT windowRect = new();
            //Win32.GetWindowRect(_handle, ref windowRect);
            //int width = windowRect.Right - windowRect.Left;
            //int height = windowRect.Bottom - windowRect.Top;
            {//这样搞无法对chrome浏览器截图，对obs也只能截一次
                //IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
                //IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
                //IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
                //GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
                //GDI32.SelectObject(hdcDest, hOld);
                //GDI32.DeleteDC(hdcDest);
                //User32.ReleaseDC(_handle, hdcSrc);
                //Bitmap img = Image.FromHbitmap(hBitmap, IntPtr.Zero);
                //GDI32.DeleteObject(hBitmap);
            }

            Win32.GetClientRect(_handle, out Win32.RECT clientRect);
            int client_width = clientRect.Right - clientRect.Left;
            int client_height = clientRect.Bottom - clientRect.Top;

            var img = new Bitmap(client_width, client_height);
            var memg = Graphics.FromImage(img);
            IntPtr dc = memg.GetHdc();
            bool success = User32.PrintWindow(_handle, dc, 3);//3最好使，虽然没得标题栏，但截RemotePlay不黑屏（有时候会有蜜汁镂空）
                                                              //PW_CLIENTONLY = 0x1
                                                              //PW_RENDERFULLCONTENT = 0x2
                                                              //PW_CLIENTONLY | PW_RENDERFULLCONTENT = 0x3
            memg.ReleaseHdc(dc);
            memg.Dispose();
            return success ? img : null;
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
            if (screenshot is null) { return 0; }
            return OpenCV.Instance.MatchColor(ref screenshot, argb, rect, tolerance);
        }

        #endregion

        #region 范围找图
        /// <summary>
        /// 范围找图_判断区域内是否包含小图 (OpenCV)
        /// </summary>
        private protected static Point? MatchImage(ref Bitmap bitmap, Rectangle? rect, double? tolerance)
        {
            if (screenshot is null) { return null; }
            return OpenCV.Instance.MatchImage(ref screenshot, ref bitmap, rect, tolerance);
        }
        /// <summary>
        /// 范围找图_判断区域内是否包含小图 (OpenCV)
        /// </summary>
        private protected static Point? MatchImage(string path, Rectangle? rect, double? tolerance)
        {
            if (screenshot is null) { return null; }
            return OpenCV.Instance.MatchImage(ref screenshot, path, rect, tolerance);
        }
        #endregion

        #region 范围找数字
        /// <summary>
        /// 范围找字_尝试识别区域内的数字 (PaddleSharp)
        /// </summary>
        private protected static string MatchNumber(Rectangle rect, bool isWhiteText, DeviceType deviceType, double zoomratio)
        {
            if (screenshot is null) { return string.Empty; }
            return OpenCV.Instance.MatchNumber(ref screenshot, rect, isWhiteText, deviceType, ModelType.EnglishV3, zoomratio);
        }
        #endregion

        #region 范围找字
        /// <summary>
        /// 范围找字_尝试识别区域内的文字，需指定语言 (PaddleSharp)
        /// </summary>
        private protected static string MatchText(Rectangle rect, bool isWhiteText, DeviceType deviceType, ModelType language, double zoomratio)
        {
            if (screenshot is null) { return string.Empty; }
            return OpenCV.Instance.MatchText(ref screenshot, rect, isWhiteText, deviceType, language, zoomratio);
        }
        #endregion
    }
}

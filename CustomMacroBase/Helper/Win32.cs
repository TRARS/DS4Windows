using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CustomMacroBase.Helper
{
    public static class Win32
    {
        public enum ShowWindowOptions
        {
            FORCEMINIMIZE = 11,
            HIDE = 0,
            MAXIMIZE = 3,
            MINIMIZE = 6,
            RESTORE = 9,
            SHOW = 5,
            SHOWDEFAULT = 10,
            SHOWMAXIMIZED = 3,
            SHOWMINIMIZED = 2,
            SHOWMINNOACTIVE = 7,
            SHOWNA = 8,
            SHOWNOACTIVATE = 4,
            SHOWNORMAL = 1
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        public static extern void MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("User32")]
        public extern static void SetCursorPos(int x, int y);
        [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);
        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        //[DllImport("user32.dll", SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32", SetLastError = true)]
        public static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint msg, uint action, IntPtr unused);
        [DllImport("user32", SetLastError = true)]
        public static extern bool ChangeWindowMessageFilter(uint msg, uint dwflag);

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }
        public struct RECT2
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Width;       // x position of lower-right corner
            public int Height;      // y position of lower-right corner
        }
        //[DllImport("setupapi.dll", SetLastError = true)]
        //public static extern int SetupDiDestroyDeviceInfoList(IntPtr lpInfoSet);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hwnd, IntPtr proccess);
        [DllImport("user32.dll")]
        public static extern IntPtr GetKeyboardLayout(uint thread);


        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
         );
        [DllImport("dwmapi.dll", PreserveSig = true)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMarInset);
        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        //public struct MARGINS                           // struct for box shadow
        //{
        //    public int leftWidth;
        //    public int rightWidth;
        //    public int topHeight;
        //    public int bottomHeight;
        //}
        public struct Margins
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
        }








        [DllImport("user32")]
        public static extern int GetWindowLong(IntPtr handle, int index);
        [DllImport("User32")]
        public static extern bool SetWindowLong(IntPtr handle, int index, int value);

        public enum GetWindowLongIndex
        {

            GWL_WNDPROC = -4,

            GWL_HINSTANCE = -6,

            GWL_HWNDPARENT = -8,

            GWL_ID = -12,

            GWL_STYLE = -16,

            GWL_EXSTYLE = -20,

            GWL_USERDATA = -21,

        }

        public enum HitTestResult
        {

            HTERROR = -2,

            HTTRANSPARENT = -1,

            HTNOWHERE = 0,

            HTCLIENT = 1,

            HTCAPTION = 2,

            HTSYSMENU = 3,

            HTSIZE = 4,

            HTGROWBOX = HTSIZE,

            HTMENU = 5,

            HTHSCROOL = 6,

            HTVSCROLL = 7,

            HTMINBUTTON = 8,

            HTREDUCE = HTMINBUTTON,

            HTMAXBUTTON = 9,

            HTZOOM = HTMAXBUTTON,

            HTLEFT = 10,

            HTRIGHT = 11,

            HTTOP = 12,

            HTTOPLEFT = 13,

            HTTOPRIGHT = 14,

            HTBOTTOM = 15,

            HTBOTTOMLEFT = 16,

            HTBOTTOMRIGHT = 17,

            HTBORDER = 18,

        }

        [Flags()]
        public enum WindowStyles
        {

            WS_OVERLAPPED = 0x00000000,

            WS_POPUP = unchecked((int)0x80000000),

            WS_CHILD = 0x40000000,

            WS_MINIMIZE = 0x20000000,

            WS_VISIBLE = 0x10000000,

            WS_DISABLED = 0x08000000,

            WS_CLIPSIBLINGS = 0x04000000,

            WS_CLIPCHILDREN = 0x02000000,

            WS_MAXIMIZE = 0x01000000,

            WS_BORDER = 0x00800000,

            WS_DLGFRAME = 0x00400000,

            WS_VSCROLL = 0x00200000,

            WS_HSCROLL = 0x00100000,

            WS_SYSMENU = 0x00080000,

            WS_THICKFRAME = 0x00040000,

            WS_GROUP = 0x00020000,

            WS_TABSTOP = 0x00010000,

            WS_MINIMIZEBOX = 0x00020000,

            WS_MAXIMIZEBOX = 0x00010000,

            WS_CAPTION = WS_BORDER | WS_DLGFRAME,

            WS_TILED = WS_OVERLAPPED,

            WS_ICONIC = WS_MINIMIZE,

            WS_SIZEBOX = WS_THICKFRAME,

            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,

            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,

            WS_CHILDWINDOW = WS_CHILD,


            WS_EX_TOOLWINDOW = 0x00000080,
            WS_EX_NOACTIVATE = 0x08000000,

            WS_EX_TRANSPARENT = 0x00000020,
        }

        public enum ResizeMode
        {

            SIZE_RESTORED = 0,

            SIZE_MINIMIZED = 1,

            SIZE_MAXIMIZED = 2,

            SIZE_MAXSHOW = 3,

            SIZE_MAXHIDE = 4,

        }

        public enum WindowMessages
        {

            WM_NCHITTEST = 0x0084,

            WM_SIZE = 0x0005,

            WM_NCPAINT = 0x0085,

            WM_QUERYENDSESSION = 0x011,

            WM_ENDSESSION = 0x16,

            MSGFLT_ALLOW = 1,
            WM_DROPFILES = 0x233,
            WM_COPYDATA = 0x004A,
            WM_COPYGLOBALDATA = 0x0049,

            WM_DPICHANGED = 0x02E0
        }



        //触发“扫描检测硬件改动”
        public const int CM_LOCATE_DEVNODE_NORMAL = 0x00000000;
        public const int CM_REENUMERATE_NORMAL = 0x00000000;
        public const int CR_SUCCESS = 0x00000000;

        [DllImport("CfgMgr32.dll", SetLastError = true)]
        public static extern int CM_Locate_DevNodeA(ref int pdnDevInst, string pDeviceID, int ulFlags);

        [DllImport("CfgMgr32.dll", SetLastError = true)]
        public static extern int CM_Reenumerate_DevNode(int dnDevInst, int ulFlags);
    }


    /// <summary>
    /// Helper class containing Gdi32 API functions
    /// </summary>
    public static class GDI32
    {

        public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
            int nWidth, int nHeight, IntPtr hObjectSource,
            int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
            int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
    }
    /// <summary>
    /// Helper class containing User32 API functions
    /// </summary>
    public static class User32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        [DllImport("User32.dll")]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);//PW_CLIENTONLY = 0x1
                                                                                    //PW_RENDERFULLCONTENT = 0x2
                                                                                    //PW_CLIENTONLY | PW_RENDERFULLCONTENT = 0x3
        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]//指定坐标处窗体句柄       
        public static extern IntPtr WindowFromPoint(System.Drawing.Point Point);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public extern static int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowTextLength(IntPtr hwnd);

    }
}

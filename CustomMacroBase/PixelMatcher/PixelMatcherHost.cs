using System;
using System.Drawing;

namespace CustomMacroBase.PixelMatcher
{
    public partial class PixelMatcherHost : PixelMatcherBase
    {
        //public PixelMatcherHost() : base() { }
    }

    public partial class PixelMatcherHost
    {
        //公开字段/属性
        public static Action<int, int> SetTargetWindowSizeEx = (a, b) =>
        {
            SetTargetWindowSizeByHandle(a, b);
        };

        public static Action<IntPtr, string, string> SetTargetWindowHandleEx = (a, b, c) =>
        {
            SetTargetWindowHandle(a, b, c);
        };
        public static Action CopyToClipboardEx = () =>
        {
            CopyToClipboard?.Invoke();
        };

        //找色
        public static int FindColor(int argb, Rectangle? rect, int? tolerance, bool flag)
        {
            if (flag) { UpdateFrames?.Invoke(); }
            return MatchColor(argb, rect, tolerance);
        }

        //找图
        public static Point? FindImage(ref Bitmap bitmap, Rectangle? rect, double? tolerance, bool flag)
        {
            if (flag) { UpdateFrames?.Invoke(); }
            return MatchImage(ref bitmap, rect, tolerance);
        }
        public static Point? FindImage(string path, Rectangle? rect, double? tolerance, bool flag)
        {
            if (flag) { UpdateFrames?.Invoke(); }
            return MatchImage(path, rect, tolerance);
        }

        //找数字
        public static string FindNumber(Rectangle rect, bool isWhiteText, double zoomratio, bool flag)
        {
            if (flag) { UpdateFrames?.Invoke(); }
            return MatchNumber(rect, isWhiteText, zoomratio);
        }

        //找字
        public static string FindText(Rectangle rect, bool isWhiteText, string language, string whitelist, double zoomratio, bool flag)
        {
            if (flag) { UpdateFrames?.Invoke(); }
            return MatchText(rect, isWhiteText, language, whitelist, zoomratio);
        }
    }
}

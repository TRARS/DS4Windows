using System;
using System.Drawing;

namespace CustomMacroBase.PixelMatcher
{
    public partial class PixelMatcherHost : PixelMatcherBase
    {
        //公开字段/属性
        public static Action<int, int> SetTargetWindowSizeEx = SetTargetWindowSize;
        public static Action<IntPtr, string, string> SetTargetWindowHandleEx = SetTargetWindowHandle;
        public static Action GetTargetWindowSnapshotEx = GetTargetWindowSnapshot;

        //Init
        public static void TryInit()
        {
            Init();
        }

        //找色
        public static int FindColor(int argb, Rectangle? rect, int? tolerance)
        {
            return MatchColor(argb, rect, tolerance);
        }

        //找图
        public static Point? FindImage(ref Bitmap bitmap, Rectangle? rect, double? tolerance)
        {
            return MatchImage(ref bitmap, rect, tolerance);
        }
        public static Point? FindImage(string path, Rectangle? rect, double? tolerance)
        {
            return MatchImage(path, rect, tolerance);
        }

        //找数字
        public static string FindNumber(Rectangle rect, bool isWhiteText, OpenCV.DeviceType deviceType, double zoomratio)
        {
            return MatchNumber(rect, isWhiteText, deviceType, zoomratio);
        }

        //找字
        public static string FindText(Rectangle rect, bool isWhiteText, OpenCV.DeviceType deviceType, OpenCV.ModelType language, double zoomratio)
        {
            return MatchText(rect, isWhiteText, deviceType, language, zoomratio);
        }
    }
}

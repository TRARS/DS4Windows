using System.Windows;

namespace CustomMacroBase.Helper.Extensions
{
    public static class PointExtensions
    {
        public static Point GetDiff(this Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }
    }
}

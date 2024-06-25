using System;
using System.Windows;

namespace CustomMacroFactory.MainView.UserControlEx.PixelPicker
{
    public record PixelDetail
    {
        public int Stride { get; set; }
        public byte[]? Values { get; set; }

        private string GetHex(int ptr)
        {
            return this.Values?[ptr].ToString("X2") ?? string.Empty;
        }

        public string GetHexRGB(int ptr)
        {
            if (this.Values is null) { return string.Empty; }

            return $"{GetHex(ptr + 2)},{GetHex(ptr + 1)},{GetHex(ptr + 0)}";
        }
    }

    public record ClickInfo
    {
        public bool IsSuccess { get; init; }
        public string Info { get; init; }
        public Point Point { get; init; }

        public ClickInfo(bool flag = false, string info = "", Point pt = default)
        {
            IsSuccess = flag;
            Info = info;
            Point = pt;
        }

        public Rect GetBoundingRect(Point otherPoint)
        {
            var x = Math.Min(Point.X, otherPoint.X);
            var y = Math.Min(Point.Y, otherPoint.Y);
            var w = Math.Abs(Point.X - otherPoint.X);
            var h = Math.Abs(Point.Y - otherPoint.Y);

            return new Rect(x, y, w, h);
        }
    }
}

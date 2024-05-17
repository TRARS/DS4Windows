using CustomMacroBase.Helper.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CustomMacroBase.CustomControlEx.PointCurveChartEx
{
    internal class cPointCurveChart_converter_size2width : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var w = ((Size)value).Width;
            var minw = double.Parse($"{parameter}");
            return w > minw ? w : minw;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cPointCurveChart_converter_size2height : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var h = ((Size)value).Height;
            var minh = double.Parse($"{parameter}");
            return h > minh ? h : minh;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class cPointCurveChart_converter_format : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var points = (ObservableCollection<List<Point>>)values[0];
                var colors = (ObservableCollection<SolidColorBrush>)values[1];
                var size = (Size)values[2];
                var zoomratio = (Point)values[3];
                var pensize = (double)values[4];
                var pointmarkers = (ObservableCollection<bool>)values[5];
                var curvetype = (CurveType)values[6];

                return CreatePathCollection(points, colors, size, zoomratio, pensize, pointmarkers, curvetype);
            }
            catch
            {
                return Binding.DoNothing;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private List<Point> FormatPointList(List<Point> points, Point maxPoint, Size containerSize, Point zoomratio)
        {
            var offset = new Point(0, 1);

            var maxX = maxPoint.X;
            var maxY = maxPoint.Y;
            var horizontalScrollBar = 4.5 + 4.5 + offset.Y;
            var w = Math.Max(containerSize.Width, 10);
            var h = Math.Max(containerSize.Height - horizontalScrollBar, 10);
            var rx = Math.Max(zoomratio.X, 0);
            var ry = Math.Max(zoomratio.Y, 0);

            var coefficientX = maxX <= w ? Math.Max(w / maxX, rx) : maxX / w;
            var coefficientY = maxY <= h ? 1.0 : Math.Max(h / maxY, ry);

            return points.Select(p => new Point((p.X * coefficientX) + offset.X, h - (p.Y * coefficientY) + offset.Y)).ToList();
        }
        private Path CreatePath(List<Point> points, SolidColorBrush? color = null, double pensize = 1.0, Point zoomratio = default, bool marker = false, CurveType curvetype = CurveType.Straight)
        {
            // 创建路径字符串
            StringBuilder data = new StringBuilder("M");

            // 创建几何组合
            GeometryGroup geometryGroup = new GeometryGroup();

            // 创建折线
            switch (curvetype)
            {
                case CurveType.Bezier:
                    {
                        // 三次方贝塞尔曲线
                        data.AppendFormat("{0},{1} C", points[0].X, points[0].Y);
                        for (int i = 1; i < points.Count; i++)
                        {
                            Point pre = new Point((points[i - 1].X + points[i].X) / 2, points[i - 1].Y);  //控制点
                            Point next = new Point((points[i - 1].X + points[i].X) / 2, points[i].Y);     //控制点
                            data.AppendFormat(" {0},{1} {2},{3} {4},{5}", pre.X, pre.Y, next.X, next.Y, points[i].X, points[i].Y);
                        }
                        geometryGroup.Children.Add(Geometry.Parse(data.ToString())); break;
                    }
                case CurveType.Straight:
                    {
                        // 折线
                        data.AppendFormat("{0},{1} L", points[0].X, points[0].Y);
                        for (int i = 1; i < points.Count; i++)
                        {
                            data.AppendFormat(" {0},{1}", points[i].X, points[i].Y);
                        }
                        geometryGroup.Children.Add(Geometry.Parse(data.ToString())); break;
                    }
            };

            // 创建圆圈
            if (marker)
            {
                var radius = pensize + 0.25;
                foreach (var point in points)
                {
                    EllipseGeometry ellipseGeometry = new EllipseGeometry(point, radius, radius);
                    geometryGroup.Children.Add(ellipseGeometry);
                }
            }

            // 创建路径
            var path = new Path
            {
                Stroke = color ?? Brushes.Yellow,
                StrokeThickness = pensize,
                Data = geometryGroup,//Geometry.Parse(data.ToString()),
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };
            return path;
        }

        private ObservableCollection<Path> CreatePathCollection(ObservableCollection<List<Point>> curves,
                                                                ObservableCollection<SolidColorBrush>? colors,
                                                                Size size,
                                                                Point zoomratio,
                                                                double pensize,
                                                                ObservableCollection<bool> markers,
                                                                CurveType curvetype)
        {
            var maxX = curves.SelectMany(v => v.Select(point => point.X)).Max();
            var maxY = curves.SelectMany(v => v.Select(point => point.Y)).Max();

            var list = new List<Path>();
            var collection = new ObservableCollection<Path>();
            {
                for (var i = 0; i < curves.Count; i++)
                {
                    var defaultColor = new SolidColorBrush(Colors.White);

                    var points = curves[i];
                    var color = (colors != null && i < colors.Count) ? colors[i] : defaultColor;
                    var marker = (markers != null && i < markers.Count) ? markers[i] : false;

                    var curve = FormatPointList(points, new Point(maxX, maxY), size, zoomratio);
                    var path = CreatePath(curve, color, pensize, zoomratio, marker, curvetype);

                    list.Add(path);
                }
            }

            collection.AddRange(list);

            return collection;
        }
    }

    internal class cPointCurveChart_converter_maxminavg : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values[0] is ObservableCollection<List<Point>> curves)
                {
                    var getmax = (Func<ObservableCollection<List<Point>>, double>?)values[1];
                    var getmin = (Func<ObservableCollection<List<Point>>, double>?)values[2];
                    var getavg = (Func<ObservableCollection<List<Point>>, double>?)values[3];

                    var max = getmax is null ? 0 : getmax.Invoke(curves);
                    var min = getmin is null ? 0 : getmin.Invoke(curves);
                    var avg = getavg is null ? 0 : getavg.Invoke(curves);

                    var sp = new StackPanel() { Orientation = Orientation.Vertical };
                    {
                        sp.Children.Add(new TextBlock() { Text = $"Max: {Math.Round(max, 2)}" });
                        sp.Children.Add(new TextBlock() { Text = $"Min: {Math.Round(min, 2)}" });
                        sp.Children.Add(new TextBlock() { Text = $"Avg: {Math.Round(avg, 2)}" });
                    }

                    return sp;
                }
            }
            catch { }

            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

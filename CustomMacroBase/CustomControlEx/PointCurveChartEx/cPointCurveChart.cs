using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomMacroBase.CustomControlEx.PointCurveChartEx
{
    public partial class cPointCurveChart : Control
    {
        static cPointCurveChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cPointCurveChart), new FrameworkPropertyMetadata(typeof(cPointCurveChart)));
        }
    }

    public partial class cPointCurveChart
    {

        /// <summary>
        /// min-size: 200x100
        /// </summary>
        public Size ChartSize
        {
            get { return (Size)GetValue(ChartSizeProperty); }
            set { SetValue(ChartSizeProperty, value); }
        }
        public static readonly DependencyProperty ChartSizeProperty = DependencyProperty.Register(
            name: "ChartSize",
            propertyType: typeof(Size),
            ownerType: typeof(cPointCurveChart),
            typeMetadata: new FrameworkPropertyMetadata(new Size(200, 100), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        /// <summary>
        /// PenSize
        /// </summary>
        public double PenSize
        {
            get { return (double)GetValue(PenSizeProperty); }
            set { SetValue(PenSizeProperty, value); }
        }
        public static readonly DependencyProperty PenSizeProperty = DependencyProperty.Register(
            name: "PenSize",
            propertyType: typeof(double),
            ownerType: typeof(cPointCurveChart),
            typeMetadata: new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        /// <summary>
        /// PointLists
        /// </summary>
        public ObservableCollection<List<Point>> PointLists
        {
            get { return (ObservableCollection<List<Point>>)GetValue(PointListsProperty); }
            set { SetValue(PointListsProperty, value); }
        }
        public static readonly DependencyProperty PointListsProperty = DependencyProperty.Register(
            name: "PointLists",
            propertyType: typeof(ObservableCollection<List<Point>>),
            ownerType: typeof(cPointCurveChart),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        /// <summary>
        /// ColorList
        /// </summary>
        public ObservableCollection<SolidColorBrush> ColorList
        {
            get { return (ObservableCollection<SolidColorBrush>)GetValue(ColorListProperty); }
            set { SetValue(ColorListProperty, value); }
        }
        public static readonly DependencyProperty ColorListProperty = DependencyProperty.Register(
            name: "ColorList",
            propertyType: typeof(ObservableCollection<SolidColorBrush>),
            ownerType: typeof(cPointCurveChart),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        /// <summary>
        /// ZoomRatio
        /// </summary>
        public Point ZoomRatio
        {
            get { return (Point)GetValue(ZoomRatioProperty); }
            set { SetValue(ZoomRatioProperty, value); }
        }
        public static readonly DependencyProperty ZoomRatioProperty = DependencyProperty.Register(
            name: "ZoomRatio",
            propertyType: typeof(Point),
            ownerType: typeof(cPointCurveChart),
            typeMetadata: new FrameworkPropertyMetadata(new Point(1, 1), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        /// <summary>
        /// PointMarkers
        /// </summary>
        public ObservableCollection<bool> PointMarkers
        {
            get { return (ObservableCollection<bool>)GetValue(PointMarkersProperty); }
            set { SetValue(PointMarkersProperty, value); }
        }
        public static readonly DependencyProperty PointMarkersProperty = DependencyProperty.Register(
            name: "PointMarkers",
            propertyType: typeof(ObservableCollection<bool>),
            ownerType: typeof(cPointCurveChart),
            typeMetadata: new FrameworkPropertyMetadata(new ObservableCollection<bool> { true }, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        /// <summary>
        /// CurveType
        /// </summary>
        public CurveType CurveType
        {
            get { return (CurveType)GetValue(CurveTypeProperty); }
            set { SetValue(CurveTypeProperty, value); }
        }
        public static readonly DependencyProperty CurveTypeProperty = DependencyProperty.Register(
            name: "CurveType",
            propertyType: typeof(CurveType),
            ownerType: typeof(cPointCurveChart),
            typeMetadata: new FrameworkPropertyMetadata(CurveType.Straight, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }

    public partial class cPointCurveChart
    {
        public Func<ObservableCollection<List<Point>>, double> MaxValueDelegate
        {
            get { return (Func<ObservableCollection<List<Point>>, double>)GetValue(MaxValueDelegateProperty); }
            set { SetValue(MaxValueDelegateProperty, value); }
        }
        public static readonly DependencyProperty MaxValueDelegateProperty = DependencyProperty.Register(
            name: "MaxValueDelegate",
            propertyType: typeof(Func<ObservableCollection<List<Point>>, double>),
            ownerType: typeof(cPointCurveChart),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Func<ObservableCollection<List<Point>>, double> MinValueDelegate
        {
            get { return (Func<ObservableCollection<List<Point>>, double>)GetValue(MinValueDelegateProperty); }
            set { SetValue(MinValueDelegateProperty, value); }
        }
        public static readonly DependencyProperty MinValueDelegateProperty = DependencyProperty.Register(
            name: "MinValueDelegate",
            propertyType: typeof(Func<ObservableCollection<List<Point>>, double>),
            ownerType: typeof(cPointCurveChart),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Func<ObservableCollection<List<Point>>, double> AvgValueDelegate
        {
            get { return (Func<ObservableCollection<List<Point>>, double>)GetValue(AvgValueDelegateProperty); }
            set { SetValue(AvgValueDelegateProperty, value); }
        }
        public static readonly DependencyProperty AvgValueDelegateProperty = DependencyProperty.Register(
            name: "AvgValueDelegate",
            propertyType: typeof(Func<ObservableCollection<List<Point>>, double>),
            ownerType: typeof(cPointCurveChart),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

﻿using System.Collections.Generic;
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

        public cPointCurveChart()
        {
            //this.ChartSize = new(400, 200);
            //this.ZoomRatio = new(10, 0);
            //this.PointLists = new()
            //{
            //    new List<Point>() { new(0, 0), new(1, 450), new(2, 40), new(3, 20), new(4, 60), new(5, 10),   new(6, 0), new(7, 150), new(8, 10), new(9, 60), new(10, 20), new(11, 100) },
            //    new List<Point>() { new(0, 0), new(1, 150), new(2, 10), new(3, 60), new(4, 20), new(5, 100),  new(6, 0), new(7, 450), new(8, 40), new(9, 20), new(10, 60), new(11, 10)},
            //};
            //this.ColorList = new()
            //{
            //    new SolidColorBrush(Colors.Crimson),
            //    new SolidColorBrush(Colors.Yellow),
            //};
        }
    }

    public partial class cPointCurveChart
    {
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
    }
}

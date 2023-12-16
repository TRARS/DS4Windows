using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomMacroBase.CustomControlEx.ValueIndicatorEx
{
    public partial class cValueIndicator : Control
    {
        static cValueIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cValueIndicator), new FrameworkPropertyMetadata(typeof(cValueIndicator)));
        }
    }

    public partial class cValueIndicator
    {
        public string PropName
        {
            get { return (string)GetValue(PropNameProperty); }
            set { SetValue(PropNameProperty, value); }
        }
        public static readonly DependencyProperty PropNameProperty = DependencyProperty.Register(
            name: "PropName",
            propertyType: typeof(string),
            ownerType: typeof(cValueIndicator),
            typeMetadata: new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public string PropValue
        {
            get { return (string)GetValue(PropValueProperty); }
            set { SetValue(PropValueProperty, value); }
        }
        public static readonly DependencyProperty PropValueProperty = DependencyProperty.Register(
            name: "PropValue",
            propertyType: typeof(string),
            ownerType: typeof(cValueIndicator),
            typeMetadata: new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public SolidColorBrush PropNameColor
        {
            get { return (SolidColorBrush)GetValue(PropNameColorProperty); }
            set { SetValue(PropNameColorProperty, value); }
        }
        public static readonly DependencyProperty PropNameColorProperty = DependencyProperty.Register(
            name: "PropNameColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cValueIndicator),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.White), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public SolidColorBrush ColonColor
        {
            get { return (SolidColorBrush)GetValue(ColonColorProperty); }
            set { SetValue(ColonColorProperty, value); }
        }
        public static readonly DependencyProperty ColonColorProperty = DependencyProperty.Register(
            name: "ColonColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cValueIndicator),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.White), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public SolidColorBrush PropValueColor
        {
            get { return (SolidColorBrush)GetValue(PropValueColorProperty); }
            set { SetValue(PropValueColorProperty, value); }
        }
        public static readonly DependencyProperty PropValueColorProperty = DependencyProperty.Register(
            name: "PropValueColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cValueIndicator),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.White), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Func<double, SolidColorBrush> PropValueColorSwitcher
        {
            get { return (Func<double, SolidColorBrush>)GetValue(PropValueColorSwitcherProperty); }
            set { SetValue(PropValueColorSwitcherProperty, value); }
        }
        public static readonly DependencyProperty PropValueColorSwitcherProperty = DependencyProperty.Register(
            name: "PropValueColorSwitcher",
            propertyType: typeof(Func<double, SolidColorBrush>),
            ownerType: typeof(cValueIndicator),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );



        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
            name: "BackgroundColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cValueIndicator),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public CornerRadius BackgroundCornerRadius
        {
            get { return (CornerRadius)GetValue(BackgroundCornerRadiusProperty); }
            set { SetValue(BackgroundCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BackgroundCornerRadiusProperty = DependencyProperty.Register(
            name: "BackgroundCornerRadius",
            propertyType: typeof(CornerRadius),
            ownerType: typeof(cValueIndicator),
            typeMetadata: new FrameworkPropertyMetadata(new CornerRadius(0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

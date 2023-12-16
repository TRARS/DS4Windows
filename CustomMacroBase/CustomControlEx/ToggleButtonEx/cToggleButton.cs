using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CustomMacroBase.CustomControlEx.ToggleButtonEx
{
    public partial class cToggleButton : ToggleButton
    {
        static cToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cToggleButton), new FrameworkPropertyMetadata(typeof(cToggleButton)));
        }

        public cToggleButton()
        {
            this.Checked += (s, e) => { CheckedAct?.Invoke(); };
            this.Unchecked += (s, e) => { UncheckedAct?.Invoke(); };
            this.Loaded += (s, e) => { LoadedAct?.Invoke(this); };
        }
    }

    public partial class cToggleButton
    {
        public Action? CheckedAct
        {
            get { return (Action)GetValue(CheckedActProperty); }
            set { SetValue(CheckedActProperty, value); }
        }
        public static readonly DependencyProperty CheckedActProperty = DependencyProperty.Register(
            name: "CheckedAct",
            propertyType: typeof(Action),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
        public Action? UncheckedAct
        {
            get { return (Action)GetValue(UncheckedActProperty); }
            set { SetValue(UncheckedActProperty, value); }
        }
        public static readonly DependencyProperty UncheckedActProperty = DependencyProperty.Register(
            name: "UncheckedAct",
            propertyType: typeof(Action),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
        public Action<cToggleButton>? LoadedAct
        {
            get { return (Action<cToggleButton>)GetValue(LoadedActProperty); }
            set { SetValue(LoadedActProperty, value); }
        }
        public static readonly DependencyProperty LoadedActProperty = DependencyProperty.Register(
            name: "LoadedAct",
            propertyType: typeof(Action<cToggleButton>),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: "Text",
            propertyType: typeof(string),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public SolidColorBrush GuideLineColor
        {
            get { return (SolidColorBrush)GetValue(GuideLineColorProperty); }
            set { SetValue(GuideLineColorProperty, value); }
        }
        public static readonly DependencyProperty GuideLineColorProperty = DependencyProperty.Register(
            name: "GuideLineColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Aqua), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
            name: "BackgroundColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            name: "Radius",
            propertyType: typeof(double),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(2d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }
        public static readonly DependencyProperty DiameterProperty = DependencyProperty.Register(
            name: "Diameter",
            propertyType: typeof(double),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(10d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public bool Enable
        {
            get { return (bool)GetValue(EnableProperty); }
            set { SetValue(EnableProperty, value); }
        }
        public static readonly DependencyProperty EnableProperty = DependencyProperty.Register(
            name: "Enable",
            propertyType: typeof(bool),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public bool DisableSliderButton
        {
            get { return (bool)GetValue(DisableSliderButtonProperty); }
            set { SetValue(DisableSliderButtonProperty, value); }
        }
        public static readonly DependencyProperty DisableSliderButtonProperty = DependencyProperty.Register(
            name: "DisableSliderButton",
            propertyType: typeof(bool),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

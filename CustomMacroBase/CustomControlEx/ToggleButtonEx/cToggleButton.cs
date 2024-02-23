using CustomMacroBase.Helper.Extensions;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
            this.Checked += (s, e) => { OnChecked(); CheckedAct?.Invoke(); };
            this.Unchecked += (s, e) => { OnUnchecked(); UncheckedAct?.Invoke(); };
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

        public Visibility GuideLineVisibility
        {
            get { return (Visibility)GetValue(GuideLineVisibilityProperty); }
            set { SetValue(GuideLineVisibilityProperty, value); }
        }
        public static readonly DependencyProperty GuideLineVisibilityProperty = DependencyProperty.Register(
            name: "GuideLineVisibility",
            propertyType: typeof(Visibility),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
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

        public Thickness DotBorderThickness
        {
            get { return (Thickness)GetValue(DotBorderThicknessProperty); }
            set { SetValue(DotBorderThicknessProperty, value); }
        }
        public static readonly DependencyProperty DotBorderThicknessProperty = DependencyProperty.Register(
            name: "DotBorderThickness",
            propertyType: typeof(Thickness),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(new Thickness(1), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public CornerRadius DotCornerRadius
        {
            get { return (CornerRadius)GetValue(DotCornerRadiusProperty); }
            set { SetValue(DotCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty DotCornerRadiusProperty = DependencyProperty.Register(
            name: "DotCornerRadius",
            propertyType: typeof(CornerRadius),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(new CornerRadius(2d), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double DotDiameter
        {
            get { return (double)GetValue(DotDiameterProperty); }
            set { SetValue(DotDiameterProperty, value); }
        }
        public static readonly DependencyProperty DotDiameterProperty = DependencyProperty.Register(
            name: "DotDiameter",
            propertyType: typeof(double),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(11d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
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

        public bool UseAlternateDotColor
        {
            get { return (bool)GetValue(UseAlternateDotColorProperty); }
            set { SetValue(UseAlternateDotColorProperty, value); }
        }
        public static readonly DependencyProperty UseAlternateDotColorProperty = DependencyProperty.Register(
            name: "UseAlternateDotColor",
            propertyType: typeof(bool),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }

    public partial class cToggleButton
    {
        public double DotTransformX
        {
            get { return (double)GetValue(DotTransformXProperty); }
            set { SetValue(DotTransformXProperty, value); }
        }
        public static readonly DependencyProperty DotTransformXProperty = DependencyProperty.Register(
            name: "DotTransformX",
            propertyType: typeof(double),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        private double distance => cToggleButton_math.Instance.WidthCalculator(DotDiameter) - DotDiameter - (DotBorderThickness.Left + DotBorderThickness.Right);

        private void OnChecked()
        {
            cToggleButton.DotTransformXProperty.SetDoubleAnimation(this, 0, distance, 100, FillBehavior.HoldEnd).Begin();
        }

        private void OnUnchecked()
        {
            cToggleButton.DotTransformXProperty.SetDoubleAnimation(this, distance, 0, 100, FillBehavior.HoldEnd).Begin();
        }
    }


}

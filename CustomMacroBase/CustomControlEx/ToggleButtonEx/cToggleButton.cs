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
            this.Checked += (s, e) => { CheckedAnimation(); };
            this.Unchecked += (s, e) => { UncheckedAnimation(); };
        }
    }

    public partial class cToggleButton
    {
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
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (d, e) =>
            {
                if (d is cToggleButton btn && e.NewValue is bool newValue)
                {
                    btn.SliderBackgroundColor = (Color)ColorConverter.ConvertFromString(newValue ? alternate_color : primary_color);
                }
            })
        );
    }

    public partial class cToggleButton
    {
        public Color SliderBackgroundColor
        {
            get { return (Color)GetValue(SliderBackgroundColorProperty); }
            set { SetValue(SliderBackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty SliderBackgroundColorProperty = DependencyProperty.Register(
            name: "SliderBackgroundColor",
            propertyType: typeof(Color),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata((Color)ColorConverter.ConvertFromString(primary_color), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double SliderSeparatorOffset
        {
            get { return (double)GetValue(SliderSeparatorOffsetProperty); }
            set { SetValue(SliderSeparatorOffsetProperty, value); }
        }
        public static readonly DependencyProperty SliderSeparatorOffsetProperty = DependencyProperty.Register(
            name: "SliderSeparatorOffset",
            propertyType: typeof(double),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

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

        public double DotDiet
        {
            get { return (double)GetValue(DotDietProperty); }
            set { SetValue(DotDietProperty, value); }
        }
        public static readonly DependencyProperty DotDietProperty = DependencyProperty.Register(
            name: "DotDiet",
            propertyType: typeof(double),
            ownerType: typeof(cToggleButton),
            typeMetadata: new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        private static IEasingFunction easing = new PowerEase() { EasingMode = EasingMode.EaseInOut, Power = 3.5 };
        private const string primary_color = "#FFC62626";
        private const string alternate_color = "#FF1394E4";
        private const double duration = 192;
        private double dot_distance => cToggleButton_math.Instance.WidthCalculator(DotDiameter) - DotDiameter - (DotBorderThickness.Left + DotBorderThickness.Right);

        private void CheckedAnimation()
        {
            this.SetDoubleAnimation(DotTransformXProperty, DotTransformX, dot_distance, duration * Factor(DotTransformX, dot_distance, dot_distance), easing).Begin();
            this.SetDoubleAnimation(SliderSeparatorOffsetProperty, SliderSeparatorOffset, 1d, duration * Factor(SliderSeparatorOffset, 1d, 1d)).Begin();
        }
        private void UncheckedAnimation()
        {
            this.SetDoubleAnimation(DotTransformXProperty, DotTransformX, 0, duration * Factor(DotTransformX, 0, dot_distance), easing).Begin();
            this.SetDoubleAnimation(SliderSeparatorOffsetProperty, SliderSeparatorOffset, 0d, duration * Factor(SliderSeparatorOffset, 0d, 1d)).Begin();
        }
        private double Factor(double from, double to, double distance)
        {
            return (Math.Abs(from - to) / distance);
        }
    }
}

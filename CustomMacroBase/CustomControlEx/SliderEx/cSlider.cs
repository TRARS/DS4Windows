using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.SliderEx
{
    public partial class cSlider : Slider
    {
        static cSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cSlider), new FrameworkPropertyMetadata(typeof(cSlider)));
        }
    }

    public partial class cSlider
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: "Text",
            propertyType: typeof(string),
            ownerType: typeof(cSlider),
            typeMetadata: new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double DefalutValue
        {
            get { return (double)GetValue(DefalutValueProperty); }
            set { SetValue(DefalutValueProperty, value); }
        }
        public static readonly DependencyProperty DefalutValueProperty = DependencyProperty.Register(
            name: "DefalutValue",
            propertyType: typeof(double),
            ownerType: typeof(cSlider),
            typeMetadata: new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }

    public partial class cSlider
    {
        public bool IsInitialized { get; set; } = false;
    }
}

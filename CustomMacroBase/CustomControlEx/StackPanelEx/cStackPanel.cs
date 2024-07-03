using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomMacroBase.CustomControlEx.StackPanelEx
{
    public partial class cStackPanel : ItemsControl
    {
        private ObservableCollection<UIElement> _children = new();
        public ObservableCollection<UIElement> Children { get => _children; }

        static cStackPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cStackPanel), new FrameworkPropertyMetadata(typeof(cStackPanel)));
        }

        public cStackPanel()
        {
            base.ItemsSource = _children;
        }
    }

    public partial class cStackPanel
    {
        /// <summary>
        /// <para>This property is invalid. Use the <see cref="cStackPanel.Children" /> property instead of <see cref="ItemsControl.ItemsSource"/> property.</para>
        /// </summary>
        public new IEnumerable ItemsSource
        {
            get { return base.ItemsSource; }
        }

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            name: "Orientation",
            propertyType: typeof(Orientation),
            ownerType: typeof(cStackPanel),
            typeMetadata: new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public SolidColorBrush GuideLineColor
        {
            get { return (SolidColorBrush)GetValue(GuideLineColorProperty); }
            set { SetValue(GuideLineColorProperty, value); }
        }
        public static readonly DependencyProperty GuideLineColorProperty = DependencyProperty.Register(
            name: "GuideLineColor",
            propertyType: typeof(SolidColorBrush),
            ownerType: typeof(cStackPanel),
            typeMetadata: new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Aqua), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

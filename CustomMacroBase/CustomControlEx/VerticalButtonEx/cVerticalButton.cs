using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CustomMacroBase.CustomControlEx.VerticalButtonEx
{
    public partial class cVerticalButton : Button
    {
        static cVerticalButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cVerticalButton), new FrameworkPropertyMetadata(typeof(cVerticalButton)));
        }
    }

    //主要属性
    public partial class cVerticalButton
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: "Text",
            propertyType: typeof(string),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }

    //作为图标按钮时的属性
    public partial class cVerticalButton
    {
        public ObservableCollection<UIElement> Icon
        {
            get { return (ObservableCollection<UIElement>)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            name: "Icon",
            propertyType: typeof(ObservableCollection<UIElement>),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(new ObservableCollection<UIElement>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public bool IconOnly
        {
            get { return (bool)GetValue(IconOnlyProperty); }
            set { SetValue(IconOnlyProperty, value); }
        }
        public static readonly DependencyProperty IconOnlyProperty = DependencyProperty.Register(
            name: "IconOnly",
            propertyType: typeof(bool),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public object RightClickContent
        {
            get { return (object)GetValue(RightClickContentProperty); }
            set { SetValue(RightClickContentProperty, value); }
        }
        public static readonly DependencyProperty RightClickContentProperty = DependencyProperty.Register(
            name: "RightClickContent",
            propertyType: typeof(object),
            ownerType: typeof(cVerticalButton),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }
}

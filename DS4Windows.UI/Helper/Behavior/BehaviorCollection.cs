using DS4WinWPF.UI.Helper.AttachedProperty;
using DS4WinWPF.UI.Helper.Base;
using DS4WinWPF.UI.Helper.Extensions;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DS4WinWPF.UI.Helper.Behavior
{
    // Border点击坐标
    public class GetBorderClickPosBehavior : Behavior<Border>
    {
        public object Target
        {
            get { return (object)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            name: "Target",
            propertyType: typeof(object),
            ownerType: typeof(GetBorderClickPosBehavior),
            typeMetadata: new FrameworkPropertyMetadata(null)
        );


        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += MouseLeftButtonDown;
        }
        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= MouseLeftButtonDown;
        }

        private void MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point relativePoint = Mouse.GetPosition((Border)sender);

            if (Target is FrameworkElement element)
            {
                if (element.FindVisualChildByName<Border>("border") is Border border)
                {
                    if (UIElementHelper.GetUniversalObjectAttached(border) is UniversalObjectViewModel uobj)
                    {
                        uobj.ClickPos = relativePoint;
                    }
                }
            }
        }
    }

    // 
}

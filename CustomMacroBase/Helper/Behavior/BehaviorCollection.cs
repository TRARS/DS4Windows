using CustomMacroBase.CustomControlEx.GridEx;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomMacroBase.Helper.Behavior
{
    // 拿点击坐标
    public class GetMouseLeftButtonDownPosBehavior : Behavior<Border>
    {
        public object Target
        {
            get { return (object)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            name: "Target",
            propertyType: typeof(object),
            ownerType: typeof(GetMouseLeftButtonDownPosBehavior),
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

            if (Target is not null)
            {
                ((dynamic)Target).ClickPos = relativePoint;
            }
        }
    }

    // 左键按下时设置ContextMenu.PlacementTarget
    public class GetMouseLeftButtonDownPlacementTargetBehavior : Behavior<cGrid>
    {
        public object Target
        {
            get { return (object)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            name: "Target",
            propertyType: typeof(object),
            ownerType: typeof(GetMouseLeftButtonDownPlacementTargetBehavior),
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
            if ((FrameworkElement?)sender is cGrid host)
            {
                host.ContextMenu.PlacementTarget = host;
                //host.ContextMenu.IsOpen = true;//直接展开会闪，故需延迟展开
            }
        }
    }
}

using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace CustomMacroBase.Helper.Extensions
{
    public static partial class DependencyPropertyExtensions
    {
        public static Storyboard SetDoubleAnimation(this DependencyProperty target, UIElement host, double from, double to, double duration = 100, FillBehavior behavior = FillBehavior.HoldEnd)
        {
            // 创建DoubleAnimation对象
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from; // 起始值
            animation.To = to; // 终止值
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(duration)); // 动画持续时间
            animation.FillBehavior = behavior;

            // 创建Storyboard对象
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation); // 将动画添加到Storyboard中

            // 将动画关联到指定依赖属性
            Storyboard.SetTarget(animation, host);
            Storyboard.SetTargetProperty(animation, new PropertyPath(target));

            return storyboard;
        }
    }
}

using DS4WinWPF.UI.Helper.Base;
using System.Windows;

namespace DS4WinWPF.UI.Helper.AttachedProperty
{
    // 通用储存可观察属性的对象
    public partial class UIElementHelper
    {
        public static readonly DependencyProperty UniversalObjectAttachedProperty = DependencyProperty.RegisterAttached(
            name: "UniversalObjectAttached",
            propertyType: typeof(UniversalObjectViewModel),
            ownerType: typeof(UIElementHelper),
            defaultMetadata: new FrameworkPropertyMetadata(null)
        );
        public static UniversalObjectViewModel? GetUniversalObjectAttached(DependencyObject target)
        {
            return (UniversalObjectViewModel?)target.GetValue(UniversalObjectAttachedProperty);
        }
        public static void SetUniversalObjectAttached(DependencyObject target, UniversalObjectViewModel? value)
        {
            target.SetValue(UniversalObjectAttachedProperty, value);
        }
    }
}

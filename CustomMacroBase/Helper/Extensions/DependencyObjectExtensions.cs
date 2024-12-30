using System.Windows;
using System.Windows.Media;

namespace CustomMacroBase.Helper.Extensions
{
    public static class DependencyObjectExtensions
    {
        public static T FindVisualAncestor<T>(this DependencyObject target) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(target);
            if (parent == null)
            {
                return null;
            }
            if (parent is T)
            {
                return (T)parent;
            }
            return parent.FindVisualAncestor<T>();
        }
        public static T FindVisualChild<T>(this DependencyObject target) where T : DependencyObject
        {
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(target) - 1; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(target, i);

                if (child != null && child is T)
                    return child as T;
                else
                {
                    T childOfChildren = FindVisualChild<T>(child);
                    if (childOfChildren != null)
                        return childOfChildren;
                }
            }
            return null;
        }

        public static T GetParentObject<T>(this DependencyObject target) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(target);
            while (parent != null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        public static T FindVisualAncestorByName<T>(this DependencyObject target, string target_name, int depth = 1) where T : DependencyObject
        {
            if (depth-- <= 0)
                return null;

            if (target is T && ((FrameworkElement)target).Name == target_name) { return (T)target; }

            var parent = VisualTreeHelper.GetParent(target);
            if (parent == null)
            {
                return null;
            }
            if (parent is T && ((FrameworkElement)parent).Name == target_name)
            {
                return (T)parent;
            }
            return parent.FindVisualAncestorByName<T>(target_name, depth);
        }
        public static T FindVisualChildByName<T>(this DependencyObject target, string target_name, int depth = 1) where T : DependencyObject
        {
            if (depth-- <= 0)
                return null;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(target) - 1; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(target, i);

                if (child != null && child is T && ((FrameworkElement)child).Name == target_name)
                    return child as T;
                else
                {
                    T childOfChildren = FindVisualChildByName<T>(child, target_name, depth);
                    if (childOfChildren != null)
                        return childOfChildren;
                }
            }
            return null;
        }
    }
}

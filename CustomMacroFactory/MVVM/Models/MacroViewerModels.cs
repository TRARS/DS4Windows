using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CustomMacroFactory.MVVM.Models
{
    public partial class MenuItem_model : ObservableObject
    {
        [ObservableProperty]
        private string _text;

        [ObservableProperty]
        private IRelayCommand _command;
    }

    class PartCreator<T>
    {
        private ObservableCollection<T> collection = new();

        public PartCreator(Action<ObservableCollection<T>> itemsSource)
        {
            itemsSource.Invoke(collection);
        }

        /// <summary>
        /// 创建一个垂直排列的UI元素集合
        /// </summary>
        public UIElement GetVerticalContent()
        {
            return GetContent(Orientation.Vertical);
        }

        /// <summary>
        /// 创建一个水平排列的UI元素集合
        /// </summary>
        public UIElement GetHorizontalContent()
        {
            return GetContent(Orientation.Horizontal);
        }

        private UIElement GetContent(Orientation orientation)
        {
            // 创建 ItemsControl 实例
            var itemsControl = new ItemsControl();

            // 创建 ControlTemplate 并设置 TargetType
            var template = new ControlTemplate(typeof(ItemsControl));

            // 创建 FrameworkElementFactory 用于 Border
            var borderFactory = new FrameworkElementFactory(typeof(Border));

            // 创建 FrameworkElementFactory 用于 StackPanel 并设置 IsItemsHost 属性
            var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            {
                stackPanelFactory.SetValue(StackPanel.OrientationProperty, orientation);
                stackPanelFactory.SetValue(StackPanel.IsItemsHostProperty, true);
            }

            // 将 StackPanel 添加到 Border 内
            borderFactory.AppendChild(stackPanelFactory);

            // 将 Border 作为模板的 VisualTree
            template.VisualTree = borderFactory;

            // 将模板应用于 ItemsControl
            itemsControl.Template = template;

            // 设置数据源
            itemsControl.ItemsSource = collection;

            return itemsControl;
        }
    }
}

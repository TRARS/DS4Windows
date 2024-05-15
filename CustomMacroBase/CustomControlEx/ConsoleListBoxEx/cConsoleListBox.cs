using CustomMacroBase.Helper;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomMacroBase.CustomControlEx.ConsoleListBoxEx
{
    sealed class LimitedSizeObservableCollection<T> : ObservableCollection<T>
    {
        private readonly object lockObject = new object();
        private readonly int capacity = 11;

        public new void Add(T item)
        {
            lock (lockObject)
            {
                if (Count >= capacity)
                {
                    base.RemoveAt(0);
                }
                base.Add(item);
            }
        }
    }

    public class cConsoleListBox : ListBox
    {
        private readonly LimitedSizeObservableCollection<string> _list = new();

        static cConsoleListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cConsoleListBox), new FrameworkPropertyMetadata(typeof(cConsoleListBox)));
        }
        public cConsoleListBox()
        {
            this.Foreground = new SolidColorBrush(Colors.White);
            this.Background = new SolidColorBrush(Colors.Transparent);
            this.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00ABADB3"));
            ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Disabled);
            ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Disabled);

            this.ItemsSource = _list;

            Mediator.Instance.Register(MessageType.PrintNewMessage, para =>
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    _list.Add($"{DateTime.Now.ToString("HH:mm:ss")} -> {(string)para}");
                });
            });
            Mediator.Instance.Register(MessageType.PrintCleanup, _ =>
            {
                this.Dispatcher.BeginInvoke(() => { _list.Clear(); });
            });
        }
    }
}

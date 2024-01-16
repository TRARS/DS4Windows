using System;
using System.Collections.ObjectModel;

namespace DS4WinWPF.UI.Helper.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void ForEach<T>(this ObservableCollection<T> collection, Action<T> action)
        {
            foreach (T item in collection)
            {
                action(item);
            }
        }

        public static T? LastItem<T>(this ObservableCollection<T> collection)
        {
            var count = collection.Count;
            return count > 0 ? collection[count - 1] : default;
        }

        public static void RemoveLastItem<T>(this ObservableCollection<T> collection)
        {
            var count = collection.Count;
            if (count > 0)
            {
                collection.RemoveAt(count - 1);
            }
        }


    }
}

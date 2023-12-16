using System;
using System.Collections.ObjectModel;

namespace CustomMacroBase.Helper.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void ForEach<T>(this ObservableCollection<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }
    }
}

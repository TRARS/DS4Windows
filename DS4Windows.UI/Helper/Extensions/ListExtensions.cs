using System.Collections.Generic;
using System.Linq;

namespace DS4WinWPF.UI.Helper.Extensions
{
    public static class ListExtensions
    {
        public static List<T> ReArrange<T>(this List<T> collection, int[] orderArray)
        {
            List<T> reorderedList = collection.Zip(orderArray, (value, index) => new { value, index })
                                              .OrderBy(pair => pair.index)
                                              .Select(pair => pair.value)
                                              .ToList();
            return reorderedList;
        }

        public static T? LastItem<T>(this List<T> collection)
        {
            var count = collection.Count;
            return count > 0 ? collection[count - 1] : default;
        }

        public static void RemoveLastItem<T>(this List<T> collection)
        {
            var count = collection.Count;
            if (count > 0)
            {
                collection.RemoveAt(count - 1);
            }
        }
    }
}

using System.Collections.Generic;

namespace Our.Umbraco.Hider.Core.Extensions
{
    internal static class ListExtensions
    {
        public static void AddRangeUnique<T>(this ICollection<T> self, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (!self.Contains(item))
                {
                    self.Add(item);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paging_Sorting_Filtering
{
    public static class MyTools
    {
        static private bool _isAscending = true;
        public static IEnumerable<T> Sorting<T>(IEnumerable<T> source, string sortBy)
        {
            var propertyInfo = typeof(T).GetProperty(sortBy);

            if (propertyInfo is null)
            {
                throw new MissingMemberException($"No property called {sortBy} was found");
            }
            if (_isAscending)
            {
                source = source.OrderBy(s => propertyInfo.GetValue(s));
                _isAscending = false;
            }
            else
            {
                source = source.OrderByDescending(s => propertyInfo.GetValue(s));
                _isAscending = true;
            }
            return source;
        }

        public static IEnumerable<T> Paging<T>(this IEnumerable<T> source, int page, byte pageSize)
        {
            int totalSkipped = page * pageSize - pageSize;
            int to = page * pageSize;
            var pagedElements = source.TakeRange(totalSkipped, to);

            return pagedElements;
        }

        private static IEnumerable<T> TakeRange<T>(this IEnumerable<T> source, int from, int to)
        {
            List<T> list = new();

            for (int i = from; i < to; i++)
            {
                T element = source.ElementAtOrDefault(i);
                if (element is null)
                {
                    break;
                }
                list.Add(element);
            }
            return list;
        }
    }
}

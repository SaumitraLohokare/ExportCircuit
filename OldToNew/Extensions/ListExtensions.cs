using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileConverter.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Moves an item at the end of the list.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="item">List item.</param>
        public static void MoveToBottom<T>(this List<T> list, T item)
        {
            if (!list.Contains(item)) return;
            list.Remove(item);
            list.Add(item);
        }

        /// <summary>
        /// Moves an item to the top of the list.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="item">List item.</param>
        public static void MoveToTop<T>(this List<T> list, T item)
        {
            if (!list.Contains(item)) return;
            list.Remove(item);
            list.Insert(0, item);
        }

    }
}

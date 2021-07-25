using System;
using System.Collections.Generic;

namespace HaoEncoding.Extensions
{
    public static class ArrayExtension
    {
        //public static int CopyToCount<T>(this IList<T> source, IList<T> array, int index = 0)
        //{
        //    if (source is null)
        //        throw new ArgumentNullException(nameof(source));
        //    if (array is null)
        //        throw new ArgumentNullException(nameof(array));

        //    if (source.Count + index > array.Count)
        //        throw new ArgumentOutOfRangeException(nameof(source));

        //    var count = 0;
        //    while (count < source.Count)
        //    {

        //        array[index + count] = source[count];
        //        ++count;
        //    }

        //    return count;
        //}

        public static int CopyToCount<T>(this IList<T> source, Span<T> destination, int index = 0)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (source.Count + index > destination.Length)
                throw new ArgumentOutOfRangeException(nameof(source));

            var count = 0;
            while (count < source.Count)
            {

                destination[index + count] = source[count];
                ++count;
            }

            return count;
        }
    }
}

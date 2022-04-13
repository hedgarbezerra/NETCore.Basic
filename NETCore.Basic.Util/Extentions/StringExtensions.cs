using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Util.Extentions
{
    public static class StringExtensions
    {
        public static string Mask(this string value, int from, int to, char substitution)
        {
            if (value.Length < to || value.Length < from)
                return value;

            return string.Create(value.Length, value, (span, str) =>
            {
                value.AsSpan().CopyTo(span);
                span[from..to].Fill(substitution);
            });
        }
        public static void ForEachNext<T>(this IList<T> collection, Action<T, T> func)
        {
            for (int i = 0; i < collection.Count - 1; i++)
                func(collection[i], collection[i + 1]);

            //example
            // new List<string>().ForEachNext((curr,next) => curr + next));
        }


    }
}

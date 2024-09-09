using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class EnumerableHelper
{
    public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
    {
        foreach (var item in source) action(item);
    }
}

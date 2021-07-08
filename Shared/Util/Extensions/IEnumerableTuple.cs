using System;
using System.Collections.Generic;
using System.Linq;

namespace ParseTreeVisualizer.Util {
    public static class IEnumerableTupleExtensions {
        public static string Joined<T1, T2>(this IEnumerable<(T1, T2)> src, string delimiter, Func<T1, T2, int, string> selector) =>
            src.Joined(delimiter, (x, index) => selector(x.Item1, x.Item2, index));

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<(TKey, TValue)> src) => src.ToDictionary(t => t.Item1, t => t.Item2);

        public static void Add<T1, T2>(this List<(T1, T2)> lst, T1 item1, T2 item2) => lst.Add((item1, item2));

        public static IEnumerable<TResult> SelectT<T1, T2, TResult>(this IEnumerable<ValueTuple<T1, T2>> src, Func<T1, T2, TResult> selector) =>
            src.Select(x => selector(x.Item1, x.Item2));
        public static IEnumerable<TResult> SelectT<T1, T2, T3, TResult>(this IEnumerable<ValueTuple<T1, T2, T3>> src, Func<T1, T2, T3, TResult> selector) =>
            src.Select(x => selector(x.Item1, x.Item2, x.Item3));
        public static IEnumerable<TResult> SelectT<T1, T2, T3, T4, TResult>(this IEnumerable<ValueTuple<T1, T2, T3, T4>> src, Func<T1, T2, T3, T4, TResult> selector) =>
            src.Select(x => selector(x.Item1, x.Item2, x.Item3, x.Item4));
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer.Util {
    public static class IEnumerableTExtensions {
        public static Dictionary<TKey, TValue> ToDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> src, Func<TSource, (TKey, TValue)> selector) => src.Select(selector).ToDictionary(x => x.Item1, x => x.Item2);

        public static void AddRangeTo<T>(this IEnumerable<T> src, ICollection<T> dest) => dest.AddRange(src);

        public static string Joined<T>(this IEnumerable<T> source, string delimiter = ",", Func<T, string> selector = null) {
            if (source == null) { return ""; }
            if (selector == null) { return string.Join(delimiter, source); }
            return string.Join(delimiter, source.Select(selector));
        }
        public static string Joined<T>(this IEnumerable<T> source, string delimiter, Func<T, int, string> selector) {
            if (source == null) { return ""; }
            if (selector == null) { return string.Join(delimiter, source); }
            return string.Join(delimiter, source.Select(selector));
        }

        public static bool None<T>(this IEnumerable<T> src, Func<T, bool> predicate = null) {
            if (predicate != null) { return !src.Any(predicate); }
            return !src.Any();
        }

        /// <summary>
        /// Returns an element If the sequence has exactly one element; otherwise returns the default of T
        /// (unlike the standard SingleOrDefault, which will throw an exception on multiple elements).
        /// </summary>
        public static T OneOrDefault<T>(this IEnumerable<T> src, Func<T,bool> predicate = null) {
            if (src == null) { return default; }
            if (predicate != null) { src = src.Where(predicate); }
            T ret = default;
            var counter = 0;
            foreach (var item in src.Take(2)) {
                if (counter == 1) { return default; }
                ret = item;
                counter += 1;
            }
            return ret;
        }

        public static IEnumerable<T> Select<T>(this IEnumerable<T> src) => src.Select(x => x);

        public static int? MinOrDefault<T>(this IEnumerable<T> src, Func<T, int> selector) {
            if (src.None()) { return null; }
            return src.Min(selector);
        }
        public static int? MaxOrDefault<T>(this IEnumerable<T> src, Func<T, int> selector) {
            if (src.None()) { return null; }
            return src.Max(selector);
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> src, Action<T> action) {
            foreach (var item in src) {
                action(item);
            }
            return src;
        }
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> src, Action<T, int> action) {
            var current = 0;
            foreach (var item in src) {
                action(item, current);
                current += 1;
            }
            return src;
        }

        public static IEnumerable<(T1, T2)> WhereT<T1, T2>(this IEnumerable<(T1, T2)> src, Func<T1, T2, bool> predicate) => src.Where(x => predicate(x.Item1, x.Item2));
    }
}

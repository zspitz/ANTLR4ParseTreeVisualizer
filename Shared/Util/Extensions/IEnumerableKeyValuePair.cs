using System;
using System.Collections.Generic;
using System.Linq;

namespace ParseTreeVisualizer.Util {
    public static class IEnumerableKeyValuePairExtensions {
        public static IEnumerable<TResult> SelectKVP<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dict, Func<TKey, TValue, TResult> selector) => dict.Select(kvp => selector(kvp.Key, kvp.Value));
    }
}

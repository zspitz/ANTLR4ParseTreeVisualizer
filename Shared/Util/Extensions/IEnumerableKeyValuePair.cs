using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer.Util {
    public static class IEnumerableKeyValuePairExtensions {
        public static IEnumerable<TResult> SelectKVP<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dict, Func<TKey, TValue, TResult> selector) => dict.Select(kvp => selector(kvp.Key, kvp.Value));
    }
}

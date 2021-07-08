using System.Collections.Generic;

namespace ParseTreeVisualizer.Util {
    public static class KeyValuePairTKeyTValueExtensions {
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value) {
            key = kvp.Key;
            value = kvp.Value;
        }
    }
}

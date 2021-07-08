using System.Collections.Generic;

namespace ParseTreeVisualizer.Util {
    public static class HashSetTExtensions {
        public static bool AddRemove<T>(this HashSet<T> src, bool add, T element) {
            if (add) {
                return src.Add(element);
            } else {
                return src.Remove(element);
            }
        }
    }
}

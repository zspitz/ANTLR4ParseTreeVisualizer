using System.Collections.Generic;

namespace ParseTreeVisualizer.Util {
    public static class ICollectionTExtensions {
        public static void AddRange<T>(this ICollection<T> dest, IEnumerable<T> toAdd) {
            if (toAdd is null) { return; }
            foreach (var x in toAdd) {
                dest.Add(x);
            }
        }
    }
}

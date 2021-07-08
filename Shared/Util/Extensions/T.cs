using System.Collections.Generic;
using System.Linq;

namespace ParseTreeVisualizer.Util {
    public static class TExtensions {
        public static bool In<T>(this T val, IEnumerable<T>? vals) => vals is { } && vals.Contains(val);
        public static bool In<T>(this T val, params T[] vals) => vals.Contains(val);
        public static bool In<T>(this T val, HashSet<T>? hs) => hs is { } &&  hs.Contains(val);
        public static bool NotIn<T>(this T val, IEnumerable<T>? vals) => vals is { } && !vals.Contains(val);
        public static bool NotIn<T>(this T val, params T[] vals) => !vals.Contains(val);
        public static bool NotIn<T>(this T val, HashSet<T>? hs) => hs is { } && !hs.Contains(val);
    }
}

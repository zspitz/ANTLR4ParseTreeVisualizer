using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer.Util {
    public static class TExtensions {
        public static bool In<T>(this T val, IEnumerable<T> vals) => vals.Contains(val);
        public static bool In<T>(this T val, params T[] vals) => vals.Contains(val);
        public static bool In<T>(this T val, HashSet<T> hs) => hs.Contains(val);
        public static bool NotIn<T>(this T val, IEnumerable<T> vals) => !vals.Contains(val);
        public static bool NotIn<T>(this T val, params T[] vals) => !vals.Contains(val);
        public static bool NotIn<T>(this T val, HashSet<T> hs) => !hs.Contains(val);
    }
}

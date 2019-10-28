using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

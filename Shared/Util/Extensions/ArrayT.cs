using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer.Util {
    public static class ArrayTExtensions {
        public static bool TryGetValue<T>(this T[] arr, int index, out T result) {
            result = default;
            if (arr == null) { return false; }
            if (index < 0 || index >= arr.Length) { return false; }
            result = arr[index];
            return true;
        }
    }
}

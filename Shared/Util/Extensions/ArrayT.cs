using System.Diagnostics.CodeAnalysis;

namespace ParseTreeVisualizer.Util {
    public static class ArrayTExtensions {
        [return: MaybeNull]
        public static bool TryGetValue<T>(this T[] arr, int index, out T result) {
            result = default!;
            if (arr == null) { return false; }
            if (index < 0 || index >= arr.Length) { return false; }
            result = arr[index];
            return true;
        }
    }
}

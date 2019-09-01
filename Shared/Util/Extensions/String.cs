using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer.Util {
    public static class StringExtensions {
        public static bool IsNullOrWhitespace(this string s) => string.IsNullOrWhiteSpace(s);
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer.Util {
    public static class StringExtensions {
        public static bool IsNullOrWhitespace(this string s) => string.IsNullOrWhiteSpace(s);

        // https://stackoverflow.com/a/14502246/111794
        public static string ToCSharpLiteral(this string input, bool withQuotationMarks = true) {
            var literal =
                withQuotationMarks ? new StringBuilder("\"", input.Length + 2) :
                new StringBuilder(input.Length);
            foreach (var c in input) {
                switch (c) {
                    case '\'': literal.Append(@"\'"); break;
                    case '\"': literal.Append("\\\""); break;
                    case '\\': literal.Append(@"\\"); break;
                    case '\0': literal.Append(@"\0"); break;
                    case '\a': literal.Append(@"\a"); break;
                    case '\b': literal.Append(@"\b"); break;
                    case '\f': literal.Append(@"\f"); break;
                    case '\n': literal.Append(@"\n"); break;
                    case '\r': literal.Append(@"\r"); break;
                    case '\t': literal.Append(@"\t"); break;
                    case '\v': literal.Append(@"\v"); break;
                    default:
                        if (char.GetUnicodeCategory(c) != UnicodeCategory.Control) {
                            literal.Append(c);
                        } else {
                            literal.Append(@"\u");
                            literal.Append($"{(ushort)c:x4}");
                        }
                        break;
                }
            }
            if (withQuotationMarks) { literal.Append("\""); }
            return literal.ToString();
        }

        public static bool ContainsAny(this string s, params string[] testStrings) => testStrings.Any(x => s.Contains(x));
        public static bool StartsWithAny(this string s, params string[] testStrings) => testStrings.Any(x => s.StartsWith(x, StringComparison.InvariantCulture));
    }
}

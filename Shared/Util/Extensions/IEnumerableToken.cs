
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer.Util {
    public static class IEnumerableTokenExtensions {
        public static int MaxTokenTypeID(this IEnumerable<Token> src) => src.MaxOrDefault(x => x.TokenTypeID) ?? 0;

        public static (int start, int end)? SelectionCharSpan(this IEnumerable<Selectable<Token>> tokens) {
            int? startChar = null;
            int? endChar = null;

            // TODO replace with tokens.Aggregate?

            foreach (var vm in tokens) {
                var token = vm.Model;
                if (vm.IsSelected) {
                    startChar =
                        startChar == null ?
                            token.Span.start :
                            Math.Min(startChar.Value, token.Span.start);
                    endChar =
                        endChar == null ?
                            token.Span.stop :
                            Math.Max(endChar.Value, token.Span.stop);
                }
            }

            if (startChar.HasValue && endChar.HasValue) { return (startChar.Value, endChar.Value); }
            return null;
        }
    }
}

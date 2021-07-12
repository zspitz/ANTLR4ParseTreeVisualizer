using ParseTreeVisualizer.Serialization;
using System;
using System.Collections.Generic;
using ZSpitz.Util.Wpf;

namespace ParseTreeVisualizer {
    public static class Extensions {
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

            return 
                startChar.HasValue && endChar.HasValue ?
                    (startChar.Value, endChar.Value) : 
                    null;
        }

    }
}

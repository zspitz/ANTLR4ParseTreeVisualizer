using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer.Util {
    public static class IParseTreeExtensions {
        public static IEnumerable<IParseTree> Children(this IParseTree tree) {
            for (int i = 0; i < tree.ChildCount; i++) {
                yield return tree.GetChild(i);
            }
        }

        public static IEnumerable<IParseTree> Descendants(this IParseTree tree) {
            for (int i = 0; i < tree.ChildCount; i++) {
                var child = tree.GetChild(i);
                yield return child;
                foreach (var descendant in child.Descendants()) {
                    yield return descendant;
                }
            }
        }

        public static string GetPositionedText(this IParseTree tree, char filler = ' ') {
            var sb = new StringBuilder();
            foreach (var descendant in tree.Descendants().OfType<TerminalNodeImpl>()) {
                var fillerCharCount = descendant.Payload.StartIndex - sb.Length;
                if (fillerCharCount > 0) {
                    sb.Append(filler, fillerCharCount);
                }
                sb.Append(descendant.Payload.Text);
            }
            return sb.ToString();
        }
    }
}

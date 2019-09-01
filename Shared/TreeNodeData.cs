using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParseTreeVisualizer.Util;
using System.Runtime.Serialization;
using Antlr4.Runtime;

namespace ParseTreeVisualizer {
    public enum TreeNodeType {
        RuleContext,
        Token,
        Error
    }

    [Serializable]
    public class TreeNodeData {
        public string Name { get; }
        public IDictionary<string, string> Properties { get; }
        public IList<TreeNodeData> Children { get; }
        public (int start, int Length) Span { get; }
        public TreeNodeType? NodeType { get; }
        public TreeNodeData(IParseTree tree, VisualizerData visualizerData) {
            if (tree is RuleContext) {
                NodeType= TreeNodeType.RuleContext;
            } else if (tree is TerminalNodeImpl terminalNode) {
                var vm = new TerminalNodeImplVM(terminalNode);
                visualizerData.TerminalNodes.Add(vm);
                NodeType = TreeNodeType.Token;
            } 

            var t = tree.GetType();
            Name = t.Name;
            Properties = t.GetProperties().OrderBy(x => x.Name).ToDictionary(x => {
                object value = null;
                try {
                    value = x.GetValue(tree);
                } catch (Exception e) {
                    value = $"<{e.GetType()}: {e.Message}";
                }
                return (x.Name, value?.ToString());
            });
            Children = tree.Children().Select(x => new TreeNodeData(x, visualizerData)).ToList();
        }
    }
}

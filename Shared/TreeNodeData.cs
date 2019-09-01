using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParseTreeVisualizer.Util;
using System.Runtime.Serialization;
using Antlr4.Runtime;
using static ParseTreeVisualizer.Util.Functions;

namespace ParseTreeVisualizer {
    public enum TreeNodeType {
        RuleContext,
        Token,
        Error
    }

    [Serializable]
    public class TreeNodeData {
        public string Caption { get; }
        public IList<PropertyValueVM> Properties { get; }
        public IList<TreeNodeData> Children { get; }
        public (int start, int Length) Span { get; }
        public TreeNodeType? NodeType { get; }
        public TreeNodeData(IParseTree tree, VisualizerData visualizerData) {
            var t = tree.GetType();

            if (tree is RuleContext) {
                NodeType= TreeNodeType.RuleContext;
                Caption = t.Name;
            } else if (tree is TerminalNodeImpl terminalNode) {
                var vm = new TerminalNodeImplVM(terminalNode);
                visualizerData.TerminalNodes.Add(vm);
                NodeType = TreeNodeType.Token;
                Caption = terminalNode.Payload.Text.ToCSharpLiteral();
            } 

            Properties = t.GetProperties().OrderBy(x => x.Name).Select(prp => new PropertyValueVM(tree, prp)).ToList();
            Children = tree.Children().Select(x => new TreeNodeData(x, visualizerData)).ToList();
        }
    }
}

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer {
    [Serializable]
    public class VisualizerData {
        public string Source { get; }
        public TreeNodeData Root { get; }
        public IList<TerminalNodeImplVM> TerminalNodes { get; } = new List<TerminalNodeImplVM>();
        public VisualizerData(IParseTree tree) {
            Source = tree.GetText();
            Root = new TreeNodeData(tree, this);
        }
    }
}

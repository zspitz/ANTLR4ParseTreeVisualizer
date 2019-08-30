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
    public class VisualizerData : VisualizerDataNode {
        public string Source { get; }
        public VisualizerData(IParseTree tree) : base(tree) {
            Source = tree.GetText();
        }
    }
}

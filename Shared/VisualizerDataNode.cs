using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParseTreeVisualizer.Util;
using System.Runtime.Serialization;

namespace ParseTreeVisualizer {
    [Serializable]
    public class VisualizerDataNode {
        public string Name { get; }
        public IDictionary<string, string> Properties { get; }
        public IList<VisualizerDataNode> Children { get; }
        public (int start, int Length) Span { get; }
        public VisualizerDataNode(IParseTree tree) {
            var t = tree.GetType();
            Name = t.Name;
            Properties = t.GetProperties().OrderBy(x => x.Name).ToDictionary(x => {
                string value;
                try {
                    value = x.GetValue(tree).ToString();
                } catch (Exception e) {
                    value = $"<{e.GetType()}: {e.Message}";
                }
                return (x.Name, value);
            });
            Children = tree.Children().Select(x => new VisualizerDataNode(x)).ToList();
        }
    }
}

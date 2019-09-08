using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ParseTreeVisualizer.Util;

namespace ParseTreeVisualizer {
    [Serializable]
    public class VisualizerData {
        public string Source { get; }
        public TreeNodeVM Root { get; }
        public IList<TerminalNodeImplVM> TerminalNodes { get; } = new List<TerminalNodeImplVM>();
        public VisualizerConfig Config { get; }
        public VisualizerData(IParseTree tree, VisualizerConfig config) {
            Source = tree.GetText();
            Config = config;

            IVocabulary vocabulary = null ;
            string[] ruleNames = null;

            if (!config.SelectedParserName.IsNullOrWhitespace()) {
                var parserType = AppDomain.CurrentDomain.GetAssemblies().Select(x =>x.GetType(config.SelectedParserName)).FirstOrDefault(x => x!=null);
                vocabulary = parserType.GetField("DefaultVocabulary").GetValue(null) as IVocabulary;
                ruleNames = parserType.GetField("ruleNames").GetValue(null) as string[];
            }
            Root = new TreeNodeVM(tree, this, vocabulary, ruleNames);
        }

        [NonSerialized]
        private Dictionary<int, TerminalNodeImplVM> nodesByIndex;
        public Dictionary<int, TerminalNodeImplVM> NodesByIndex {
            get {
                if (nodesByIndex == null) { nodesByIndex = TerminalNodes.ToDictionary(x => x.Index); }
                return nodesByIndex;
            }
        }
    }
}

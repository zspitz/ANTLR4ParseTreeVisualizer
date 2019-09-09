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

            IVocabulary vocabulary = null;
            string[] ruleNames = null;

            if (!config.SelectedParserName.IsNullOrWhitespace()) {
                var parserType = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetType(config.SelectedParserName)).FirstOrDefault(x => x != null);
                vocabulary = parserType.GetField("DefaultVocabulary").GetValue(null) as IVocabulary;
                TokenTypeMapping = Enumerable.Range(0, vocabulary.MaxTokenType).ToDictionary(x => (x, vocabulary.GetSymbolicName(x)));

                ruleNames = parserType.GetField("ruleNames").GetValue(null) as string[];
            }

            Root = new TreeNodeVM(tree, this, TokenTypeMapping, ruleNames);

            if (TokenTypeMapping == null) {
                TokenTypeMapping = TerminalNodes
                    .Select(x=>x.TokenType)
                    .Distinct()
                    .ToDictionary(x => (int.Parse(x), x));
            }

            LoadAvailables();
        }

        public Dictionary<int,string> TokenTypeMapping { get; }

        [NonSerialized]
        private Dictionary<int, TerminalNodeImplVM> nodesByIndex;
        public Dictionary<int, TerminalNodeImplVM> NodesByIndex {
            get {
                if (nodesByIndex == null) { nodesByIndex = TerminalNodes.ToDictionary(x => x.Index); }
                return nodesByIndex;
            }
        }

        // Ideally we would use ImmutableList for this, but ImmutableList isn't serializable -- https://github.com/dotnet/corefx/issues/1272
        // the following should not be saved to disk
        public List<ClassInfo> AvailableParsers { get; private set; } = new List<ClassInfo>();
        public List<ClassInfo> AvailableLexers { get; private set; } = new List<ClassInfo>();
        public List<ClassInfo> ParserRuleContexts { get; private set; } = new List<ClassInfo>();
        public HashSet<TokenTypeVM> AvailableTokens { get; } = new HashSet<TokenTypeVM>();

        // We need to load information from three different kinds of types: parsers, lexers, and parser context rules

        private void LoadAvailables() {
            AvailableParsers = new List<ClassInfo>();
            AvailableLexers = new List<ClassInfo>();
            ParserRuleContexts = new List<ClassInfo>();

            var baseTypes = new[] { typeof(Parser), typeof(Lexer), typeof(ParserRuleContext) };
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => !x.IsAbstract);
            foreach (var t in types) {
                var dest =
                    t.InheritsFromOrImplements<Parser>() ? AvailableParsers :
                    t.InheritsFromOrImplements<Lexer>() ? AvailableLexers :
                    t.InheritsFromOrImplements<ParserRuleContext>() ? ParserRuleContexts :
                    null;
                dest?.Add(new ClassInfo(t));
            }

            Comparison<ClassInfo> comparison = (x, y) => string.Compare(x.Name, y.Name);
            AvailableParsers.Sort(comparison);
            AvailableLexers.Sort(comparison);
            ParserRuleContexts.Sort(comparison);

            Config.SelectedParserName = fixList(AvailableParsers, Config.SelectedParserName);
            Config.SelectedLexerName = fixList(AvailableLexers, Config.SelectedLexerName);

        }

        private string fixList(List<ClassInfo> lst, string selected) {
            lst.Insert(0, ClassInfo.None);
            var selected1 = selected; // because selected is a ref
            if (lst.None(x => x.FullName == selected1)) {
                selected = null;
            }
            if (selected.IsNullOrWhitespace()) {
                selected = (
                    lst.OneOrDefault(x => x.Antlr != "Runtime") ??
                    lst.OneOrDefault()
                )?.FullName;
            }
            return selected;
        }
    }
}

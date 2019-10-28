using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ParseTreeVisualizer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Linq.Enumerable;
using static ParseTreeVisualizer.Util.Functions;

namespace ParseTreeVisualizer {
    [Serializable]
    public class VisualizerData {
        public string Source { get; }
        public Config Config { get; }
        public ParseTreeNode Root { get; }
        public List<Token> Tokens { get; } = new List<Token>();
        public int SourceOffset { get; }
        public List<ClassInfo> AvailableParsers { get; } = new List<ClassInfo>();
        public List<ClassInfo> AvailableLexers { get; } = new List<ClassInfo>();
        public List<string> AssemblyLoadErrors { get; } = new List<string>();
        public Dictionary<int, string> TokenTypeMapping { get; private set; }
        public List<ClassInfo> UsedRuleContexts { get; }
        public bool CanSelectLexer { get; }
        public bool CanSelectParser { get; }

        [NonSerialized]
        private Type[] types;
        private T createInstance<T>(string typename, object[] args = null) =>
            (T)Activator.CreateInstance(types.Single(x => x.FullName == typename), args);

        public VisualizerData(object o, Config config) {
            if (config is null) { throw new ArgumentNullException(nameof(config)); }

            Config = config;

            {
                var baseTypes = new[] { typeof(Parser), typeof(Lexer) };
                types = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => x != GetType().Assembly)
                    .SelectMany(x => {
                        try {
                            return x.GetTypes();
#pragma warning disable CA1031 // Do not catch general exception types
                        } catch {
#pragma warning restore CA1031 // Do not catch general exception types
                            AssemblyLoadErrors.Add(x.FullName);
                            return Empty<Type>();
                        }
                    })
                    .Where(x => !x.IsAbstract)
                    .ToArray();
                foreach (var t in types) {
                    var toAdd =
                        t.InheritsFromOrImplements<Parser>() ? new ClassInfo(t, null, null, true) :
                        t.InheritsFromOrImplements<Lexer>() ? new ClassInfo(t) :
                        null;
                    var dest =
                        t.InheritsFromOrImplements<Parser>() ? AvailableParsers :
                        t.InheritsFromOrImplements<Lexer>() ? AvailableLexers :
                        null;
                    dest?.Add(toAdd);
                }

                Config.SelectedParserName = checkSelection(AvailableParsers, Config.SelectedParserName);
                Config.SelectedLexerName = checkSelection(AvailableLexers, Config.SelectedLexerName);
            }

            string source = null;
            BufferedTokenStream tokenStream = null;
            IParseTree tree = null;
            IVocabulary vocabulary = null;

            // these three are mutually exclusive
            switch (o) {
                case string source1:
                    source = source1;
                    CanSelectLexer = true;
                    CanSelectParser = true;
                    Source = source;
                    break;
                case BufferedTokenStream tokenStream1:
                    tokenStream = tokenStream1;
                    CanSelectParser = true;
                    Source = tokenStream.TokenSource.InputStream.ToString();
                    break;
                case IParseTree tree1:
                    tree = tree1;
                    Source = tree.GetText();
                    break;
                default:
                    throw new ArgumentException("Unhandled type");
            }

            if (source is { } && !Config.SelectedLexerName.IsNullOrWhitespace()) {
                var input = new AntlrInputStream(source);
                var lexer = createInstance<Lexer>(Config.SelectedLexerName, new[] { input });
                tokenStream = new CommonTokenStream(lexer);
                vocabulary = lexer.Vocabulary;
            }

            if (
                tokenStream is { } &&
                !Config.SelectedParserName.IsNullOrWhitespace() &&
                !Config.ParseTokensWithRule.IsNullOrWhitespace()
            ) {
                var parser = createInstance<Parser>(Config.SelectedParserName, new[] { tokenStream });
                tree = (IParseTree)parser.GetType().GetMethod(Config.ParseTokensWithRule).Invoke(parser, Array.Empty<object>());
                vocabulary = parser.Vocabulary;
            }

            if (tree is null && tokenStream is null) {
                return;
            }

            if (tree is null) {
                tokenStream.Fill();
                tokenStream.GetTokens()
                    .Select(token => new Token(token, getTokenTypeMapping()))
                    .Where(token => token.ShowToken(config))
                    .AddRangeTo(Tokens);
                return;
            }

            if (!config.RootNodePath.IsNullOrWhitespace()) {
                var pathParts = config.RootNodePath.Split('.').Select(x =>
                    int.TryParse(x, out var ret) ?
                        ret :
                        -1
                ).ToArray();
                foreach (var pathPart in pathParts) {
                    var nextTree = tree.GetChild(pathPart);
                    if (nextTree == null) {
                        break;
                    }
                    tree = tree.GetChild(pathPart);
                }
            }

            var parserType = tree.GetType().DeclaringType;
            Config.SelectedParserName = parserType.FullName;
            if (vocabulary is null) {
                vocabulary = parserType.GetField("DefaultVocabulary").GetValue(null) as IVocabulary;
            }

            var tokenTypeMapping = getTokenTypeMapping();

            var ruleNames = parserType.GetField("ruleNames").GetValue(null) as string[];
            var rulenameMapping = new Dictionary<Type, (string name, int index)>();
            var actualRoot = new ParseTreeNode(tree, Tokens, ruleNames, tokenTypeMapping, config, rulenameMapping, Config.RootNodePath);
            if (config.RootNodePath.IsNullOrWhitespace()) {
                Root = actualRoot;
            } else {
                Root = ParseTreeNode.GetPlaceholder(actualRoot);
                SourceOffset = actualRoot.CharSpan.startChar;
            }

            UsedRuleContexts = rulenameMapping.Keys
                .Select(x => {
                    rulenameMapping.TryGetValue(x, out var y);
                    return new ClassInfo(x, y.name, y.index);
                })
                .OrderBy(x => x.Name)
                .ToList();

            Dictionary<int, string> getTokenTypeMapping() {
                if (vocabulary == null) { return null; }
#if ANTLR_RUNTIME
                int maxTokenType = vocabulary.MaxTokenType;
#else
                int maxTokenType = (vocabulary as Vocabulary).getMaxTokenType();
#endif
                TokenTypeMapping = Range(1, maxTokenType).ToDictionary(x => (x, vocabulary.GetSymbolicName(x)));
                return TokenTypeMapping;
            }

        }

        private string checkSelection(List<ClassInfo> lst, string selected) {
            if (lst.None(x => x.FullName == selected)) {
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

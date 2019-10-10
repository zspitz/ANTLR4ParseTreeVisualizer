using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ParseTreeVisualizer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Linq.Enumerable;

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
        public Dictionary<int, string> TokenTypeMapping { get; }
        public List<ClassInfo> UsedRuleContexts { get; }

        public VisualizerData(object o, Config config) {
            if (!(o is IParseTree tree)) { throw new ArgumentException("Unhandled type."); }

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

            Config = config;
            Source = tree.GetText();

            var parserType = tree.GetType().DeclaringType;
            var vocabulary = parserType.GetField("DefaultVocabulary").GetValue(null) as IVocabulary;
            var tokenTypeMapping = Range(1, vocabulary.MaxTokenType).ToDictionary(x => (x, vocabulary.GetSymbolicName(x)));

            var ruleNames = parserType.GetField("ruleNames").GetValue(null) as string[];

            var rulenameMapping = new Dictionary<Type, (string name, int index)>();
            var actualRoot = new ParseTreeNode(tree, Tokens, ruleNames, tokenTypeMapping, config, rulenameMapping, Config.RootNodePath);
            if (config.RootNodePath.IsNullOrWhitespace()) {
                Root = actualRoot;
            } else {
                Root = ParseTreeNode.GetPlaceholder(actualRoot);
                SourceOffset = actualRoot.CharSpan.startChar;
            }

            #region Load debuggee state
            TokenTypeMapping = tokenTypeMapping;

            UsedRuleContexts = rulenameMapping.Keys
                .Select(x => {
                    rulenameMapping.TryGetValue(x, out var y);
                    return new ClassInfo(x, y.name, y.index);
                })
                .OrderBy(x => x.Name)
                .ToList();

            {
                // load available parsers and lexers

                var baseTypes = new[] { typeof(Parser), typeof(Lexer) };
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => x != GetType().Assembly)
                    .SelectMany(x => {
                        try {
                            return x.GetTypes();
                        } catch {
                            AssemblyLoadErrors.Add(x.FullName);
                            return Empty<Type>();
                        }
                    })
                    .Where(x => !x.IsAbstract);
                foreach (var t in types) {
                    var dest =
                        t.InheritsFromOrImplements<Parser>() ? AvailableParsers :
                        t.InheritsFromOrImplements<Lexer>() ? AvailableLexers :
                        null;
                    dest?.Add(new ClassInfo(t));
                }

                Config.SelectedParserName = checkSelection(AvailableParsers, Config.SelectedParserName);
                Config.SelectedLexerName = checkSelection(AvailableLexers, Config.SelectedLexerName);
            }

            #endregion
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

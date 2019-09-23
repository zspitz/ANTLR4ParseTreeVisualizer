using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParseTreeVisualizer.Util;
using static System.Linq.Enumerable;

namespace ParseTreeVisualizer.ViewModels {
    [Serializable]
    public class TreeVisualizer {
        public string Source { get; }
        public Config Config { get; }
        public ParseTreeNode Root { get; }
        public TokenList Tokens { get; } = new TokenList();
        public int SourceOffset { get; }

        public TreeVisualizer(IParseTree tree, Config config) {
            if (!config.RootNodePath.IsNullOrWhitespace()) {
                var pathParts = config.RootNodePath.Split('.').Select(x =>
                    int.TryParse(x, out var ret) ?
                        ret :
                        -1
                ).ToArray();
                foreach (var pathPart in pathParts) {
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
            var actualRoot = new ParseTreeNode(tree, Tokens, ruleNames, tokenTypeMapping, config, rulenameMapping, null);
            if (config.RootNodePath.IsNullOrWhitespace()) {
                Root = actualRoot;
            } else {
                Root = ParseTreeNode.GetPlaceholder(actualRoot);
                SourceOffset = actualRoot.CharSpan.startChar;
            }

            #region Load debuggee state

            if (tokenTypeMapping == null) {
                tokenTypeMapping = Range(1, Tokens.MaxTokenTypeID()).ToDictionary(x => (x, x.ToString()));
            }
            config.TokenTypeMapping = tokenTypeMapping;

            config.UsedRuleContexts = rulenameMapping.Keys
                .Select(x => {
                    rulenameMapping.TryGetValue(x, out var y);
                    return new ParseRuleContextInfo(x, y.name, y.index);
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

                Comparison<ClassInfo> comparison = (x, y) => string.Compare(x.Name, y.Name);
                AvailableParsers.Sort(comparison);
                AvailableLexers.Sort(comparison);

                Config.SelectedParserName = fixList(AvailableParsers, Config.SelectedParserName);
                Config.SelectedLexerName = fixList(AvailableLexers, Config.SelectedLexerName);
            }

            #endregion
        }

        #region Debuggee state
        public List<ClassInfo> AvailableParsers { get; } = new List<ClassInfo>();
        public List<ClassInfo> AvailableLexers { get; } = new List<ClassInfo>();
        public List<string> AssemblyLoadErrors { get; } = new List<string>();
        #endregion

        private string fixList(List<ClassInfo> lst, string selected) {
            lst.Insert(0, ClassInfo.None);
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

﻿using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ParseTreeVisualizer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Linq.Enumerable;

namespace ParseTreeVisualizer {
    [Serializable]
    public class VisualizerData {
        public string Source { get; }
        public Config Config { get; }
        public ParseTreeNode? Root { get; }
        public List<Token>? Tokens { get; }
        public int SourceOffset { get; }
        public List<ClassInfo> AvailableParsers { get; } = new List<ClassInfo>();
        public List<ClassInfo> AvailableLexers { get; } = new List<ClassInfo>();
        public List<string> AssemblyLoadErrors { get; } = new List<string>();
        public Dictionary<int, string>? TokenTypeMapping { get; private set; }
        public List<ClassInfo>? UsedRuleContexts { get; }
        public bool CanSelectLexer { get; }
        public bool CanSelectParser { get; }

        private static readonly string[] ignnoreLoadErrors = new[] {
            "Microsoft.Xaml.Behaviors"
        };

        public VisualizerData(object o, Config config) {
            if (config is null) { throw new ArgumentNullException(nameof(config)); }

            Config = config;

            Type[] types;
            T createInstance<T>(string typename, object[]? args = null) =>
                (T)Activator.CreateInstance(types.Single(x => x.FullName == typename), args);

            {
                var baseTypes = new[] { typeof(Parser), typeof(Lexer) };
                types = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => x != GetType().Assembly)
                    .SelectMany(x => {
                        var ret = Empty<Type>();
                        if (!x.FullName.StartsWithAny(ignnoreLoadErrors)) {
                            try {
                                ret = x.GetTypes();
#pragma warning disable CA1031 // Do not catch general exception types
                            } catch {
#pragma warning restore CA1031 // Do not catch general exception types
                                AssemblyLoadErrors.Add(x.FullName);
                            }
                        }
                        return ret;
                    })
                    .Where(x => !x.IsAbstract)
                    .ToArray();
                foreach (var t in types) {
                    if (t.InheritsFromOrImplements<Parser>()) {
                        AvailableParsers.Add(new ClassInfo(t, null, null, true));
                    } else if (t.InheritsFromOrImplements<Lexer>()) {
                        AvailableLexers.Add(new ClassInfo(t));
                    }
                }

                Config.SelectedLexerName = checkSelection(AvailableLexers, Config.SelectedLexerName);
                Config.SelectedParserName = checkSelection(AvailableParsers, Config.SelectedParserName);
                if (!Config.SelectedParserName.IsNullOrWhitespace()) {
                    var parserInfo = AvailableParsers.OneOrDefault(x => x.FullName == Config.SelectedParserName);
                    if (parserInfo is null) {
                        Config.ParseTokensWithRule = null;
                    } else {
                        if (Config.ParseTokensWithRule.NotIn(parserInfo.MethodNames)) {
                            Config.ParseTokensWithRule = null;
                        }
                        if (Config.ParseTokensWithRule is null) {
                            Config.ParseTokensWithRule = parserInfo.MethodNames.OneOrDefault();
                        }
                    }
                }
            }

            string? source = null;
            BufferedTokenStream? tokenStream = null;
            IParseTree? tree = null;
            IVocabulary? vocabulary = null;

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
                    Source = tree.GetPositionedText();
                    break;
                default:
                    throw new ArgumentException("Unhandled type");
            }

            if (source != null && !Config.SelectedLexerName.IsNullOrWhitespace()) {
                var input = new AntlrInputStream(source);
                var lexer = createInstance<Lexer>(Config.SelectedLexerName!, new[] { input });
                tokenStream = new CommonTokenStream(lexer);
                vocabulary = lexer.Vocabulary;
            }

            if (
                tokenStream != null &&
                !Config.SelectedParserName.IsNullOrWhitespace() &&
                !Config.ParseTokensWithRule.IsNullOrWhitespace()
            ) {
                var parser = createInstance<Parser>(Config.SelectedParserName, new[] { tokenStream });
                tree = (IParseTree)parser.GetType().GetMethod(Config.ParseTokensWithRule)?.Invoke(parser, Array.Empty<object>())!;
                vocabulary = parser.Vocabulary;
            }

            if (tree is null && tokenStream is null) {
                return;
            }

            if (tree is null) {
                tokenStream!.Fill();
                Tokens = tokenStream.GetTokens()
                    .Select(token => new Token(token, getTokenTypeMapping()))
                    .Where(token => token.ShowToken(config))
                    .ToList();
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

            var tokenTypeMapping = getTokenTypeMapping()!;

            var ruleNames = (parserType.GetField("ruleNames").GetValue(null) as string[])!;
            var rulenameMapping = new Dictionary<Type, (string? name, int? index)>();
            Tokens = new List<Token>();
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

            Dictionary<int, string>? getTokenTypeMapping() {
                if (vocabulary is null) { return null; }
#if ANTLR_RUNTIME
                int maxTokenType = vocabulary.MaxTokenType;
#else
                int maxTokenType = (vocabulary as Vocabulary)!.getMaxTokenType();
#endif
                TokenTypeMapping = Range(1, maxTokenType).ToDictionary(x => (x, vocabulary.GetSymbolicName(x)));
                return TokenTypeMapping;
            }

        }

        private string? checkSelection(List<ClassInfo> lst, string? selected) {
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

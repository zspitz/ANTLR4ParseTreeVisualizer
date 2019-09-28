using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ParseTreeVisualizer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer {
    [Serializable]
    public class ParseTreeNode {
        public static ParseTreeNode GetPlaceholder(ParseTreeNode actualRoot) => new ParseTreeNode {
            Caption = "(parent nodes)",
            NodeType = TreeNodeType.Placeholder,
            Children = new List<ParseTreeNode> { actualRoot },
            CharSpan = actualRoot.CharSpan
        };

        public string Caption { get; private set; }
        public List<PropertyValue> Properties { get; }
        public List<ParseTreeNode> Children { get; private set; }
        public (int startTokenIndex, int endTokenIndex) TokenSpan { get; }
        public (int startChar, int endChar) CharSpan { get; private set; }
        public TreeNodeType? NodeType { get; private set; }
        public FilterStates? FilterState { get; }
        public string Path { get; }

        private ParseTreeNode() { }
        public ParseTreeNode(IParseTree tree, List<Token> tokens, string[] ruleNames, Dictionary<int,string> tokenTypeMapping, Config config, Dictionary<Type, (string caption, int index)> ruleMapping, string path) {
            var type = tree.GetType();

            if (tree is ParserRuleContext ruleContext) {
                NodeType = TreeNodeType.RuleContext;

                string caption = type.Name;
                if (!ruleMapping.TryGetValue(type, out var x)) {
                    var ruleIndex = (int)(type.GetProperty("RuleIndex")?.GetValue(tree) ?? -1);
                    if (ruleNames.TryGetValue(ruleIndex, out caption)) {
                        ruleMapping[type] = (caption, ruleIndex);
                    } else {
                        caption = type.Name;
                        ruleMapping[type] = (null, -1);
                    }
                } else {
                    caption = x.caption;
                }

                Caption = caption;
                CharSpan = (ruleContext.Start.StartIndex, ruleContext.Stop.StopIndex);
            } else if (tree is TerminalNodeImpl terminalNode) {
                var token = new Token(terminalNode, tokenTypeMapping);

                if (token.IsError) {
                    Caption = token.Text;
                    NodeType = TreeNodeType.ErrorToken;
                } else {
                    Caption = $"\"{token.Text}\"";
                    NodeType = token.IsWhitespace ? TreeNodeType.WhitespaceToken : TreeNodeType.Token;
                }
                CharSpan = token.Span;

                // should the token be added to the token list?
                var addToken = false;

                if (!token.IsError && !token.IsWhitespace) {
                    addToken = config.ShowTextTokens;
                } else {
                    addToken =
                        (token.IsError ? config.ShowErrorTokens : true) &&
                        (token.IsWhitespace ? config.ShowWhitespaceTokens : true);
                }

                addToken &= config.SelectedTokenTypes.None() || token.TokenTypeID.In(config.SelectedTokenTypes);

                if (addToken) {
                    tokens.Add(token);
                }
            }

            Path = path;
            var pathDelimiter = path.IsNullOrWhitespace() ? "" : ".";
            Properties = type.GetProperties().OrderBy(x => x.Name).Select(prp => new PropertyValue(tree, prp)).ToList();
            Children = tree.Children()
                .Select((x, index) => new ParseTreeNode(x, tokens, ruleNames, tokenTypeMapping, config, ruleMapping, $"{path}{pathDelimiter}{index}"))
                .Where(x => x.FilterState != FilterStates.NotMatched) // intentionally doesn't exclude null
                .ToList();
            TokenSpan = (tree.SourceInterval.a, tree.SourceInterval.b);

            var matched = true;
            if (config.HasTreeFilter()) {
                if (NodeType == TreeNodeType.ErrorToken) {
                    matched = config.ShowTreeErrorTokens;
                } else if (NodeType == TreeNodeType.RuleContext) {
                    matched = config.ShowRuleContextNodes;
                    if (config.SelectedRuleContexts?.Any() ?? false) {
                        matched = matched && type.FullName.In(config.SelectedRuleContexts);
                    }
                } else if (NodeType == TreeNodeType.WhitespaceToken) {
                    matched = config.ShowTreeWhitespaceTokens;
                } else { // assumes NodeType == TreeNodeType.Token
                    matched = config.ShowTreeTextTokens;
                }

                if (matched) {
                    FilterState = FilterStates.Matched;
                } else if (Children.Any(x => x.FilterState.In(FilterStates.Matched, FilterStates.DescendantMatched))) {
                    FilterState = FilterStates.DescendantMatched;
                } else {
                    FilterState = FilterStates.NotMatched;
                }
            }

            var toPromote = Children
                .Select((child, index) => (grandchild: child.Children.OneOrDefault(x => x.FilterState.In(FilterStates.Matched, FilterStates.DescendantMatched)), index))
                .WhereT((grandchild, index) => grandchild != null)
                .ToList();
            foreach (var (grandchild, index) in toPromote) {
                Children[index] = grandchild;
            }
        }

        public string Stringify(int indentLevel = 0) {
            var ret = new string(' ', indentLevel * 4) + Caption;
            foreach (var child in Children) {
                ret += "\n" + child.Stringify(indentLevel + 1);
            }
            return ret;
        }
    }
}

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ParseTreeVisualizer.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParseTreeVisualizer {
    [Serializable]
    public class ParseTreeNode {
        public static ParseTreeNode GetPlaceholder(ParseTreeNode actualRoot) {
            if (actualRoot is null) { throw new ArgumentNullException(nameof(actualRoot)); }
            return new ParseTreeNode("(parent nodes)", TreeNodeType.Placeholder, new List<ParseTreeNode> { actualRoot }, actualRoot.CharSpan);
        }

        public string? Caption { get; private set; }
        public List<PropertyValue>? Properties { get; }
        public List<ParseTreeNode> Children { get; private set; }
        public (int startTokenIndex, int endTokenIndex) TokenSpan { get; }
        public (int startChar, int endChar) CharSpan { get; private set; }
        public TreeNodeType? NodeType { get; private set; }
        public FilterStates? FilterState { get; }
        public string? Path { get; }

        private ParseTreeNode(string caption, TreeNodeType nodeType, List<ParseTreeNode> children, (int startChar, int endChar) charSpan) {
            Caption = caption;
            NodeType = nodeType;
            Children = children;
            CharSpan = charSpan;
        }
        public ParseTreeNode(IParseTree tree, List<Token> tokens, string[] ruleNames, Dictionary<int, string> tokenTypeMapping, Config config, Dictionary<Type, (string? caption, int? index)> ruleMapping, string? path) {
            if (tree is null) { throw new ArgumentNullException(nameof(tree)); }
            if (ruleMapping is null) { throw new ArgumentNullException(nameof(ruleMapping)); }
            if (tokens is null) { throw new ArgumentNullException(nameof(tokens)); }
            if (config is null) { throw new ArgumentNullException(nameof(config)); }

            var type = tree.GetType();

            if (tree is ParserRuleContext ruleContext) {
                NodeType = TreeNodeType.RuleContext;

                string? caption = type.Name;
                if (!ruleMapping.TryGetValue(type, out var x)) {
                    var ruleIndex = (int)(type.GetProperty("RuleIndex")?.GetValue(tree) ?? -1);
                    if (ruleNames.TryGetValue(ruleIndex, out caption)) {
                        ruleMapping[type] = (caption, ruleIndex);
                    } else {
                        caption = type.Name;
                        ruleMapping[type] = (null, null);
                    }
                } else {
                    caption = x.caption;
                }

                Caption = caption;
                CharSpan = (ruleContext.Start.StartIndex, ruleContext.Stop?.StopIndex ?? int.MaxValue);
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

                if (token.ShowToken(config)) {
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
                    if (config.SelectedRuleContexts.Any()) {
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

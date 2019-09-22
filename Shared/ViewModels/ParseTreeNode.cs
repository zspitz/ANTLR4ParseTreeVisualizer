using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ParseTreeVisualizer.Util;

namespace ParseTreeVisualizer.ViewModels {
    public enum TreeNodeType {
        RuleContext,
        Token,
        ErrorToken,
        WhitespaceToken
    }

    public enum FilterState {
        NotMatched,
        Matched,
        DescendantMatched
    }

    [Serializable]
    public class ParseTreeNode : INotifyPropertyChanged {
        public string Caption { get; }
        public List<PropertyValue> Properties { get; }
        public List<ParseTreeNode> Children { get; }
        public (int startTokenIndex, int endTokenIndex) TokenSpan { get; }
        public (int startChar, int endChar) CharSpan { get; private set; }
        public TreeNodeType? NodeType { get; }
        public FilterState? FilterState { get; }

        public ParseTreeNode(IParseTree tree, TokenList tokens, string[] ruleNames, Dictionary<int, string> tokenTypeMapping, Config config, Dictionary<Type, (string caption, int index)> ruleMapping) {
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

            Properties = type.GetProperties().OrderBy(x => x.Name).Select(prp => new PropertyValue(tree, prp)).ToList();
            Children = tree.Children()
                .Select(x => new ParseTreeNode(x, tokens, ruleNames, tokenTypeMapping, config, ruleMapping))
                .Where(x => x.FilterState != ViewModels.FilterState.NotMatched) // intentionally doesn't exclude null
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
                    FilterState = ViewModels.FilterState.Matched;
                } else if (Children.Any(x => x.FilterState.In(ViewModels.FilterState.Matched, ViewModels.FilterState.DescendantMatched))) {
                    FilterState = ViewModels.FilterState.DescendantMatched;
                } else {
                    FilterState = ViewModels.FilterState.NotMatched;
                }
            }

            var toPromote = Children
                .Select((child, index) => (grandchild: child.Children.OneOrDefault(x => x.FilterState.In(ViewModels.FilterState.Matched, ViewModels.FilterState.DescendantMatched)), index))
                .WhereT((grandchild, index) => grandchild != null)
                .ToList();
            foreach (var (grandchild, index) in toPromote) {
                Children[index] = grandchild;
            }

            // trying and failing to find the CharSpan for error nodes from all the previous nodes, to the end of the token
            //var errorChildren = Children.Where(x => x.NodeType == TreeNodeType.Error && x.CharSpan == (-1, -1));
            //foreach (var error in errorChildren) {
            //    var index = Children.IndexOf(error);
            //    if (index>0) {
            //        var previousChildren = Children.Take(index - 1).ToList();
            //        error.CharSpan = (previousChildren.Min(x => x.CharSpan.endChar) + 1, (tree as ParserRuleContext).Start.InputStream.Size);
            //    }
            //}
        }

        public string Stringify(int indentLevel=0) {
            var ret = new string(' ', indentLevel*4) + Caption;
            foreach (var child in Children) {
                ret += "\n" + child.Stringify(indentLevel + 1);
            }
            return ret;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NonSerialized]
        private bool isSelected;
        public bool IsSelected {
            get => isSelected;
            set => this.NotifyChanged(ref isSelected, value, args => PropertyChanged?.Invoke(this, args));
        }

        public void ClearSelection() {
            IsSelected = false;
            foreach (var child in Children) {
                child.ClearSelection();
            }
        }

        [NonSerialized]
        private bool isExpanded;
        public bool IsExpanded {
            get => isExpanded;
            set => this.NotifyChanged(ref isExpanded, value, args => PropertyChanged?.Invoke(this, args));
        }
        public void SetSubtreeExpanded(bool expand) {
            IsExpanded = expand;
            Children.ForEach(x => x.SetSubtreeExpanded(expand));
        }
    }
}

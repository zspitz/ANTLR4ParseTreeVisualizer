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
        Error
    }

    [Serializable]
    public class ParseTreeNode : INotifyPropertyChanged {
        public string Caption { get; }
        public List<PropertyValue> Properties { get; }
        public List<ParseTreeNode> Children { get; }
        public (int startTokenIndex, int endTokenIndex) TokenSpan { get; }
        public (int startChar, int endChar) CharSpan { get; }
        public TreeNodeType? NodeType { get; }

        public ParseTreeNode(IParseTree tree, TokenList tokens, string[] ruleNames, Dictionary<int,string> tokenTypeMapping, Config config) {
            var type = tree.GetType();

            if (tree is RuleContext) {
                NodeType = TreeNodeType.RuleContext;
                Caption = type.Name;
                if (ruleNames != null) {
                    var ruleIndex = type.GetProperty("RuleIndex")?.GetValue(tree) as int?;
                    if (ruleIndex.HasValue && ruleIndex >= 0 && ruleIndex < ruleNames.Length) {
                        Caption = ruleNames[ruleIndex.Value];
                    }
                }
            } else if (tree is TerminalNodeImpl terminalNode) {
                var token = new Token(terminalNode, tokenTypeMapping);

                if (token.IsError) {
                    Caption = token.Text;
                    NodeType = TreeNodeType.Error;
                } else {
                    Caption = $"\"{token.Text}\"";
                    NodeType = TreeNodeType.Token;
                }
                CharSpan = token.Span;

                // should the token be added to the token list?
                var addToken = false;

                if (token.IsError) {
                    addToken = config.ShowErrorTokens;
                } else if (token.Text.IsNullOrWhitespace()) {
                    addToken = config.ShowWhitespaceTokens;
                } else { //token is not whitespace
                    addToken = config.ShowTextTokens;
                }

                addToken &= config.SelectedTokenTypes.None() || token.TokenTypeID.In(config.SelectedTokenTypes);

                if (addToken) {
                    tokens.Add(token);
                }
            }

            Properties = type.GetProperties().OrderBy(x => x.Name).Select(prp => new PropertyValue(tree, prp)).ToList();
            Children = tree.Children()
                .Select(x => new ParseTreeNode(x, tokens, ruleNames, tokenTypeMapping, config))
                .ToList();
            TokenSpan = (tree.SourceInterval.a, tree.SourceInterval.b);

            if (Children.Any()) {
                CharSpan = (
                    Children.First().CharSpan.startChar,
                    Children.Last().CharSpan.endChar
                );
            }
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
    }
}

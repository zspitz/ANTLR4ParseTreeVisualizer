using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParseTreeVisualizer.Util;
using System.Runtime.Serialization;
using Antlr4.Runtime;
using static ParseTreeVisualizer.Util.Functions;
using System.ComponentModel;

namespace ParseTreeVisualizer {
    public enum TreeNodeType {
        RuleContext,
        Token,
        Error
    }

    [Serializable]
    public class TreeNodeData : INotifyPropertyChanged {
        public string Caption { get; }
        public IList<PropertyValueVM> Properties { get; }
        public IList<TreeNodeData> Children { get; }
        public (int startTokenIndex, int endTokenIndex) TokenSpan { get; }
        public TreeNodeType? NodeType { get; }
        public TreeNodeData(IParseTree tree, VisualizerData visualizerData) {
            var t = tree.GetType();

            if (tree is RuleContext) {
                NodeType= TreeNodeType.RuleContext;
                Caption = t.Name;
            } else if (tree is TerminalNodeImpl terminalNode) {
                var vm = new TerminalNodeImplVM(terminalNode);
                visualizerData.TerminalNodes.Add(vm);
                if (vm.IsError) {
                    Caption = vm.Text;
                    NodeType = TreeNodeType.Error;
                } else {
                    Caption = vm.Text.ToCSharpLiteral();
                    NodeType = TreeNodeType.Token;
                }
            }

            Properties = t.GetProperties().OrderBy(x => x.Name).Select(prp => new PropertyValueVM(tree, prp)).ToList();
            Children = tree.Children().Select(x => new TreeNodeData(x, visualizerData)).ToList();
            TokenSpan = (tree.SourceInterval.a, tree.SourceInterval.b);
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

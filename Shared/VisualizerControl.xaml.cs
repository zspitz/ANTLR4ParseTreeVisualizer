using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ParseTreeVisualizer.Util;

namespace ParseTreeVisualizer {
    public partial class VisualizerControl {
        public VisualizerControl() {
            InitializeComponent();

            tokens.AutoGeneratingColumn += (s, e) => {
                if (e.PropertyName == "IsError") { e.Cancel = true; }
            };

            // scrolls the tree view item into view when expanded
            AddHandler(TreeViewItem.ExpandedEvent, (RoutedEventHandler)((s, e) => {
                var tvi = e.OriginalSource as TreeViewItem;
                tvi.BringIntoView();
            }));


            Loaded += (s, e) => {
                data.Root.IsExpanded = true;

                source.LostFocus += (s1, e1) => e1.Handled = true;
                source.Focus();
                source.SelectAll();
            };

            source.SelectionChanged += (s, e) => changeSelection(source);
            tokens.SelectionChanged += (s, e) => changeSelection(tokens);
            treeview.SelectedItemChanged += (s, e) => changeSelection(treeview);
        }

        private VisualizerData data => (VisualizerData)DataContext;

        private bool inChangeSelection;
        private void changeSelection(object sender) {
            if (inChangeSelection) { return; }
            inChangeSelection = true;

            (int startTokenIndex, int endTokenIndex) = (0, 0);
            if (sender == treeview) {
                (startTokenIndex, endTokenIndex) = treeview.SelectedItem<TreeNodeData>().TokenSpan;
            } else if (sender == source) {
                var startChar = source.SelectionStart;
                var endChar = source.SelectionStart + source.SelectionLength;
                startTokenIndex = data.TerminalNodes.FirstOrDefault(x => x.Span.start <= startChar && x.Span.stop >= startChar)?.Index ??
                    data.TerminalNodes.Min(x => x.Index);
                endTokenIndex = data.TerminalNodes.FirstOrDefault(x => x.Span.start <= endChar && x.Span.stop >= endChar)?.Index ??
                    data.TerminalNodes.Max(x => x.Index);
            } else if (sender == tokens) {
                var selectedItems = tokens.SelectedItems<TerminalNodeImplVM>();
                startTokenIndex = selectedItems.Min(x => x.Index);
                endTokenIndex = selectedItems.Max(x => x.Index);
            } else {
                throw new InvalidOperationException("Unrecognized selection change source.");
            }

            if (sender != source) {
                var selectionStart = data.NodesByIndex[startTokenIndex].Span.start;
                var selectionStop = data.NodesByIndex[endTokenIndex].Span.stop;
                if (selectionStart<0) {
                    source.Select(0, 0);
                } else {
                    source.Select(selectionStart, selectionStop - selectionStart + 1);
                }
            }

            if (sender != tokens) {
                tokens.SelectedItems.Clear();
                foreach (var selectedToken in data.TerminalNodes.Where(x => x.Index >= startTokenIndex && x.Index <= endTokenIndex)) {
                    tokens.SelectedItems.Add(selectedToken);
                    tokens.ScrollIntoView(selectedToken);
                }
            }

            if (sender != treeview) {
                data.Root.ClearSelection();
                var selectedNode = data.Root;
                while (true) {
                    var nextChild = selectedNode.Children.SingleOrDefault(x => startTokenIndex >= x.TokenSpan.startTokenIndex && endTokenIndex <= x.TokenSpan.endTokenIndex);
                    if (nextChild == null) { break; }
                    selectedNode = nextChild;
                    selectedNode.IsExpanded = true;
                }
                selectedNode.IsSelected = true;
            }

            inChangeSelection = false;
        }
    }
}

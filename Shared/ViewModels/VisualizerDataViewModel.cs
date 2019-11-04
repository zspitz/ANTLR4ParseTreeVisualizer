using ParseTreeVisualizer.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer {
    public class VisualizerDataViewModel : ViewModelBase<VisualizerData> {
        private int sourceSelectionStart;
        public int SourceSelectionStart {
            get => sourceSelectionStart;
            set => NotifyChanged(ref sourceSelectionStart, value);
        }

        private int sourceSelectionLength;
        public int SourceSelectionLength {
            get => sourceSelectionLength;
            set => NotifyChanged(ref sourceSelectionLength, value);
        }

        private int sourceSelectionEnd => 
            sourceSelectionLength == 0 ? 
                sourceSelectionStart :
                sourceSelectionStart + sourceSelectionLength - 1;

        public ParseTreeNodeViewModel Root { get; }
        public ReadOnlyCollection<TokenViewModel> Tokens { get; }

        public VisualizerDataViewModel(VisualizerData visualizerData) : base(visualizerData) {
            Root = ParseTreeNodeViewModel.Create(visualizerData.Root);
            Tokens = visualizerData.Tokens?.OrderBy(x => x.Index).Select(x => new TokenViewModel(x)).ToList().AsReadOnly();

            if (!(Root is null)) {
                if (visualizerData.Config.HasTreeFilter()) {
                    Root.SetSubtreeExpanded(true);
                } else {
                    Root.IsExpanded = true;
                }
            }
        }

        private bool inUpdateSelection;
        private void updateSelection(object parameter) {
            if (inUpdateSelection) { return; }
            inUpdateSelection = true;

            // sender will be either VisualizerDataViewModel, treenode viewmodel, or token viewmodel
            // HACK the type of the sender also tells us which part of the viewmodel has been selected -- source, tree, or tokens

            (int start, int end)? charSpan = null;
            string source;
            if (parameter == this) { // textbox's data context
                charSpan = (sourceSelectionStart, sourceSelectionEnd);
                source = "Source";
            } else if (parameter is ParseTreeNodeViewModel selectedNode) { // treeview.SelectedItem
                charSpan = selectedNode.Model?.CharSpan;
                source = "Root";
            } else if (parameter is IList) { // selected items in datagrid
                charSpan = Tokens.SelectionCharSpan();
                source = "Tokens";
            } else {
                throw new Exception("Unknown sender");
            }

            var (start, end) = (charSpan ?? (-1, -1));
            if (source != "Source") {
                if (charSpan != null && charSpan != (-1, -1)) {
                    SourceSelectionStart = start - Model.SourceOffset;
                    SourceSelectionLength = end -start + 1;
                } else {
                    SourceSelectionLength = 0;
                    SourceSelectionStart = 0;
                }
            }

            
            if (source != "Tokens") {
                if (charSpan == null) {
                    Tokens.ForEach(x => x.IsSelected = false);
                } else {
                    Tokens.ForEach(x => 
                        x.IsSelected = x.Model.Span.start <= end && x.Model.Span.stop >= start
                    );
                }
            }

            if (source != "Root" && Root.Model != null) {
                Root.ClearSelection();
                var selectedNode = Root;

                // returns true if the node encompasses the selection
                bool matcher(ParseTreeNodeViewModel x) {
                    var (nodeStart, nodeEnd) = x.Model.CharSpan;
                    return start >= nodeStart && end <= nodeEnd;
                }

                if (matcher(selectedNode)) {
                    while (true) {
                        var nextChild = selectedNode.Children.OneOrDefault(matcher);
                        if (nextChild==null) { break; }
                        selectedNode = nextChild;
                        selectedNode.IsExpanded = true;
                    }
                    selectedNode.IsSelected = true;
                }
            }

            inUpdateSelection = false;
        }

        // TOOD move filtering to here

        private RelayCommand changeSelection;
        public RelayCommand ChangeSelection {
            get {
                if (changeSelection == null) {
                    changeSelection = new RelayCommand(sender => updateSelection(sender));
                }
                return changeSelection;
            }
        }
    }
}

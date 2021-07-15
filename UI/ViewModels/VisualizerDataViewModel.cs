using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using ZSpitz.Util.Wpf;
using ParseTreeVisualizer.Serialization;
using ZSpitz.Util;
using System.Windows.Input;
using System.Collections.Generic;

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

        private TokenViewModel? firstSelectedToken;
        public TokenViewModel? FirstSelectedToken => firstSelectedToken;

        public ParseTreeNodeViewModel? Root { get; }
        public ReadOnlyCollection<TokenViewModel>? Tokens { get; }

        public VisualizerDataViewModel(VisualizerData visualizerData, ICommand? openInNewWindow, RelayCommand? copyWatchExpression, RelayCommand? setAsRootNode) : base(visualizerData) {
            if (visualizerData.Root is not null) {
                Root = new ParseTreeNodeViewModel(visualizerData.Root, openInNewWindow, copyWatchExpression, setAsRootNode);
            }

            Tokens = visualizerData.Tokens?.OrderBy(x => x.Index).Select(x => {
                var vm = new TokenViewModel(x);
                vm.PropertyChanged += (s, e) => {
                    if (e.PropertyName != nameof(vm.IsSelected)) { return; }

                    if (vm.IsSelected) {
                        if (firstSelectedToken is null || firstSelectedToken.Model.Index > vm.Model.Index) {
                            NotifyChanged(ref firstSelectedToken, vm, nameof(FirstSelectedToken));
                        }
                    } else {
                        if (firstSelectedToken != null && firstSelectedToken.Model.Index == vm.Model.Index) {
                            var firstSelected = Tokens.Where(x => x.IsSelected).OrderBy(x => x.Model.Index).FirstOrDefault();
                            NotifyChanged(ref firstSelectedToken, firstSelected, nameof(FirstSelectedToken));
                        }
                    }
                };
                return vm;
            }).ToList().AsReadOnly();

            if (!(Root is null)) {
                if (visualizerData.Config.HasTreeFilter()) {
                    Root.SetSubtreeExpanded(true);
                } else {
                    Root.IsExpanded = true;
                }
            }

            ChangeSelection = new RelayCommand(sender => updateSelection(sender));
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
                charSpan = (sourceSelectionStart + Model.SourceOffset, sourceSelectionEnd + Model.SourceOffset);
                source = "Source";
            } else if (parameter is ParseTreeNodeViewModel selectedNode) { // treeview.SelectedItem
                charSpan = selectedNode.Model?.CharSpan;
                source = "Root";
            } else if (parameter is IList) { // selected items in datagrid
                charSpan = Tokens?.SelectionCharSpan();
                source = "Tokens";
            } else if (parameter is null) {
                inUpdateSelection = false;
                return;
            } else {
                throw new Exception("Unknown sender");
            }

            var (start, end) = (charSpan ?? (-1, -1));
            if (source != "Source") {
                if (charSpan != null && charSpan != (-1, -1)) {
                    SourceSelectionStart = start - Model.SourceOffset;
                    SourceSelectionLength = end - start + 1;
                } else {
                    SourceSelectionLength = 0;
                    SourceSelectionStart = 0;
                }
            }

            if (source != "Tokens" && !(Tokens is null)) {
                if (charSpan == null) {
                    Tokens.ForEach(x => x.IsSelected = false);
                } else {
                    Tokens.ForEach(x =>
                        x.IsSelected = x.Model.Span.start <= end && x.Model.Span.stop >= start
                    );
                }
            }

            if (source != "Root" && !(Root?.Model is null)) {
                Root.ClearSelection();
                var selectedNode = Root;

                // returns true if the node encompasses the selection
                bool matcher(ParseTreeNodeViewModel x) {
                    var (nodeStart, nodeEnd) = x.Model.CharSpan;
                    return start >= nodeStart && end <= nodeEnd;
                }

                if (matcher(selectedNode)) {
                    while (true) {
                        var nextChild = selectedNode.Children.SingleOrDefaultExt(matcher);
                        if (nextChild is null) { break; }
                        selectedNode = nextChild;
                        selectedNode.IsExpanded = true;
                    }
                    selectedNode.IsSelected = true;
                }
            }

            inUpdateSelection = false;
        }

        // TOOD move filtering to here

        public RelayCommand ChangeSelection { get; }

        public string WindowTitle {
            get {
                var (lexer, parser, rule) = Model.Config;
                return new[] {
                    ("Lexer", lexer ),
                    ("Parser", parser ),
                    ("Rule context", rule )
                }
                .WhereT((name, val) => val is not null)
                .JoinedT("  /   ", (name, val) => $"{name}: {val}");
            }
        }
    }
}

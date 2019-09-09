using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.VisualStudio.DebuggerVisualizers;
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
                // https://stackoverflow.com/a/21436273/111794
                configPopup.CustomPopupPlacementCallback += (popupSize, targetSize, offset) =>
                    new[] {
                        new CustomPopupPlacement() {
                            Point = new Point(targetSize.Width - popupSize.Width, targetSize.Height)
                        }
                    };
                configButton.Click += (s1, e1) => configPopup.IsOpen = true;
                configPopup.Opened += (s1, e1) => configPopup.DataContext = data.Config.Clone();
                configPopup.Closed += (s1, e1) => {
                    var popupConfig = configPopup.DataContext<VisualizerConfig>();
                    if (popupConfig.ShouldTriggerReload ?? false) {
                        Config = configPopup.DataContext<VisualizerConfig>();
                    }
                };

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

            (int? startTokenIndex, int? endTokenIndex) = (null, null);
            if (sender == treeview) {
                (startTokenIndex, endTokenIndex) = treeview.SelectedItem<TreeNodeVM>().TokenSpan;
            } else if (sender == source) {
                var startChar = source.SelectionStart;
                var endChar = source.SelectionStart + source.SelectionLength;
                startTokenIndex = data.TerminalNodes.FirstOrDefault(x => x.Span.start <= startChar && x.Span.stop >= startChar)?.Index ??
                    data.TerminalNodes.Min(x => x.Index);
                endTokenIndex = data.TerminalNodes.FirstOrDefault(x => x.Span.start <= endChar && x.Span.stop >= endChar)?.Index ??
                    data.TerminalNodes.Max(x => x.Index);
            } else if (sender == tokens) {
                var selectedItems = tokens.SelectedItems<TerminalNodeImplVM>();
                if (selectedItems.Any()) {
                    startTokenIndex = selectedItems.Min(x => x.Index);
                    endTokenIndex = selectedItems.Max(x => x.Index);
                }
            } else {
                throw new InvalidOperationException("Unrecognized selection change source.");
            }

            if (sender != source) {
                if (startTokenIndex == null || endTokenIndex == null) {
                    source.Select(0, 0);
                } else {
                    var selectionStart = data.NodesByIndex[startTokenIndex.Value].Span.start;
                    var selectionStop = data.NodesByIndex[endTokenIndex.Value].Span.stop;
                    if (selectionStart < 0) {
                        source.Select(0, 0);
                    } else {
                        source.Select(selectionStart, selectionStop - selectionStart + 1);
                    }
                }
            }

            if (sender != tokens) {
                tokens.SelectedItems.Clear();
                if (startTokenIndex != null && endTokenIndex != null) {
                    foreach (var selectedToken in data.TerminalNodes.Where(x => x.Index >= startTokenIndex && x.Index <= endTokenIndex)) {
                        tokens.SelectedItems.Add(selectedToken);
                        tokens.ScrollIntoView(selectedToken);
                    }
                }
            }

            if (sender != treeview) {
                data.Root.ClearSelection();
                if (startTokenIndex != null && endTokenIndex != null) {
                    var selectedNode = data.Root;
                    while (true) {
                        var nextChild = selectedNode.Children.SingleOrDefault(x => startTokenIndex >= x.TokenSpan.startTokenIndex && endTokenIndex <= x.TokenSpan.endTokenIndex);
                        if (nextChild == null) { break; }
                        selectedNode = nextChild;
                        selectedNode.IsExpanded = true;
                    }
                    selectedNode.IsSelected = true;
                }
            }

            inChangeSelection = false;
        }

        private void LoadDataContext() {
            if (_objectProvider == null || _config == null) { return; }
            DataContext = _objectProvider.TransferObject(_config);
            data.Root.IsExpanded = true;
            _config = data.Config;
            Config.Write();
        }

        private IVisualizerObjectProvider _objectProvider;
        public IVisualizerObjectProvider objectProvider {
            get => _objectProvider;
            set {
                if (value == _objectProvider) { return; }
                _objectProvider = value;
                LoadDataContext();
            }
        }

        private VisualizerConfig _config;
        public VisualizerConfig Config {
            get => _config;
            set {
                if (value == _config) { return; }
                _config = value;
                LoadDataContext();
            }
        }
    }
}

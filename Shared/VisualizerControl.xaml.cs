using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.VisualStudio.DebuggerVisualizers;
using ParseTreeVisualizer.Util;
using ParseTreeVisualizer.ViewModels;

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

            tokens.AutoGeneratingColumn += (s, e) => {
                if (e.PropertyName.In(nameof(Token.IsSelected), nameof(Token.TokenTypeID))) {
                    e.Cancel = true;
                }
            };

            Loaded += (s, e) => {
                // https://stackoverflow.com/a/21436273/111794
                configPopup.CustomPopupPlacementCallback += (popupSize, targetSize, offset) =>
                    new[] {
                        new CustomPopupPlacement() {
                            Point = new Point(targetSize.Width - popupSize.Width, targetSize.Height)
                        }
                    };
                configButton.Click += (s1, e1) => configPopup.IsOpen = true;

                configPopup.Opened += (s1, e1) => {
                    var cloned = data.Config.Clone();
                    configPopup.DataContext = cloned;
                    // TODO replace this foreach with databinding -- https://github.com/zspitz/ANTLR4ParseTreeVisualizer/issues/22
                    foreach (var kvp in data.TokenTypeMapping.Where(x =>  x.Key.In(cloned.SelectedTokenTypes))) {
                        lbSelectedTokenTypes.SelectedItems.Add(kvp);
                    }
                };

                configPopup.Closed += (s1, e1) => {
                    var popupConfig = configPopup.DataContext<Config>();
                    // TODO replace the next line with databinding -- https://github.com/zspitz/ANTLR4ParseTreeVisualizer/issues/22
                    popupConfig.SelectedTokenTypes = lbSelectedTokenTypes.SelectedItems.Cast<KeyValuePair<int,string>>().Select(x => x.Key).ToHashSet();
                    if (popupConfig.ShouldTriggerReload() ?? false) {
                        Config = popupConfig;
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

        private TreeVisualizer data => (TreeVisualizer)DataContext;

        private bool inChangeSelection;
        private void changeSelection(object sender) {
            if (inChangeSelection) { return; }
            inChangeSelection = true;

            (int? startTokenIndex, int? endTokenIndex) = (null, null);
            if (sender == treeview) {
                (startTokenIndex, endTokenIndex) = treeview.SelectedItem<ParseTreeNode>().TokenSpan;
            } else if (sender == source) {
                var startChar = source.SelectionStart;
                var endChar = source.SelectionStart + source.SelectionLength;
                startTokenIndex = data.Tokens.FirstOrDefault(x => x.Span.start <= startChar && x.Span.stop >= startChar)?.Index ??
                    data.Tokens.DefaultIfEmpty().Min(x => x.Index);
                endTokenIndex = data.Tokens.FirstOrDefault(x => x.Span.start <= endChar && x.Span.stop >= endChar)?.Index ??
                    data.Tokens.DefaultIfEmpty().Max(x => x.Index);
            } else if (sender == tokens) {
                var selectedItems = tokens.SelectedItems<Token>();
                if (selectedItems.Any()) {
                    startTokenIndex = selectedItems.Min(x => x.Index);
                    endTokenIndex = selectedItems.Max(x => x.Index);
                }
            } else {
                throw new InvalidOperationException("Unrecognized source of selection change.");
            }

            if (sender != source) {
                if (startTokenIndex == null || endTokenIndex == null) {
                    source.Select(0, 0);
                } else {
                    var selectionStart = data.Tokens.GetByIndex(startTokenIndex.Value).Span.start;
                    var selectionStop = data.Tokens.GetByIndex(endTokenIndex.Value).Span.stop;
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
                    data.Tokens.SelectionStartTokenIndex = startTokenIndex;
                    data.Tokens.SelectionEndTokenIndex = endTokenIndex;
                    tokens.ScrollIntoView(data.Tokens[startTokenIndex.Value]);

                    //foreach (var selectedToken in data.TerminalNodes.Where(x => x.Index >= startTokenIndex && x.Index <= endTokenIndex)) {
                    //    tokens.SelectedItems.Add(selectedToken);
                    //    tokens.ScrollIntoView(selectedToken);
                    //}
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

            if (data.AssemblyLoadErrors.Any()) {
                MessageBox.Show($"Error loading the following assemblies:\n\n{data.AssemblyLoadErrors.Joined("\n")}");
            }
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

        private Config _config;
        public Config Config {
            get => _config;
            set {
                if (value == _config) { return; }
                _config = value;
                LoadDataContext();
            }
        }
    }
}

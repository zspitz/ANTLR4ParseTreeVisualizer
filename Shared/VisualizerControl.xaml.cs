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
                };

                configPopup.Closed += (s1, e1) => {
                    var popupConfig = configPopup.DataContext<Config>();
                    popupConfig.UpdateSelectedTokenTypes(); // in case changes were made to the selection
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
            tokens.SelectionChanged += (s, e) => {
                changeSelection(tokens);
            };
            treeview.SelectedItemChanged += (s, e) => changeSelection(treeview);
        }

        private TreeVisualizer data => (TreeVisualizer)DataContext;

        private bool inChangeSelection;
        private void changeSelection(object sender) {
            if (inChangeSelection) { return; }
            inChangeSelection = true;

            (int start, int end)? charSpan = null;
            if (sender == treeview) {
                charSpan = treeview.SelectedItem<ParseTreeNode>()?.CharSpan;
            } else if (sender == source) {
                charSpan = (source.SelectionStart, source.SelectionStart + source.SelectionLength);
            } else if (sender == tokens) {
                var tokensStartChar = data.Tokens.SelectionStartChar;
                var tokensEndChar = data.Tokens.SelectionEndChar;
                if (tokensStartChar != null && tokensEndChar != null) {
                    charSpan = (tokensStartChar.Value, tokensEndChar.Value);
                }
            }

            if (sender != source) {
                if (charSpan != null) {
                    source.Select(charSpan.Value.start, charSpan.Value.end - charSpan.Value.start + 1);
                } else {
                    source.Select(0, 0);
                }
            }

            if (sender != tokens) {
                if (charSpan == null) {
                    data.Tokens.Select(null, null);
                } else {
                    data.Tokens.Select(charSpan.Value.start, charSpan.Value.end);
                }
            }

            if (sender != treeview) {
                data.Root.ClearSelection();
                if (charSpan != null) {
                    var startChar = charSpan.Value.start;
                    var endChar = charSpan.Value.end;
                    var selectedNode = data.Root;
                    while (true) {
                        var nextChild = selectedNode.Children.SingleOrDefault(x => startChar >= x.CharSpan.startChar && endChar<=x.CharSpan.endChar);
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

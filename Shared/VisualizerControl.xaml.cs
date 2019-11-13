using System;
using System.Linq;
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
                if (e.PropertyName.In(
                    nameof(ViewModelBase<string>.Model),
                    nameof(Selectable<Token>.IsSelected)
                )) {
                    e.Cancel = true;
                }

                if (e.PropertyName == nameof(TokenViewModel.Text)) {
                    e.Column.Width = 150;
                    (e.Column as DataGridTextColumn)!.ElementStyle = FindResource("TextTrimmedTextbox") as Style;
                }
            };

            // scrolls the tree view item into view when selected
            AddHandler(TreeViewItem.SelectedEvent, (RoutedEventHandler)((s, e) => ((TreeViewItem)e.OriginalSource).BringIntoView()));

            Loaded += (s, e) => {
                // https://stackoverflow.com/a/21436273/111794
                configPopup.CustomPopupPlacementCallback += (popupSize, targetSize, offset) =>
                    new[] {
                        new CustomPopupPlacement() {
                            Point = new Point(targetSize.Width - popupSize.Width, targetSize.Height)
                        }
                    };
                configButton.Click += (s1, e1) => configPopup.IsOpen = true;

                configPopup.Opened += (s1, e1) =>
                    configPopup.DataContext = new ConfigViewModel(data.Model.Config, data.Model);

                configPopup.Closed += (s1, e1) => {
                    var popupConfig = configPopup.DataContext<ConfigViewModel>();
                    if (popupConfig.IsDirty) {
                        Config = popupConfig.Model;
                    }
                };

                source.LostFocus += (s1, e1) => e1.Handled = true;
                source.Focus();
                source.SelectionChanged += (s1, e1) => {
                    if (data == null) { return; }
                    data.SourceSelectionLength = source.SelectionLength;
                    data.SourceSelectionStart = source.SelectionStart;
                };
                source.SelectAll();
            };

            Unloaded += (s, e) => Config?.Write();
        }

        private VisualizerDataViewModel data => (VisualizerDataViewModel)DataContext;

        private void LoadDataContext() {
            if (_objectProvider is null || config is null) { return; }
            DataContext = null;
            if (!(_objectProvider.TransferObject(config) is VisualizerData response)) {
                throw new InvalidOperationException("Unspecified error while serializing/deserializing");
            }

            var vm = new VisualizerDataViewModel(response);
            vm.PropertyChanged += (s, e) => {
                if (e.PropertyName != nameof(VisualizerDataViewModel.FirstSelectedToken)) { return; }
                if (vm.FirstSelectedToken is null) { return; }
                tokens.ScrollIntoView(vm.FirstSelectedToken);
            };

            DataContext = vm;
            config = data.Model.Config;
            Config?.Write();

            var assemblyLoadErrors = data.Model.AssemblyLoadErrors;
            if (assemblyLoadErrors.Any()) {
                MessageBox.Show($"Error loading the following assemblies:\n\n{assemblyLoadErrors.Joined("\n")}");
            }
        }

        private IVisualizerObjectProvider? _objectProvider;
        public IVisualizerObjectProvider? objectProvider {
            get => _objectProvider;
            set {
                if (value == _objectProvider) { return; }
                _objectProvider = value;
                LoadDataContext();
            }
        }

        private Config? config;
        public Config? Config {
            get => config;
            set {
                if (value == config) { return; }
                config = value;
                LoadDataContext();
            }
        }

        private void ExpandAll(object sender, RoutedEventArgs e) =>
            ((MenuItem)sender).DataContext<ParseTreeNodeViewModel>()?.SetSubtreeExpanded(true);
        private void CollapseAll(object sender, RoutedEventArgs e) =>
            ((MenuItem)sender).DataContext<ParseTreeNodeViewModel>()?.SetSubtreeExpanded(false);
        private void SetRootNode(object sender, RoutedEventArgs e) {
            if (Config is null) { return; }
            Config.RootNodePath = ((MenuItem)sender).DataContext<ParseTreeNodeViewModel>()?.Model.Path;
            LoadDataContext();
        }
        private void OpenRootNewWindow(object sender, RoutedEventArgs e) {
            if (Config is null) { return; }
            var newWindow = new VisualizerWindow();
            var content = (newWindow.Content as VisualizerControl)!;
            content.Config = Config.Clone();
            content.Config.RootNodePath = ((MenuItem)sender).DataContext<ParseTreeNodeViewModel>()?.Model.Path;
            content.objectProvider = objectProvider;
            newWindow.ShowDialog();
        }

        private void CopyWatchExpression(object sender, RoutedEventArgs e) {
            if (Config is null) { return; }

            var node = ((MenuItem)sender).DataContext<ParseTreeNodeViewModel>();
            if (node == null) { return; }
            var model = node.Model;

            if (data.Model.Config.WatchBaseExpression.IsNullOrWhitespace()) {
                var dlg = new WatchExpressionPrompt();
                dlg.ShowDialog();
                data.Model.Config.WatchBaseExpression = dlg.Expression;
            }

            var watchExpression = Config.WatchBaseExpression;
            if (!model.Path.IsNullOrWhitespace()) {
                watchExpression += model.Path.Split('.').Joined("", x => $".GetChild({x})");
            }
            Clipboard.SetText(watchExpression);
        }
    }
}

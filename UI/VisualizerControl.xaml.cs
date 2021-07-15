using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ParseTreeVisualizer.Serialization;
using ZSpitz.Util;
using ZSpitz.Util.Wpf;
using static System.Windows.SystemColors;

namespace ParseTreeVisualizer {
    public partial class VisualizerControl {
        public VisualizerControl() {
            InitializeComponent();

            // When a control loses focus, it should look no different from when it had the focus (e.g. selection color)
            Resources[InactiveSelectionHighlightBrushKey] = HighlightBrush;
            Resources[InactiveSelectionHighlightTextBrushKey] = HighlightTextBrush;

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
                source.LostFocus += (s1, e1) => e1.Handled = true;
                source.Focus();
                source.SelectAll();
            };

            void handler(object s1, PropertyChangedEventArgs e1) {
                var vm = (VisualizerDataViewModel)s1;
                if (e1.PropertyName != nameof(VisualizerDataViewModel.FirstSelectedToken)) { return; }
                if (vm.FirstSelectedToken is null) { return; }
                tokens.ScrollIntoView(vm.FirstSelectedToken);
            }

            DataContextChanged += (s, e) => {
                if (e.OldValue is VisualizerDataViewModel vm) {
                    vm.PropertyChanged -= handler;
                }
                if (e.NewValue is VisualizerDataViewModel vm1) {
                    vm1.PropertyChanged += handler;
                }

                var assemblyLoadErrors = data.Model.AssemblyLoadErrors;
                if (assemblyLoadErrors.Any()) {
                    MessageBox.Show($"Error loading the following assemblies:\n\n{assemblyLoadErrors.Joined("\n")}");
                }
            };
        }

        private VisualizerDataViewModel data => (VisualizerDataViewModel)DataContext;
    }
}

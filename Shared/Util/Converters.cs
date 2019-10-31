using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static System.Windows.Media.Brushes;
using static System.Windows.DependencyProperty;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows;
using System.Collections;

namespace ParseTreeVisualizer.Util {
    public abstract class ReadOnlyConverterBase : IValueConverter {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => UnsetValue;
    }

    public abstract class ReadOnlyMultiConverterBase : IMultiValueConverter {
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => new[] { UnsetValue };
    }

    public class RootConverter : ReadOnlyConverterBase {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => new[] { value };
    }

    public class NullConverter : ReadOnlyConverterBase {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null) { return UnsetValue; }

            if (targetType == typeof(Brush)) {
                return DarkGray;
            } else if (targetType == typeof(FontStyle)) {
                return FontStyles.Italic;
            }
            throw new InvalidOperationException("Converter only valid for Brush and FontStyle.");
        }
    }

    public class ErrorColorConverter : ReadOnlyConverterBase {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ((bool)value) ? Red : UnsetValue;
    }

    public class NodeForegroundConverter : ReadOnlyMultiConverterBase {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values is null) { throw new ArgumentNullException(nameof(values)); }

            if (values[0] == UnsetValue || values[1] == UnsetValue) { return UnsetValue; }

            var nodeType = (TreeNodeType)values[0];
            var filterState = (FilterStates?)values[1];
            switch (nodeType) {
                case TreeNodeType.RuleContext:
                    if (filterState.In(null, FilterStates.Matched)) { return Black; }
                    return LightGray;
                case TreeNodeType.Token:
                    if (filterState.In(null, FilterStates.Matched)) { return Black; }
                    return LightGray;
                case TreeNodeType.ErrorToken: return Red;
                case TreeNodeType.WhitespaceToken: return UnsetValue;
                case TreeNodeType.Placeholder: return DarkGray;
                default: throw new InvalidOperationException("Invalid NodeType value");
            }
        }
    }

    public class NodeFontWeightConverter : ReadOnlyConverterBase {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ((TreeNodeType)value) == TreeNodeType.RuleContext ? FontWeights.Bold : UnsetValue;
    }

    public class NonEmptyListConverter : ReadOnlyMultiConverterBase {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => values.OfType<IEnumerable<object>>().FirstOrDefault(x => x != null && x.Any());
    }

    public class InvertVisibilityConverter : ReadOnlyConverterBase {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            ((Visibility)value) == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
    }

    public class HasValuesVisibilityConverter : ReadOnlyMultiConverterBase {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
            values.All(x => x is string s && !s.IsNullOrWhitespace()) ? Visibility.Visible : Visibility.Hidden;
    }
}

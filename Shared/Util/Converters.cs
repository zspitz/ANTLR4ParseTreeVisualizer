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
using ParseTreeVisualizer.ViewModels;

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
            var nodeType = (TreeNodeType)values[0];
            var filterState = (FilterState?)values[1];
            switch (nodeType) {
                case TreeNodeType.RuleContext:
                    if (filterState.In(null, FilterState.Matched)) { return Black; }
                    return LightGray;
                case TreeNodeType.Token:
                    if (filterState.In(null, FilterState.Matched)) { return Black; }
                    return LightGray;
                case TreeNodeType.ErrorToken: return Red;
                case TreeNodeType.WhitespaceToken: return UnsetValue;
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
}

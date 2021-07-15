using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static System.Windows.Media.Brushes;
using System.Windows.Media;
using System.Windows;
using ZSpitz.Util;
using ZSpitz.Util.Wpf;
using ParseTreeVisualizer.Serialization;
using static ParseTreeVisualizer.Serialization.TreeNodeType;

namespace ParseTreeVisualizer {
    public class RootConverter : ReadOnlyConverterBase {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => 
            new[] { value };
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
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => 
            ((bool)value) ? Red : UnsetValue;
    }

    public class NodeForegroundConverter : ReadOnlyMultiConverterBase {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
            // values[0] should be TreeNodeType; values[1] should be a Serialization.FilterStates
            values is null ? throw new ArgumentNullException(nameof(values)) :
                values[0] == UnsetValue || values[1] == UnsetValue ? UnsetValue :
                (values[0], values[1]) switch {
                    (RuleContext or TreeNodeType.Token, null or Serialization.FilterStates.Matched) => Black,
                    (RuleContext or TreeNodeType.Token, _) => LightGray,
                    (ErrorToken, _) => Red,
                    (WhitespaceToken, _) => UnsetValue,
                    (Placeholder, _) => DarkGray,
                    _ => throw new InvalidOperationException("Unmatched node type / filter state combination.")
                };
    }

    public class NodeFontWeightConverter : ReadOnlyConverterBase {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => 
            ((TreeNodeType)value) == RuleContext ? FontWeights.Bold : UnsetValue;
    }

    public class NonEmptyListConverter : ReadOnlyMultiConverterBase {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => 
            values.OfType<IEnumerable<object>>().FirstOrDefault(x => x != null && x.Any());
    }
}

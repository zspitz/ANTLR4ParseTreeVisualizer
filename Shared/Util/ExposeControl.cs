using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static ParseTreeVisualizer.Util.Functions;
using static System.Windows.FrameworkPropertyMetadataOptions;

namespace ParseTreeVisualizer.Util {
    public static class ExposeControl {
        static readonly object NoObject = new object();

        public static readonly DependencyProperty AsProperty = DPRegisterAttached(typeof(ExposeControl), NoObject, BindsTwoWayByDefault, AsChanged);

        public static object GetAs(DependencyObject obj) => obj.GetValue(AsProperty);

        public static void SetAs(DependencyObject obj, object value) => obj.SetValue(AsProperty, value);

        private static void AsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (e.NewValue == d) { return; }
            d.Dispatcher.BeginInvoke((Action)(() => d.SetCurrentValue(AsProperty, d)));
        }
    }
}

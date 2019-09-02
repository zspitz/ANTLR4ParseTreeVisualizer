using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer.Util {
    public static class INotifyPropertyChangedExtensions {
        public static void NotifyChanged<T>(this INotifyPropertyChanged inpc, ref T current, T newValue, Action<PropertyChangedEventArgs> eventRaiser, [CallerMemberName] string name = null) where T : IEquatable<T> {
            if (current.Equals(newValue)) { return; }
            current = newValue;
            eventRaiser(new PropertyChangedEventArgs(name));
        }
    }
}

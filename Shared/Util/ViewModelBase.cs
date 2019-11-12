using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ParseTreeVisualizer.Util {
    public abstract class ViewModelBase<TModel> : INotifyPropertyChanged {
        public TModel Model { get; }

        protected ViewModelBase(TModel model) => Model = model;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyChanged<T>(ref T current, T newValue, [CallerMemberName] string name = null) {
            if (current is IEquatable<T> equatable) {
                if (equatable.Equals(newValue)) { return; }
            } else if (current is null) {
                if (newValue is null) { return; }
            } else {
                if (current.Equals(newValue)) { return; }
            }
            current = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

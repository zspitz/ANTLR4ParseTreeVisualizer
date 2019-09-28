using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ParseTreeVisualizer.Util {
    public abstract class ViewModelBase<TModel> : INotifyPropertyChanged {
        public TModel Model { get; }

        public ViewModelBase(TModel model) {
            Model = model;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyChanged<T>(ref T current, T newValue, [CallerMemberName] string name = null) where T : IEquatable<T> {
            if (current.Equals(newValue)) { return; }
            current = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

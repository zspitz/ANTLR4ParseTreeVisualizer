using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParseTreeVisualizer.Util;

namespace ParseTreeVisualizer.ViewModels {
    [Serializable]
    public class TokenType : INotifyPropertyChanged {
        public int Index { get; }
        public string Text { get; }

        private bool isSelected;
        public bool IsSelected {
            get => isSelected;
            set => this.NotifyChanged(ref isSelected, value, args => PropertyChanged?.Invoke(this, args));
        }

        public TokenType(int index, string text) {
            Index = index;
            Text = text;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

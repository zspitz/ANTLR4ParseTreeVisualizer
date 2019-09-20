using Antlr4.Runtime.Tree;
using ParseTreeVisualizer.Util;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer.ViewModels {
    [Serializable]
    public class ClassInfo : INotifyPropertyChanged {
        public string Name { get; protected set; }
        public string Namespace { get; protected set;  }
        public string Assembly { get; protected set; }
        public string Antlr { get; protected set; } // Runtime - in Antlr.Runtime, <number> - version
        public string FullName { get; protected set; }

        private ClassInfo(string name) => Name = name;

        protected ClassInfo() { }

        public ClassInfo(Type t) {
            Name = t.Name;
            Namespace = t.Namespace;
            Assembly = t.Assembly.Location;
            if (t.Assembly == typeof(IParseTree).Assembly) {
                Antlr = "Runtime";
            } else {
                t.IfAttribute<GeneratedCodeAttribute>(attr => Antlr = attr.Version);
            }
            FullName = t.FullName;
        }
        public override string ToString() => FullName;

        public static readonly ClassInfo None = new ClassInfo("(None)");

        [NonSerialized]
        private bool isSelected;
        public bool IsSelected {
            get => isSelected;
            set => this.NotifyChanged(ref isSelected, value, args => PropertyChanged?.Invoke(this, args));
        }

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

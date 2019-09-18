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
        public string Name { get; }
        public string Namespace { get; }
        public string Assembly { get; }
        public string Antlr { get; private set; } // Runtime - in Antlr.Runtime, <number> - version
        public string FullName {
            get {
                if (Namespace.IsNullOrWhitespace()) { return null; }
                return $"{Namespace}.{Name}";
            }
        }
        public List<ClassInfo> RelatedTypes { get; }

        private ClassInfo(string name) => Name = name;

        public ClassInfo(Type t, IEnumerable<ClassInfo> relatedTypes = null) {
            Name = t.Name;
            Namespace = t.Namespace;
            Assembly = t.Assembly.Location;
            if (t.Assembly == typeof(IParseTree).Assembly) {
                Antlr = "Runtime";
            } else {
                t.IfAttribute<GeneratedCodeAttribute>(attr => Antlr = attr.Version);
            }
            RelatedTypes = relatedTypes?.ToList();
        }
        public override string ToString() => FullName;

        public static readonly ClassInfo None = new ClassInfo("(None)");

        private bool isSelected;
        public bool IsSelected {
            get => isSelected;
            set => this.NotifyChanged(ref isSelected, value, args => PropertyChanged?.Invoke(this, args));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

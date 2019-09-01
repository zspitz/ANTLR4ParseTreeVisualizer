using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer {
    [Serializable]
    public class PropertyValueVM {
        public bool Custom { get;  }
        public string Key { get; }
        public string Value { get; }
        public PropertyValueVM(object instance, PropertyInfo prp) {
            Key = prp.Name;

            object value = null;
            try {
                value = prp.GetValue(instance);
            } catch (Exception e) {
                value = $"<{e.GetType()}: {e.Message}";
            }
            Value = value?.ToString();

            Custom = !prp.DeclaringType.Namespace.StartsWith("Antlr4");
        }
    }
}

using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using static ParseTreeVisualizer.Util.Functions;

namespace ParseTreeVisualizer.ViewModels {
    [Serializable]
    public class PropertyValue {
        public bool Custom { get; }
        public string Key { get; }
        public string Value { get; }
        public PropertyValue(object instance, PropertyInfo prp) {
            Key = prp.Name;

            // null values map to null strings
            // exceptions map to <...> delineated strings
            // other values map to result of RenderLiteral

            object value = null;
            try {
                value = prp.GetValue(instance);
            } catch (Exception e) {
                Value = $"<{e.GetType()}: {e.Message}>";
            }
            if (value != null) { Value = StringValue(value); }

            Custom = !prp.DeclaringType.Namespace.StartsWith("Antlr4");
        }
    }
}

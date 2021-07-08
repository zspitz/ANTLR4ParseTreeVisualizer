﻿using System;
using System.Reflection;
using static ParseTreeVisualizer.Util.Functions;

namespace ParseTreeVisualizer {
    [Serializable]
    public class PropertyValue {
        public bool Custom { get; }
        public string Key { get; }
        public string? Value { get; }
        public PropertyValue(object instance, PropertyInfo prp) {
            if (prp is null) { throw new ArgumentNullException(nameof(prp)); }

            Key = prp.Name;

            // null values map to null strings
            // exceptions map to <...> delineated strings
            // other values map to result of RenderLiteral

            object? value = null;
            try {
                value = prp.GetValue(instance);
            } catch (Exception e) {
                Value = $"<{e.GetType()}: {e.Message}>";
            }
            if (value is { }) { Value = StringValue(value); }

            Custom = !prp.DeclaringType?.Namespace?.StartsWith("Antlr4", StringComparison.Ordinal) ?? false;
        }
    }
}

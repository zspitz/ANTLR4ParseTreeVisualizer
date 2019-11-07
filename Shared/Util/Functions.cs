using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ParseTreeVisualizer.Util {
    public static class Functions {
        private static string ParseCallerMemberName(string callerMemberName) {
            if (callerMemberName is null) { throw new ArgumentNullException(nameof(callerMemberName)); }
            var s = callerMemberName;
            if (s.EndsWith("Property", StringComparison.OrdinalIgnoreCase)) { s = s.Substring(0, s.Length - "Property".Length); }
            return s;
        }
        public static DependencyProperty DPRegister<TProperty, TOwner>(TProperty defaultValue = default, PropertyChangedCallback callback = null, [CallerMemberName] string propertyName = null) =>
            DependencyProperty.Register(ParseCallerMemberName(propertyName), typeof(TProperty), typeof(TOwner), new PropertyMetadata(defaultValue, callback));
        public static DependencyProperty DPRegister<TProperty, TOwner>(TProperty defaultValue, FrameworkPropertyMetadataOptions flags, PropertyChangedCallback callback = null, [CallerMemberName] string propertyName = null) =>
            DependencyProperty.Register(ParseCallerMemberName(propertyName), typeof(TProperty), typeof(TOwner), new FrameworkPropertyMetadata(defaultValue, flags, callback));

        public static DependencyProperty DPRegisterAttached<TProperty, TOwner>(TProperty defaultValue = default, PropertyChangedCallback callback = null, [CallerMemberName] string propertyName = null) =>
            DependencyProperty.RegisterAttached(ParseCallerMemberName(propertyName), typeof(TProperty), typeof(TOwner), new PropertyMetadata(defaultValue, callback));
        public static DependencyProperty DPRegisterAttached<TProperty, TOwner>(TProperty defaultValue, FrameworkPropertyMetadataOptions flags, PropertyChangedCallback callback = null, [CallerMemberName] string propertyName = null) =>
            DependencyProperty.RegisterAttached(ParseCallerMemberName(propertyName), typeof(TProperty), typeof(TOwner), new FrameworkPropertyMetadata(defaultValue, flags, callback));
        public static DependencyProperty DPRegisterAttached<TProperty>(Type ownerType, TProperty defaultValue, FrameworkPropertyMetadataOptions flags, PropertyChangedCallback callback = null, [CallerMemberName] string propertyName = null) =>
            DependencyProperty.RegisterAttached(ParseCallerMemberName(propertyName), typeof(TProperty), ownerType, new FrameworkPropertyMetadata(defaultValue, flags, callback));

        public static KeyValuePair<TKey, TValue> KVP<TKey, TValue>(TKey key, TValue value) => new KeyValuePair<TKey, TValue>(key, value);

        public static (bool isLiteral, string repr) TryRenderLiteral(object o) {
            var type = o?.GetType().UnderlyingIfNullable();
            bool rendered = true;
            string ret = null;

            if (o == null) {
                ret = "null";
            } else if (o is bool b) {
                ret = b ? "true" : "false";
            } else if (o is char c) {
                ret = $"'{c}'";
            } else if (o is string s) {
                ret = s.ToCSharpLiteral();
            } else if (o is Enum e) {
                ret = $"{e.GetType().Name}.{e.ToString()}";
            } else if (o is MemberInfo mi) {
                bool isType;
                bool isByRef = false;
                if (mi is Type t1) {
                    isType = true;
                    if (t1.IsByRef) {
                        isByRef = true;
                        t1 = t1.GetElementType();
                    }
                } else {
                    isType = false;
                    t1 = mi.DeclaringType;
                }
                ret = $"typeof({t1.FriendlyName()})";
                if (isByRef) { ret += ".MakeByRef()"; }
                if (!isType) {
                    if (mi is ConstructorInfo) {
                        ret += ".GetConstructor()";
                    } else {
                        string methodName = null;
                        switch (mi) {
                            case EventInfo _:
                                methodName = "GetEvent";
                                break;
                            case FieldInfo _:
                                methodName = "GetField";
                                break;
                            case MethodInfo _:
                                methodName = "GetMethod";
                                break;
                            case PropertyInfo _:
                                methodName = "GetProperty";
                                break;
                        }
                        ret += $".{methodName}(\"{mi.Name}\")";
                    }
                }
            } else if (type.IsArray && !type.GetElementType().IsArray && type.GetArrayRank() == 1) {
                var values = (o as dynamic[]).Joined(", ", x => RenderLiteral(x));
                ret = $"new[] {{ {values} }}";
            } else if (type.IsTupleType()) {
                ret = "(" + TupleValues(o).Select(x => RenderLiteral(x)).Joined(", ") + ")";
            } else if (type.IsNumeric()) {
                ret = o.ToString();
            }

            if (ret == null) {
                rendered = false;
                ret = $"#{type.FriendlyName()}";
            }
            return (rendered, ret);
        }

        public static string RenderLiteral(object o) => TryRenderLiteral(o).repr;

        // TODO handle more than 8 values
        public static object[] TupleValues(object tuple) {
            if (tuple is null) { throw new ArgumentNullException(nameof(tuple)); }
            if (!tuple.GetType().IsTupleType()) { throw new InvalidOperationException(); }
            var fields = tuple.GetType().GetFields();
            if (fields.Any()) { return tuple.GetType().GetFields().Select(x => x.GetValue(tuple)).ToArray(); }
            return tuple.GetType().GetProperties().Select(x => x.GetValue(tuple)).ToArray();
        }

        public static string StringValue(object o) {
            var (isLiteral, repr) = TryRenderLiteral(o);
            if (!isLiteral) {
                var hasDeclaredToString = o.GetType().GetMethods().Any(x => {
                    if (x.Name != "ToString") { return false; }
                    if (x.GetParameters().Any()) { return false; }
                    if (x.DeclaringType == typeof(object)) { return false; }
                    if (x.DeclaringType.InheritsFromOrImplements<EnumerableQuery>()) { return false; } // EnumerableQuery implements its own ToString which we don't want to use
                    return true;
                });
                if (hasDeclaredToString) { return o.ToString(); }
            }
            return repr;
        }
    }
}

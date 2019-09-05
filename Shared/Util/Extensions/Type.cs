using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Linq.Enumerable;

namespace ParseTreeVisualizer.Util {
    public static class TypeExtensions {
        public static Type UnderlyingIfNullable(this Type type) => Nullable.GetUnderlyingType(type) ?? type;

        public static bool IsClosureClass(this Type type) =>
            type != null && type.HasAttribute<CompilerGeneratedAttribute>() && type.Name.ContainsAny("DisplayClass", "Closure$");

        private static readonly HashSet<Type> numericTypes = new HashSet<Type>() {
            typeof(byte),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(sbyte),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal)
        };

        public static bool IsAnonymous(this Type type) =>
            type.HasAttribute<CompilerGeneratedAttribute>() && type.Name.Contains("Anonymous") && type.Name.ContainsAny("<>", "VB$");

        public static bool IsNumeric(this Type type) => type.UnderlyingIfNullable().In(numericTypes);

        public static IEnumerable<(Type current, Type root)> NestedArrayTypes(this Type type) {
            var currentType = type;
            while (currentType.IsArray) {
                var nextType = currentType.GetElementType();
                if (nextType.IsArray) {
                    yield return (currentType, null);
                } else {
                    yield return (currentType, nextType);
                    break;
                }
                currentType = nextType;
            }
        }

        private static readonly Dictionary<Type, string> CSKeywordTypes = new Dictionary<Type, string> {
            {typeof(bool), "bool"},
            {typeof(byte), "byte"},
            {typeof(sbyte), "sbyte"},
            {typeof(char), "char"},
            {typeof(decimal), "decimal"},
            {typeof(double), "double"},
            {typeof(float), "float"},
            {typeof(int), "int"},
            {typeof(uint), "uint"},
            {typeof(long), "long"},
            {typeof(ulong), "ulong"},
            {typeof(object), "object"},
            {typeof(short), "short"},
            {typeof(ushort), "ushort"},
            {typeof(string), "string"},
            {typeof(void), "void" }
        };

        public static string FriendlyName(this Type type) {
            if (type.IsClosureClass()) { return "<closure>" ; }

            if (type.IsAnonymous()) {
                return "{ " + type.GetProperties().Joined(", ", p => {
                    var name = p.Name;
                    var typename = p.PropertyType.FriendlyName();
                    return $"{typename} {name}";
                }) + " }";
            }

            if (type.IsArray) {
                (string left, string right) = ("[", "]");
                var nestedArrayTypes = type.NestedArrayTypes().ToList();
                string arraySpecifiers = nestedArrayTypes.Joined("",
                    (current, _, index) => left + Repeat("", current.GetArrayRank()).Joined() + right
                );
                return nestedArrayTypes.Last().root.FriendlyName() + arraySpecifiers;
            }

            if (!type.IsGenericType) {
                if (CSKeywordTypes.TryGetValue(type, out var ret)) { return ret; }
                return type.Name;
            }

            if (type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                return type.UnderlyingIfNullable().FriendlyName() + "?";
            }

            if (type.IsGenericParameter) { return type.Name; }

            var parts = type.GetGenericArguments().Joined(", ", t => t.FriendlyName());
            var backtickIndex = type.Name.IndexOf('`');
            var nongenericName = type.Name.Substring(0, backtickIndex);
            return $"{nongenericName}<{parts}>";
        }

        public static bool IsTupleType(this Type type) {
            if (!type.IsGenericType) { return false; }
            var openType = type.GetGenericTypeDefinition();
            if (openType.In(
                typeof(ValueTuple<>),
                typeof(ValueTuple<,>),
                typeof(ValueTuple<,,>),
                typeof(ValueTuple<,,,>),
                typeof(ValueTuple<,,,,>),
                typeof(ValueTuple<,,,,,>),
                typeof(ValueTuple<,,,,,,>),
                typeof(Tuple<>),
                typeof(Tuple<,>),
                typeof(Tuple<,,>),
                typeof(Tuple<,,,>),
                typeof(Tuple<,,,,>),
                typeof(Tuple<,,,,,>),
                typeof(Tuple<,,,,,,>)
            )) {
                return true;
            }
            return (openType.In(typeof(ValueTuple<,,,,,,,>), typeof(Tuple<,,,,,,,>))
                && type.GetGenericArguments()[7].IsTupleType());
        }

        public static bool InheritsFromOrImplements<T>(this Type type) => typeof(T).IsAssignableFrom(type);

        public static bool InheritsFromOrImplementsAny(this Type type, IEnumerable<Type> types) => types.Any(t => t.IsAssignableFrom(type));
        public static bool InheritsFromOrImplementsAny(this Type type, params Type[] types) => types.Any(t => t.IsAssignableFrom(type));
    }
}

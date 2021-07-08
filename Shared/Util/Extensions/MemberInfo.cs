using System;
using System.Linq;
using System.Reflection;

namespace ParseTreeVisualizer.Util {
    public static class MemberInfoExtensions {
        public static bool HasAttribute<TAttribute>(this MemberInfo mi, bool inherit = true) where TAttribute : Attribute =>
            mi.GetCustomAttributes(typeof(TAttribute), inherit).Any();

        public static bool IfAttribute<TAttribute>(this MemberInfo mi, Action<TAttribute> action) where TAttribute : Attribute {
            // TODO what happens if AllowMultiple=true on the attribute?
            var attr = (TAttribute)mi.GetCustomAttribute(typeof(TAttribute), false);
            if (attr == null) { return false; }
            action(attr);
            return true;
        }
    }
}

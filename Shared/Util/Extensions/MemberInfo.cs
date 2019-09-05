using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

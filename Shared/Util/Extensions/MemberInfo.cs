using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer.Util
{
    public static class MemberInfoExtensions {
        public static bool HasAttribute<TAttribute>(this MemberInfo mi, bool inherit = false) where TAttribute : Attribute =>
            mi.GetCustomAttributes(typeof(TAttribute), inherit).Any();
    }
}

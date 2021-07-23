using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ParseTreeVisualizer.Test {
    public static class Extensions {
        public static TheoryData<T1, T2> ToTheoryData<T1, T2>(this IEnumerable<(T1, T2)> src) {
            var ret = new TheoryData<T1, T2>();
            foreach (var (a, b) in src) {
                ret.Add(a, b);
            }
            return ret;
        }
        public static TheoryData<T1, T2, T3, T4> ToTheoryData<T1, T2, T3, T4>(this IEnumerable<(T1, T2, T3, T4)> src) {
            var ret = new TheoryData<T1, T2, T3, T4>();
            foreach (var (a, b, c, d) in src) {
                ret.Add(a, b, c, d);
            }
            return ret;
        }
    }
}

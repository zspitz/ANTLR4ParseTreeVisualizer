using ParseTreeVisualizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTreeVisualizer.Util {
    public static class IEnumerableTokenExtensions {
        public static int MaxTokenTypeID(this IEnumerable<Token> src) => src.MaxOrDefault(x => x.TokenTypeID) ?? 0;
    }
}

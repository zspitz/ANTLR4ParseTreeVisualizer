using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ParseTreeVisualizer.Tests {
    public partial class TestContainer {
        static TestContainer() {
            //inputs = new[] { "Simple.java", "FormatterTest.java" }.Select(x => File.ReadAllText(x)).ToArray();

            var text = File.ReadAllText("Simple.java");
            inputs = new[] {
                text,
                text.Substring(0,200)
            };

            lexerType = typeof(Java9Lexer);
            parserType = typeof(Java9Parser);
            parseMethod = "compilationUnit";
        }
    }
}

using Rubberduck.Parsing.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ParseTreeVisualizer.Tests {
    public partial class TestContainer {
        static TestContainer() {
            inputs = new[] {
                    @"Public Sub DoSomething()
    Debug.Print ""hi from path 1""
    If True Then
        MsgBox ""hello from path 2""
    End If
    Debug.Print ""still in path 1""
End Sub"
                };
            lexerType = typeof(VBALexer);
            parserType = typeof(VBAParser);
            parseMethod = "subStmt";
        }
    }
}

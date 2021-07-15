using System.IO;
using Rubberduck.Parsing.Grammar;

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
End Sub",
                    @"Public Sub DoSomething()
    Debug.Print ""hi from path 1""
    If True Then",
                    File.ReadAllText("BasicModule(edited).bas")
                };
            lexerType = typeof(VBALexer);
            parserType = typeof(VBAParser);
            parseMethod = "subStmt";
        }
    }
}

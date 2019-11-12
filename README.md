# ANTLR4 Parse Tree Visualizer

[![GitHub Release](https://img.shields.io/github/release/zspitz/antlr4parsetreevisualizer?style=flat&max-age=86400)](https://github.com/zspitz/ANTLR4ParseTreeVisualizer/releases) [![AppVeyor build status](https://img.shields.io/appveyor/ci/zspitz/antlr4parsetreevisualizer?style=flat&max-age=86400)](https://ci.appveyor.com/project/zspitz/antlr4parsetreevisualizer)

![Screenshot](screenshot.png)

## Features

* List of tokens (error tokens are highlighted in red)
* Treeview of rule contexts and terminal nodes (error nodes in red)
* Properties of selected treeview node (properties not declared in the Antlr namespace are checked)
* Input text, or the text from the current channel positioned as in the input
* Selection sync, when selecting in the token list, the tree view, or the source text.

  ![Selection sync](selection-sync.gif)

* Filtering the token list, by text, whitespace, or error; or by specific token types:

  ![Token filtering](token-filtering.gif)

* Filtering the parse tree nodes by text, whitespace, or error nodes; or by specific rule types.

  ![Parse tree filtering](parse-tree-filtering.gif)

* Set a specific node as the root node, either in the current window, or in a new window

  ![Set node as root, in current or new window](set-root.gif)

## Requirements

* Visual Studio 2017 or 2019  
  (This is not a hard dependency; the visualizer can probably be compiled against an older version of Microsoft.VisualStudio.DebuggerVisualizers.dll and used with the relevant VS version.)
* Supports the current Antlr.Runtime.Standard.DLL (4.7.2) as well as the older Antlr.Runtime.DLL (4.6.6).

## Installation

1. Download the ZIP file matching your version of Visual Studio (2019 or 2017) from the [releases](https://github.com/zspitz/ANTLR4ParseTreeVisualizer/releases) page. You may need to [unblock the ZIP file](https://github.com/zspitz/ExpressionToString/wiki/Troubleshooting-visualizer-installation).
2. Unzip the contents of the ZIP file into one of Visual Studio's recognized visualizer folders:

    * _VisualStudioInstallPath_`\Common7\Packages\Debugger\Visualizers`
    * `My Documents\Visual Studio `_Version_`\Visualizers`

    You don't have to restart VS, just make sure VS is not currently in a debugging session

You can also compile the source yourself (`2019.sln` or `2017.sln`) and place the output DLLs in one of the visualizer folders.

If you are debugging .NET Core applications, you may also need to put an additional copy of the visualizer files in a subfolder called `netstandard2.0`, under the folder where you installed the visualizer.

## Usage

1. Begin a debugging session, and break at some point.
2. Hover over an instance of one of the visualizer target types (`Antlr4.Runtime.RuleContext`, `Antlr4.Runtime.BufferedTokenStream`, or `string`), in the code editor, or the Watch or Locals window. This instance can be exposed by any variable, or any expression; the type of the expression doesn't matter.
3. Click on the magnifying glass to the right of the expression.
4. You may need to choose a lexer class if you are visualizing a `string` and there are multiple lexers in the debugged assemblies; and you haven't already done so.
5. You may need to choose a parser class and a parser method from the settings, if you are visualizing a `BufferedTokenStream` or a `string`, and the debugged assemblies have multiple parser classes. (These choices persist between sessions, so if you've already chosen, there's no need to do so again.)

  ![Visualizing a string](visualize-string.gif)

## Contributing

* Test the visualizer. (The significance of this kind of contribution cannot be overestimated.)
* Suggest ideas and enhancements ([issues](https://github.com/zspitz/ANTLR4ParseTreeVisualizer/issues/new))
* Notify about bugs ([issues](https://github.com/zspitz/ANTLR4ParseTreeVisualizer/issues/new))
* Provide screenshots which could be used to demonstrate different features of the visualizer (also via ([issues](https://github.com/zspitz/ANTLR4ParseTreeVisualizer/issues/new)))
* Feel free to star the project
* Provide feedback to Microsoft about [these limitations to the visualizer API](https://github.com/zspitz/ExpressionToString/wiki/External-issues)

## Roadmap

* [Parsing errors in a separate pane](https://github.com/zspitz/ANTLR4ParseTreeVisualizer/issues/24)
* Live window, perhaps as a VS extension. It should be possible to select a lexer class from a lexer assembly, a parser class from a parser assembly, and display the resultant token stream and parse tree.

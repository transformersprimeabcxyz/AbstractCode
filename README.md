AbstractCode
==
AbstractCode is an open source compiler front-end that allows you to set up a compiler, interpreter or analyser for code written in any programming language, using a consistent type system.

History
==
Primarily the goal of this project was to provide a centralised type system and robust parser engine for [LiteDevelop](https://github.com/JerreS/LiteDevelop), an open source Integrated Development Environment (IDE), to make certain features like good provision of suggestions, detection of syntax errors, and refactory of source code, possible. 

Features
==
- **Centralised type system**: The root project (_AbstractCode_) provides a consistent type system that is able to represent members defined in virtually any assembly. It does not matter whether these assemblies are pre-compiled windows applications, or yet-to-be-compiled projects. All member representations are being derived from the same base definitions defined in the _AbstractCode_ project. This takes away the need of worrying about different kinds of implementations of different kinds of metadata sources, and allows you to generalise the way to resolve metadata references and definitions that are being found in an assembly.
- **Detailed syntax trees and nodes**: The _AbstractCode.Ast_ project and its descendants provide definitions of structures in yet-to-be-compiled source files in the form of an _Abstract Syntax Tree_ (or AST), which allows for deep semantic analysis of written source code. Every file's syntax tree starts with an instance of a `CompilationUnit`, which exposes e.g. the top-level using directives, namespace- and type definitions that are being declared in the source file. From there on, it is possible to traverse the tree by inspecting the branches of every tree node, all the way to the leaf nodes. These leaf nodes represent the very basic code elements (also known as tokens) of the source code, such as a specific keyword, identifier, literal, or a single operator character. Moreover, when a syntax tree was created from an input string, each AST node will also remember its text location and text length using the `AstNode.Range` property. This way, the syntax tree is a rather detailed and precise representation of a source code, which can be used in many ways.
- **AST nodes implement the visitor pattern**: Every derivative of the `AstNode` class holds at least three overloads of the `AcceptVisitor` method, which allows for a visitor pattern to be implemented. An example of a visitor is the `CSharpAstWriter` class, which takes an arbitrary AST node, and appends the corresponding C# code to a text output.
- **Structural analysis and pattern matching**: Every derivative of the `AstNode` class defines the `Match(AstNode other)` method, that helps to find matches in AST structures, by comparing the two AST nodes by value rather than by reference. This allows for structural analysis and pattern matching, making it easier to detect issues in the source code.
- **LALR(1) parser generator**: The `AbstractCode.Ast.Parser` namespace makes defining a grammar of a specific programming language and creating a source parser for it very easy. By defining the structures and rules that exist in the language using the `Grammar` and the `GrammarCompiler` classes, it is possible to generate a LALR(1) parser automaton. This graph can then be used by an instance of a `ParserAutomaton`, to create a fully working code reader that obeys the grammatical rules the user defined and creates high-detailed syntax trees of input source codes.

Why another compiler front-end?
==
Some of the readers and users might have noticed at this moment that this project is rather similar to [Micorosoft's .NET compiler platform (a.k.a. Roslyn)](https://github.com/dotnet/roslyn) or [ICSharpCode.NRefactory](https://github.com/icsharpcode/NRefactory), and might have elements from [yacc](http://dinosaur.compilertools.net/yacc/), [bison](http://dinosaur.compilertools.net/bison/), [Irony](https://github.com/dotnet/roslyn) and the like. These people are probably wondering why on earth I even started this project. My answer to that is that this project exists purely because I enjoy solving complex programming-related problems, and wanted to try to create a source parser myself, rather than using existing projects. Unlike many source readers (like the Mono C# Compiler used in NRefactory), AbstractCode does not make use of third-party compiler-compilers to construct a parser. The algorithms are based on various articles and courses I've found on the internet, especially from the [Stanfords's CS143 course regarding compiler constructions](http://web.stanford.edu/class/archive/cs/cs143/cs143.1128/) , so a big shoutout to those guys for teaching me how things work!

How do I contribute?
==
Liked the project and want to support the development? Here are a few things that you can do:
- Star the project.
- Fork the project, make your changes to the code and create a pull request.
- Find and report bugs.
- Donate using PayPal, even the tiny bits help a lot! 

[![Donate!](https://www.paypalobjects.com/en_US/GB/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=VS2P6V5X85QHA)

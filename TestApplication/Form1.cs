
// Uncomment this to test the parser engine.
// #define INITIALIZE_OWN_PARSER

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AbstractCode;
using AbstractCode.Assembly;
using AbstractCode.Ast;
using AbstractCode.Ast.CSharp;
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Parser;
using AbstractCode.Ast.Types;
using AbstractCode.Ast.VisualBasic;
using AbstractCode.Symbols;

namespace AbstractCode.TestApplication
{
    public partial class Form1 : Form
    {
        private static readonly SourceLanguage[] Languages =
        {
            CSharpLanguage.Instance,
            VisualBasicLanguage.Instance
        };
        
        private readonly SourceAssembly _assembly;
        private Compilation _compilation;
        private CompilationUnit _compilationUnit;
        private IDocument _document;

#if INITIALIZE_OWN_PARSER
        private ParserNode _rootParserNode;
        private readonly AutomatonSourceParser _parser;
#endif

        public Form1()
        {
            InitializeComponent();

            foreach (var language in Languages)
                comboBox1.Items.Add(language.Name);
                        
            CurrentLanguage = CSharpLanguage.Instance;

            sourceTextBox.Text = @"
using System;
using System.IO;

namespace MyProgram
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            int count;
            do 
            {
                Console.WriteLine(""Type in a number: "");
            } while (!int.TryParse(Console.ReadLine(), out count));

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(i);
            }

            Console.ReadKey();
        }
    }
}
";
            _compilation = new Compilation(_assembly = new SourceAssembly("dummy"));
            _assembly.CompilationUnits.Add(_compilationUnit = new CompilationUnit());
            _compilation.Assemblies.Add(new NetAssembly(typeof(object).Assembly.Location));

#if INITIALIZE_OWN_PARSER
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = GrammarCompiler.Compile(new VisualBasicGrammar());
            stopwatch.Stop();

            foreach (var conflict in result.CompilationContext.Conflicts)
            {
                listView1.Items.Add(new ListViewItem(new[]
                { conflict.Lookahead.ToString(), conflict.Action1.ToString(), conflict.Action2.ToString() }));
            }

            Text = string.Format("Compiled grammar in {0}ms, States: {1}, Conflicts: {2}", stopwatch.Elapsed.TotalMilliseconds, result.Automaton.States.Count,
                result.CompilationContext.Conflicts.Count);

            _parser = new AutomatonSourceParser(result.Automaton);
            _parser.EnableLogging = true;

            const string logFormat = "{0,-5} | {1, -30} | {2, -30} | {3, -30}";
            Console.WriteLine(logFormat, "State", "Current node", "Top of stack", "Message");
            Console.WriteLine(new string('-', Console.BufferWidth - 1));
            Console.BufferHeight = 400;
            //Console.WindowWidth = Console.BufferWidth = 150;
           // Console.WindowHeight = 60;

            _parser.LogMessageReceived += (sender, args) =>
            {
                Console.WriteLine(logFormat, args.ParserContext.CurrentState.Id, args.ParserContext.CurrentNode,
                    args.ParserContext.ParserStack.Peek(), args.Message);

                if (args.MessageSeverity == MessageSeverity.Error)
                {
                    var expected = args.ParserContext.CurrentState.Actions.Keys.OfType<TokenGrammarElement>();
                    Console.WriteLine("Expected {0}.", string.Join(" or ", expected));
                }
            };
#endif
        }
        
        public SourceLanguage CurrentLanguage
        {
            get { return Languages.FirstOrDefault(x => x.Name == comboBox1.Text); }
            set { comboBox1.Text = value.Name; }
        }

        private TreeNode CreateParserTreeNode(ParserNode parserNode)
        {
            var treeNode = new TreeNode(parserNode.ToString());
            treeNode.Tag = parserNode;

            PopulateTreeNode(treeNode, parserNode);
            treeNode.Expand();
            return treeNode;
        }

        private TreeNode CreateSyntaxTreeNode(AstNode astNode)
        {
            var treeNode = new TreeNode(astNode is AstToken || astNode is Comment ? 
                astNode.ToString() :
                $"{astNode.Title.Name} : {astNode}");

            treeNode.Tag = astNode;

            PopulateTreeNode(treeNode, astNode);
            //treeNode.Expand();
            return treeNode;
        }

        private void PopulateTreeNode(TreeNode treeNode, AstNode astNode)
        {
            foreach (var child in astNode.Children)
                treeNode.Nodes.Add(CreateSyntaxTreeNode(child));
        }

        private void PopulateTreeNode(TreeNode treeNode, ParserNode parserNode)
        {
            foreach (var child in parserNode.Children)
                treeNode.Nodes.Add(CreateParserTreeNode(child));
        }

        private void UpdateSyntaxTree(ITextSource source)
        {
            astTreeView.BeginUpdate();
            rawParserTreeView.BeginUpdate();
            
            listView1.Items.Clear();
            astTreeView.Nodes.Clear();
            rawParserTreeView.Nodes.Clear();
            
            var stopwatch = new Stopwatch();

            try
            {
                _assembly.CompilationUnits.Remove(_compilationUnit);
                stopwatch.Start();
                _document = new ReadOnlyDocument(source.CreateSnapshot());

#if INITIALIZE_OWN_PARSER
                _rootParserNode = _parser.Parse(
                    new AstTokenStream(new VisualBasicLexer(_document.CreateReader())));
                stopwatch.Stop();
                rawParserTreeView.Nodes.Add(CreateParserTreeNode(_rootParserNode));
                _compilationUnit = (CompilationUnit)_rootParserNode.CreateAstNode();
#else
                _compilationUnit = CurrentLanguage.Parse(_document.CreateDocumentSnapshot());
                stopwatch.Stop();
#endif
                
                _assembly.CompilationUnits.Add(_compilationUnit);
                foreach (var error in _compilationUnit.Errors)
                {
                    listView1.Items.Add(new ListViewItem(new string[]
                    {
                        error.Severity.ToString(),
                        error.Message,
                        error.Location.ToString()
                    }));
                }

                label2.Text = string.Format("Parsing completed in: {0} ticks ({1} ms)", stopwatch.ElapsedTicks,
                    stopwatch.ElapsedMilliseconds);

                astTreeView.Nodes.Add(CreateSyntaxTreeNode(_compilationUnit));
                
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                MessageBox.Show(ex.ToString());
            }

            astTreeView.EndUpdate();
            rawParserTreeView.EndUpdate();
        }


        private static TreeNode FindTreeNode(TreeNode container, AstNode astNode)
        {
            if (container.Tag == astNode)
                return container;
            
            foreach (TreeNode node in container.Nodes)
            {
                var target = FindTreeNode(node, astNode);
                if (target != null)
                    return target;
            }

            return null;
        }

        private void ParseButtonOnClick(object sender, EventArgs e)
        {
            UpdateSyntaxTree(new StringTextSource(sourceTextBox.Text));
            findInTreeButton.PerformClick();
        }

        private void AstTreeViewOnAfterSelect(object sender, TreeViewEventArgs e)
        {
            var currentNode = e.Node.Tag as AstNode;
            if (currentNode != null && currentNode.Range != TextRange.Empty)
            {
                sourceTextBox.SelectionStart = _document.LocationToOffset(currentNode.Range.Start);
                sourceTextBox.SelectionLength = _document.GetRangeTextLength(currentNode.Range);
            }
        }

        private void CheckBoxOnCheckedChanged(object sender, EventArgs e)
        {
            sourceTextBox.WordWrap = checkBox1.Checked;
        }

        private void GenerateSourceButtonOnClick(object sender, EventArgs e)
        {
            if (astTreeView.Nodes.Count == 0)
                return;
            
            var stopwatch = new Stopwatch();

            using (var stringWriter = new StringWriter())
            {
                var textOutput = new StringTextOutput(stringWriter, "   ");
                var formatter = new TextOutputFormatter(textOutput);
                var codeWriter = CurrentLanguage.CreateWriter(formatter);

                stopwatch.Start();
                _compilationUnit.AcceptVisitor(codeWriter);
                stopwatch.Stop();

                label1.Text = string.Format("Generating source completed in: {0} ticks ({1} ms)", stopwatch.ElapsedTicks, stopwatch.ElapsedMilliseconds);
                sourceTextBox.Text = stringWriter.ToString();
            }
        }

        private void AddTypeButtonOnClick(object sender, EventArgs e)
        {
            
        }

        private void AddFieldButtonOnClick(object sender, EventArgs e)
        {
            if (astTreeView.Nodes.Count == 0)
                return;

            var tree = astTreeView.Nodes[0].Tag as CompilationUnit;
            var namespaceNode = tree.GetChildren<NamespaceDeclaration>().First();
            var typeNode = namespaceNode.Types.First();

            var newField = new FieldDeclaration("_dynamicField", new PrimitiveTypeReference(PrimitiveType.Int32));

            newField.ModifierElements.AddRange(new[]
                {
                    new ModifierElement(Modifier.Internal),
                    new ModifierElement(Modifier.Static),
                });

            
            typeNode.Members.Add(newField);
        }

        private void FindInTreeButtonOnClick(object sender, EventArgs e)
        {
            if (astTreeView.Nodes.Count == 0)
                return;

            var rootTreeNode = astTreeView.Nodes[0];
            var currentNode = _compilationUnit.GetNodeAtLocation(_document.OffsetToLocation(sourceTextBox.SelectionStart));
            var treeNode = FindTreeNode(rootTreeNode, currentNode);
            if (treeNode != null)
            {
                treeNode.EnsureVisible();
                astTreeView.SelectedNode = treeNode;
                astTreeView.Focus();
            }
        }
        
        private void ResolveButtonOnClick(object sender, EventArgs e)
        {
            var currentNode = astTreeView.SelectedNode?.Tag as AstNode;
            if (currentNode != null)
            {
                var scope = currentNode.GetDeclaringScope();
                if (scope == null)
                {
                    MessageBox.Show("Current AST node is not resolvable.");
                }
                else
                {
                    var resolvableNode = currentNode as IResolvable;

                    if (resolvableNode == null)
                        MessageBox.Show("Current AST node is not resolvable.");
                    else
                        new ResolveResultDialog(resolvableNode.Resolve(currentNode.GetDeclaringScope())).Show();
                }
            }
        }

        private void SourceTextBoxOnSelectionChanged(object sender, EventArgs e)
        {
            if (_document != null)
            {
                var textLocation = _document.OffsetToLocation(sourceTextBox.SelectionStart);
                label3.Text =
                    $"Offset: {sourceTextBox.SelectionStart}, Line: {textLocation.Line}, Column: {textLocation.Column}";
            }
        }

        private void SourceTextBoxOnTextChanged(object sender, EventArgs e)
        {
            _document = null;
        }
    }
}

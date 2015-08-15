using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AbstractCode.Ast.Parser.Compiler
{
    class Program
    {
        private static int Main(string[] args)
        {
            Console.WriteLine("AbstractCode parser automaton compiler utility.");
            Console.WriteLine();

            string filePath;
            if (args.Length > 0)
            {
                filePath = args[0];
            }
            else
            {
                Console.Write("Assembly: ");
                filePath = Console.ReadLine();
            }

            Assembly assembly = null;
            if (!PerformAction("Reading assembly", () => assembly = Assembly.LoadFile(filePath.Replace("\"", ""))))
                return 1;

            string typeName;
            if (args.Length > 1)
            {
                typeName = args[1];
            }
            else
            {
                Console.Write("Grammar type: ");
                typeName = Console.ReadLine();
            }

            Type grammarType = null;
            if (!PerformAction("Finding type", () => grammarType = assembly.GetType(typeName, true)))
                return 1;
            
            Grammar grammar = null;
            if (!PerformAction("Creating instance of grammar", () => grammar = (Grammar)Activator.CreateInstance(grammarType)))
                return 1;

            string outputFile;
            if (args.Length > 2)
            {
                outputFile = args[2];
            }
            else
            {
                Console.Write("Output file: ");
                outputFile = Console.ReadLine();
            }

            GrammarData data = null;
            if (!PerformAction("Collecting grammar data", () => data = new GrammarData(grammar)))
                return 1;

            GrammarCompilationResult result = null;
            if (!PerformAction("Constructing automaton", () => result = GrammarCompiler.Compile(data)))
                return 1;

            if (result.CompilationContext.Conflicts.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Warning: ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("{0} conflicts detected! Grammar might not be LALR(1).",
                    result.CompilationContext.Conflicts.Count);

                foreach (var conflict in result.CompilationContext.Conflicts)
                {
                    Console.WriteLine("State: {0}, lookahead: {1}, action 1: {2}, action 2: {3}", conflict.State.Id,
                        conflict.Lookahead.Name, conflict.Action1, conflict.Action2);
                }
            }

            if (!PerformAction("Serializing automaton", () =>
            {
                using (var stream = File.Create(outputFile))
                {
                    var serializer = new ParserAutomatonSerializer(data);
                    serializer.Serialize(stream, result.Automaton);
                }
            }))
                return 1;

            return 0;
        }

        private static bool PerformAction(string title, Action action)
        {
            var stopwatch = new Stopwatch();
            Console.Write(title + "...");
            stopwatch.Start();
            try
            {
                action();
                stopwatch.Stop();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" Done! ({0} ticks ({1}ms))", stopwatch.ElapsedTicks, stopwatch.ElapsedMilliseconds);
                Console.ForegroundColor = ConsoleColor.Gray;
                return true;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Failed!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}

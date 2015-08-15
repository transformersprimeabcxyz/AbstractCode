// This file is part of AbstractCode.
// 
// AbstractCode is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// AbstractCode is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with AbstractCode.  If not, see <http://www.gnu.org/licenses/>.
// 
using System;
using System.Linq;

namespace AbstractCode.Ast.Parser
{
    public class AutomatonSourceParser : SourceParser
    {
        public AutomatonSourceParser(Grammar grammar)
            : this (GrammarCompiler.Compile(new GrammarData(grammar)).Automaton)
        {
        }

        public AutomatonSourceParser(ParserAutomaton automaton)
        {
            if (automaton == null)
                throw new ArgumentNullException(nameof(automaton));
            Automaton = automaton;
        }

        public ParserAutomaton Automaton
        {
            get;
        }
        
        public override ParserNode Parse(AstTokenStream stream)
        {
            return Parse(stream, Automaton.DefaultInitialState);
        }

        public ParserNode Parse(AstTokenStream stream, GrammarDefinition rootDefinition)
        {
            return Parse(stream, Automaton.InitialStates[rootDefinition]);
        }

        public ParserNode Parse(AstTokenStream stream, ParserState initialState)
        {
            var context = new ParserContext(this, Automaton.Grammar, stream);
            context.CurrentState = initialState;
            context.ParserStack.Push(new ParserNode(new GrammarDefinition("init"))
            {
                State = initialState
            });

            context.CurrentNode = new ParserNode(Automaton.Grammar.ToElement(stream.Current.GetTokenCode()));
            context.CurrentNode.Token = stream.Current;
            while (stream.Current != null && context.Root == null)
            {
                // If no input and no default action (e.g. a grammar reduction),
                // that means we have to read the next token.
                if (context.CurrentNode == null && context.CurrentState.DefaultAction == null)
                {
                    context.SendLogMessage(MessageSeverity.Message, "Read next token");
                    stream.Advance();
                    context.CurrentNode = new ParserNode(Automaton.Grammar.ToElement(stream.Current.GetTokenCode()));
                    context.CurrentNode.Token = stream.Current;
                }

                GetNextAction(context).Execute(context);
            }

            return context.Root;
        }

        private ParserAction GetNextAction(ParserContext context)
        {
            if (context.CurrentState.DefaultAction != null)
            {
                return context.CurrentState.DefaultAction;
            }
            else
            {
                ParserAction parserAction;
                if (context.CurrentState.Actions.TryGetValue(context.CurrentNode.GrammarElement, out parserAction))
                {
                    return parserAction;
                }
                else
                {
                    var expectedTokens = context.CurrentState.Actions.Keys.OfType<TokenGrammarElement>().ToArray();
                    var expectedString = //expectedTokens.Length > 15
                       // ? string.Empty :
                       "Expected " + string.Join(" or ", expectedTokens.Select(x => x.Name));
                    

                    // Unexpected token or grammar pattern. Report syntax error.
                    var message = string.Format("Unexpected {0} token at line {1}, column {2}. " + expectedString,
                        context.CurrentNode.GrammarElement.Name, context.CurrentNode.Range.Start.Line, context.CurrentNode.Range.Start.Column);
                    context.SendLogMessage(MessageSeverity.Error, message);
                    throw new Exception(message);
                }

            }
        }
    }
}
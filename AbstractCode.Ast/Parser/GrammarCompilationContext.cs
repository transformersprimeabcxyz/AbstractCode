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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AbstractCode.Ast.Parser;

namespace AbstractCode.Ast.Parser
{
    public class GrammarCompilationContext
    {

        private readonly Dictionary<int[], IntermediateParserState> _itemIdStateMapping =
            new Dictionary<int[], IntermediateParserState>();

        public GrammarCompilationContext(GrammarData grammarData)
        {
            if (grammarData == null)
                throw new ArgumentNullException(nameof(grammarData));
            GrammarData = grammarData;
            States= new List<IntermediateParserState>();
            Conflicts = new List<GrammarReductionConflict>();
            InitialStates = new Dictionary<GrammarDefinition, IntermediateParserState>();
            AllAnnotatedElements = new List<AnnotatedGrammarElement>();
        }

        public GrammarData GrammarData
        {
            get;
        }
        
        public IList<IntermediateParserState> States
        {
            get;
        }

        public IDictionary<GrammarDefinition, IntermediateParserState> InitialStates
        {
            get;
        }
        
        public IList<AnnotatedGrammarElement> AllAnnotatedElements
        {
            get;
        } 

        public IList<GrammarReductionConflict> Conflicts
        {
            get;
        }

        public  IntermediateParserState GetOrCreateState(IList<Lr0Item> kernels)
        {
            // Sort the LR(0) items to avoid the creation of states 
            // that have the same LR(0) items but in a different order.
            var identifiers = (from item in kernels
                orderby item.Id
                select item.Id).ToArray();

            var state = _itemIdStateMapping.FirstOrDefault(
                    x => x.Key.Length == identifiers.Length 
                    && x.Key.SequenceEqual(identifiers)).Value;

            if (state == null)
            {
                state = new IntermediateParserState(this, new ParserState());
                state.State.Intermediate = state;
                state.State.Id = States.Count;

                foreach (var item in kernels)
                    state.RegisterItemAndChildren(this, item);

                States.Add(state);

                _itemIdStateMapping.Add(identifiers, state);
            }

            return state;
        }
        
        public ParserTransition GetOrCreateTransition(IntermediateParserState source, GrammarElement element,
            IntermediateParserState destination)
        {
            var transition = source.OutgoingTransitions.FirstOrDefault(x => x.Destination == destination);
            if (transition == null)
            {
                transition = new ParserTransition(source, element, destination);
                source.OutgoingTransitions.Add(transition);
                destination.IncomingTransitions.Add(transition);
            }

            return transition;
        }

        public AnnotatedGrammarElement GetOrCreateAnnotatedElement(GrammarElement element, ParserTransition transition)
        {
            // TODO: find a better alternative for avoiding duplicate elements. This one is rather cpu usage expensive.
            var annotatedElement = AllAnnotatedElements.FirstOrDefault(x => x.Element == element && x.Transition == transition);
            if (annotatedElement == null)
            {
                annotatedElement = new AnnotatedGrammarElement(element, transition);
                AllAnnotatedElements.Add(annotatedElement);
            }

            return annotatedElement;
        }
    }
}
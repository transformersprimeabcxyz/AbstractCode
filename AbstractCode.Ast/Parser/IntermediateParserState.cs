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
using System.Collections.Generic;
using System.Linq;

namespace AbstractCode.Ast.Parser
{
    public class IntermediateParserState
    {
        public IntermediateParserState(GrammarCompilationContext context, ParserState state)
        {
            Context = context;
            State = state;
            Items = new List<Lr1Item>();
            IncomingTransitions = new List<ParserTransition>();
            OutgoingTransitions = new List<ParserTransition>();
        }

        public GrammarCompilationContext Context
        {
            get;
        }

        public ParserState State
        {
            get;
        }

        public IList<Lr1Item> Items
        {
            get;
        }

        public IList<ParserTransition> IncomingTransitions
        {
            get;
        }

        public IList<ParserTransition> OutgoingTransitions
        {
            get;
        }

        public IEnumerable<Lr0Item> GetAllKernels()
        {
            return Items.Select(x => x.Kernel);
        }

        public IEnumerable<Lr1Item> GetShifters()
        {
            return from item in Items
                   where !item.Kernel.IsFinalizer
                   select item;
        }

        public IEnumerable<Lr1Item> GetFinalizers()
        {
            return from item in Items
                where item.Kernel.IsFinalizer
                select item;
        }

        public void RegisterItemAndChildren(GrammarCompilationContext context, Lr0Item item)
        {
            // Avoid infinite recursion.
            if (GetAllKernels().Contains(item))
                return;

            var lr1Item = new Lr1Item(this, item);
            Items.Add(lr1Item);

            var definition = item.Element as GrammarDefinition;
            if (definition == null)
                return;

            var reductions = context.GrammarData.GrammarReductionsMapping[definition];
            foreach (var reduction in reductions)
            {
                RegisterItemAndChildren(context, reduction.Lr0Items[0]);
            }
        }

        public ParserTransition GetOutgoingTransition(GrammarElement element)
        {
            return OutgoingTransitions.FirstOrDefault(x => x.Element == element);
        }

        public override string ToString()
        {
            return State.ToString();
        }
    }
}
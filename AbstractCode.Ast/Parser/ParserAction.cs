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
    // Algorithms based on https://en.wikipedia.org/wiki/LR_parser#LR_parser_loop

    public abstract class ParserAction
    {
        protected internal ParserAction()
        {
        }

        public abstract void Execute(ParserContext context);
    }

    public sealed class ShiftParserAction : ParserAction
    {
        public ShiftParserAction(ParserState nextState)
        {
            NextState = nextState;
        }

        public ParserState NextState
        {
            get;
        }

        public override void Execute(ParserContext context)
        {
            context.SendLogMessage(MessageSeverity.Message, ToString());
            context.ParserStack.Push(context.CurrentNode);
            context.CurrentState = context.CurrentNode.State = NextState;
            context.CurrentNode = null;
        }

        public override string ToString()
        {
            return string.Format("Shift to {0}", NextState);
        }
    }

    public sealed class ReduceParserAction : ParserAction
    {
        public ReduceParserAction(GrammarReduction reduction)
        {
            Reduction = reduction;
        }

        public GrammarReduction Reduction
        {
            get;
        }

        public override void Execute(ParserContext context)
        {
            context.SendLogMessage(MessageSeverity.Message, ToString());
            var temp = context.CurrentNode;

            var node = context.CurrentNode = CreateNode(context);
            context.CurrentState = context.ParserStack.Peek().State;

            var action = context.CurrentState.Actions[node.GrammarElement];
            action.Execute(context);

            context.CurrentNode = temp;
        }

        private ParserNode CreateNode(ParserContext context)
        {
            var node = new ParserNode(Reduction.Product);
            node.Children.AddRange(PopChildren(context).Reverse());
            return node;
        }

        private IEnumerable<ParserNode> PopChildren(ParserContext context)
        {
            var childCount = Reduction.Sequence.Count;
            for (int i = 0; i < childCount; i++)
                yield return context.ParserStack.Pop();
        }

        public override string ToString()
        {
            return "Apply grammar rule " + Reduction;
        }
    }

    public sealed class AcceptParserAction : ParserAction
    {
        public override void Execute(ParserContext context)
        {
            context.SendLogMessage(MessageSeverity.Message, ToString());
            context.Root = context.ParserStack.Pop();
            context.CurrentNode = null;

            context.CurrentState = context.ParserStack.Peek().State;
        }

        public override string ToString()
        {
            return "Accept";
        }
    }

    public sealed class ErrorParserAction : ParserAction
    {
        public override void Execute(ParserContext context)
        {
            

        }

        public override string ToString()
        {
            return "Report error";
        }
    }

}
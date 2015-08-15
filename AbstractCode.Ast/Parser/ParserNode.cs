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

namespace AbstractCode.Ast.Parser
{
    public class ParserNode
    {
        public ParserNode(GrammarElement grammarElement)
        {
            GrammarElement = grammarElement;
            Children = new List<ParserNode>();
        }

        public List<ParserNode> Children
        {
            get;
        }

        public bool HasChildren
        {
            get { return Children.Count != 0; }
        }

        public GrammarElement GrammarElement
        {
            get;
        }

        public GrammarDefinition GrammarDefinition
        {
            get { return GrammarElement as GrammarDefinition; }
        }

        public AstToken Token
        {
            get;
            set;
        }

        public ParserState State
        {
            get;
            set;
        }

        public TextRange Range
        {
            get
            {
                if (Token != null)
                    return Token.Range;
                if (Children.Count == 1)
                    return Children[0].Range;
                if (Children.Count > 1)
                    return new TextRange(Children[0].Range.Start, Children[Children.Count - 1].Range.End);
                return TextRange.Empty;
            }
        }

        public AstNode CreateAstNode()
        {
            if (GrammarDefinition == null)
            {
                if (Token != null)
                    return Token;
                throw new InvalidOperationException();
            }
            return GrammarDefinition.CreateNode(this);
        }

        public TAstNode CreateAstNode<TAstNode>()
            where TAstNode : AstNode
        {
            return (TAstNode)CreateAstNode();
        }

        public IEnumerable<ParserNode> GetAllNodesFromListDefinition()
        {
            if (GrammarDefinition == null)
                throw new InvalidOperationException();

            var containerStack = new Stack<ParserNode>();
            containerStack.Push(this);

            var currentNode = this;
            while (currentNode.Children.Count > 0
                   && currentNode.Children[0].GrammarElement == GrammarDefinition)
                containerStack.Push(currentNode = currentNode.Children[0]);

            while (containerStack.Count > 0)
            {
                var currentContainer = containerStack.Peek();
                for (int i = 0; i < currentContainer.Children.Count; i++)
                {
                    if (currentContainer.Children[i].GrammarElement != GrammarDefinition)
                        yield return currentContainer.Children[i];
                }
                containerStack.Pop();
            }
        }

        public override string ToString()
        {
            if (Token != null)
                return string.Format("{0} ({1})", Token, GrammarElement);
            return GrammarElement.ToString();
        }
    }
}
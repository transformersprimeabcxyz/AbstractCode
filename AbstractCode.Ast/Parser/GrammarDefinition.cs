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
    public class GrammarDefinition : GrammarElement
    {
        public GrammarDefinition(string name)
            : this(name, null)
        {
            
        }

        public GrammarDefinition(string name, GrammarExpression rule)
            : this(name, rule, node => node.Children[0].CreateAstNode())
        {
        }

        public GrammarDefinition(string name, GrammarExpression rule, CreateAstNodeDelegate createNode) 
            : base(name)
        {
            Rule = rule;
            CreateNode = createNode;
        }

        public GrammarExpression Rule
        {
            get;
            set;
        }

        public CreateAstNodeDelegate CreateNode
        {
            get;
            set;
        }

        public static implicit operator GrammarExpression(GrammarDefinition a)
        {
            return new GrammarExpression(a);
        }

        public override IEnumerable<TokenGrammarElement> GetFirstItems(IList<GrammarElement> alreadyTraversedItems)
        {
            if (alreadyTraversedItems.Contains(this))
                return Enumerable.Empty<TokenGrammarElement>();
            alreadyTraversedItems.Add(this);
            return Rule.GetFirstItems(alreadyTraversedItems);
        }
    }
}
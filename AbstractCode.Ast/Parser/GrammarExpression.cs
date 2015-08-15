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
using AbstractCode.Ast.Expressions;

namespace AbstractCode.Ast.Parser
{

    public class GrammarExpression : GrammarElement
    {
        public GrammarExpression(GrammarElement element) : this()
        {
            if (element != null)
                Switch[0].Add(element);
        }

        public GrammarExpression()
            : base(null)
        {
            Switch = new GrammarElementSwitch();
            Switch.Add(new GrammarElementSequence());
        }

        public GrammarElementSwitch Switch
        {
            get;
        }

        public static implicit operator GrammarExpression(TokenGrammarElement element)
        {
            return new GrammarExpression(element);
        }

        public override IEnumerable<TokenGrammarElement> GetFirstItems(IList<GrammarElement> alreadyTraversedItems)
        {
            return Switch.SelectMany(sequence => sequence.Count == 0
                ? new[] { Grammar.Epsilon }
                : sequence[0].GetFirstItems(alreadyTraversedItems)).Distinct();
        }

        public override string ToString()
        {
            return Switch.ToString();
        }
    }

}

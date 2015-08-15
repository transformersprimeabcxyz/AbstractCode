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

namespace AbstractCode.Ast.Parser
{
    public class Lr1Item
    {
        public Lr1Item(IntermediateParserState state, Lr0Item kernel)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));
            if (kernel == null)
                throw new ArgumentNullException(nameof(kernel));
            State = state;
            Kernel = kernel;
            Lookahead = new HashSet<TokenGrammarElement>();
        }
        
        public IntermediateParserState State
        {
            get;
            set;
        }

        public Lr0Item Kernel
        {
            get;
            set;
        }

        public HashSet<TokenGrammarElement> Lookahead
        {
            get;
            set;
        }

        public Lr1Item NextItem
        {
            get;
            set;
        }
        
        public Lr1Item GetFinalizer()
        {
            var current = this;
            while (current.NextItem != null)
                current = current.NextItem;
            return current;
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Kernel, string.Join(", ", Lookahead.Select(x => x.ToString())));
        }
    }
}

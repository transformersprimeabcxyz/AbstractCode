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
namespace AbstractCode.Ast.Parser
{
    public class GrammarReductionConflict
    {
        public GrammarReductionConflict(ParserState state, TokenGrammarElement lookahead, ParserAction action1, ParserAction action2)
        {
            State = state;
            Lookahead = lookahead;
            Action1 = action1;
            Action2 = action2;
        }

        public ParserState State
        {
            get;
        }

        public TokenGrammarElement Lookahead
        {
            get;
        }

        public ParserAction Action1
        {
            get;
        }

        public ParserAction Action2
        {
            get;
        }
    }
}
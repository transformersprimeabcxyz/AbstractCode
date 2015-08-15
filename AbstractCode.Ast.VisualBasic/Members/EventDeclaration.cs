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

using EventDeclarationBase = AbstractCode.Ast.Members.EventDeclaration;

namespace AbstractCode.Ast.VisualBasic.Members
{
    public class EventDeclaration : EventDeclarationBase
    {
        private readonly AstNodeTitle<AstToken> AsKeywordTitle = new AstNodeTitle<AstToken>("AsKeyword");

        public EventDeclaration()
        {
        }

        public AstToken AsKeyword
        {
            get { return GetChildByTitle(AsKeywordTitle); }
            set { SetChildByTitle(AsKeywordTitle, value); }
        }
    }
}
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

using AbstractCode.Ast.Members;
using AbstractCode.Ast.Types;
using VariableDeclaratorBase = AbstractCode.Ast.Members.VariableDeclarator;

namespace AbstractCode.Ast.VisualBasic.Members
{
    public class VariableDeclarator : VariableDeclaratorBase
    {
        public static readonly AstNodeTitle<AstToken> AsKeywordTitle = new AstNodeTitle<AstToken>("AsKeyword");

        public VariableDeclarator()
        {
        }

        public VariableDeclarator(Identifier identifier)
            : base(identifier)
        {
        }

        public AstToken AsKeyword
        {
            get { return GetChildByTitle(AsKeywordTitle); }
            set { SetChildByTitle(AsKeywordTitle, value); }
        }

        public TypeReference VariableType
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public bool HasVariableType
        {
            get { return VariableType != null; }
        }
    }
}
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

namespace AbstractCode.Ast.VisualBasic
{
    public class VisualBasicAstToken : AstToken
    {
        public VisualBasicAstToken(VisualBasicAstTokenCode code, string value, TextRange range)
            : base(value, range)
        {
            Code = code;
        }

        public VisualBasicAstTokenCode Code
        {
            get;
            set;
        }

        public override int GetTokenCode()
        {
            return (int)Code;
        }

        public override bool Match(AstNode other)
        {
            var token = other as VisualBasicAstToken;
            return token != null && Code == token.Code;
        }
    }
}
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

namespace AbstractCode.Ast
{
    public abstract class AstToken : AstNode
    {
        protected AstToken()
        {
        }

        protected AstToken(string value, TextRange range)
        {
            Value = value;
            Range = range;
        }

        public string Value
        {
            get;
        }

        public abstract int GetTokenCode();
        
        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitAstToken(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitAstToken(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitAstToken(this, data);
        }

        public override string ToString()
        {
            return Value;
        }
        
        
    }
}

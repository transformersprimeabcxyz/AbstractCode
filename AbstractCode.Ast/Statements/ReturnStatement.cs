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

using AbstractCode.Ast.Expressions;

namespace AbstractCode.Ast.Statements
{
    public class ReturnStatement : Statement
    {
        public ReturnStatement()
        {
        }

        public ReturnStatement(Expression value)
        {
            Value = value;
        }

        public AstToken ReturnKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public Expression Value
        {
            get { return GetChildByTitle(AstNodeTitles.ValueExpression); }
            set { SetChildByTitle(AstNodeTitles.ValueExpression, value); }
        }
        
        public override string ToString()
        {
            return string.Format("{0}{1}", GetType().Name, Value == null ? "" : string.Format(" (Value = {0})", Value));
        }

        public override bool Match(AstNode other)
        {
            var statement = other as ReturnStatement;
            return statement != null
                   && Value.MatchOrNull(statement.Value);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitReturnStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitReturnStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitReturnStatement(this, data);
        }

        
    }
}

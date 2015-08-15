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

namespace AbstractCode.Ast.VisualBasic.Members
{
    public class HandlesClause : AstNode, IVisualBasicVisitable
    {
        public HandlesClause()
        {
        }

        public HandlesClause(Expression eventExpression)
        {
            EventExpression = eventExpression;
        }

        public AstToken HandlesKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public Expression EventExpression
        {
            get { return GetChildByTitle(AstNodeTitles.Expression); }
            set { SetChildByTitle(AstNodeTitles.Expression, value); }
        }

        public override bool Match(AstNode other)
        {
            var clause = other as HandlesClause;
            return clause != null
                   && EventExpression.MatchOrNull(clause.EventExpression);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            this.AcceptVisualBasicVisitor(visitor);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return this.AcceptVisualBasicVisitor(visitor);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return this.AcceptVisualBasicVisitor(visitor, data);
        }

        public void AcceptVisitor(IVisualBasicAstVisitor visitor)
        {
            visitor.VisitHandlesClause(this);
        }

        public TResult AcceptVisitor<TResult>(IVisualBasicAstVisitor<TResult> visitor)
        {
            return visitor.VisitHandlesClause(this);
        }

        public TResult AcceptVisitor<TData, TResult>(IVisualBasicAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitHandlesClause(this, data);
        }
    }
}
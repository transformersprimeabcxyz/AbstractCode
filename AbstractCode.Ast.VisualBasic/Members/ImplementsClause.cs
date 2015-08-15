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
    public class ImplementsClause : AstNode, IVisualBasicVisitable
    {
        public static readonly AstNodeTitle<MemberReferenceExpression> MemberExpressionTitle =
            new AstNodeTitle<MemberReferenceExpression>("MemberExpression");

        public ImplementsClause()
        {
        }

        public ImplementsClause(MemberReferenceExpression memberExpression)
        {
            MemberExpression = memberExpression;
        }

        public AstToken ImplementsKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public MemberReferenceExpression MemberExpression
        {
            get { return GetChildByTitle(MemberExpressionTitle); }
            set { SetChildByTitle(MemberExpressionTitle, value); }
        }

        public override bool Match(AstNode other)
        {
            var clause = other as ImplementsClause;
            return clause != null
                   && MemberExpression.MatchOrNull(clause.MemberExpression);
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
            visitor.VisitImplementsClause(this);
        }

        public TResult AcceptVisitor<TResult>(IVisualBasicAstVisitor<TResult> visitor)
        {
            return visitor.VisitImplementsClause(this);
        }

        public TResult AcceptVisitor<TData, TResult>(IVisualBasicAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitImplementsClause(this, data);
        }
    }
}
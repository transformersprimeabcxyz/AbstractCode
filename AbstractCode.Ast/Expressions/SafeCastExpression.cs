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

using AbstractCode.Ast.Types;

namespace AbstractCode.Ast.Expressions
{
    public class SafeCastExpression : CastExpression
    {
        public SafeCastExpression()
        {

        }

        public SafeCastExpression(TypeReference targetType, Expression targetExpression)
        {
            TargetType = targetType;
            TargetExpression = targetExpression;
        }

        public AstToken CastKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public override bool Match(AstNode other)
        {
            var expression = other as SafeCastExpression;
            return expression != null
                   && TargetType.MatchOrNull(expression.TargetType)
                   && TargetExpression.MatchOrNull(expression.TargetExpression);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitSafeCastExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitSafeCastExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitSafeCastExpression(this, data);
        }
    }
}

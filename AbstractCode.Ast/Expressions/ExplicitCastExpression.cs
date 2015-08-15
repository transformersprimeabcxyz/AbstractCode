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
    public class ExplicitCastExpression : CastExpression
    {
        public ExplicitCastExpression()
        {
        }

        public ExplicitCastExpression(TypeReference targetType, Expression targetExpression)
        {
            TargetType = targetType;
            TargetExpression = targetExpression;
        }

        public override bool Match(AstNode other)
        {
            var expression = other as ExplicitCastExpression;
            return expression != null
                   && TargetType.MatchOrNull(expression.TargetType)
                   && TargetExpression.MatchOrNull(expression.TargetExpression);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitExplicitCastExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitExplicitCastExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitExplicitCastExpression(this, data);
        }
                
    }
}

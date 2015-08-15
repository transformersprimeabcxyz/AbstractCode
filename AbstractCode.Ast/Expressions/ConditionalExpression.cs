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

namespace AbstractCode.Ast.Expressions
{
    public class ConditionalExpression : Expression
    {
        public static readonly AstNodeTitle<Expression> TrueExpressionTitle = new AstNodeTitle<Expression>("TrueExpression");
        public static readonly AstNodeTitle<Expression> FalseExpressionTitle = new AstNodeTitle<Expression>("FalseExpression");

        public ConditionalExpression()
        {

        }

        public ConditionalExpression(Expression condition, Expression trueExpression, Expression falseExpression)
        {
            Condition = condition;
            TrueExpression = trueExpression;
            FalseExpression = falseExpression;
        }

        public Expression Condition
        {
            get { return GetChildByTitle(AstNodeTitles.Condition); }
            set { SetChildByTitle(AstNodeTitles.Condition, value); }
        }

        public Expression TrueExpression
        {
            get { return GetChildByTitle(TrueExpressionTitle); }
            set { SetChildByTitle(TrueExpressionTitle, value); }
        }

        public Expression FalseExpression
        {
            get { return GetChildByTitle(FalseExpressionTitle); }
            set { SetChildByTitle(FalseExpressionTitle, value); }
        }

        public override bool Match(AstNode other)
        {
            var expression = other as ConditionalExpression;
            return expression != null
                   && Condition.MatchOrNull(expression.Condition)
                   && TrueExpression.MatchOrNull(expression.TrueExpression)
                   && FalseExpression.MatchOrNull(expression.FalseExpression);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitConditionalExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitConditionalExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitConditionalExpression(this, data);
        }
    }
}

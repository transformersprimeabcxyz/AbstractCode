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
    public class UnaryOperatorExpression : Expression
    {
        public UnaryOperatorExpression()
        {

        }

        public UnaryOperatorExpression(UnaryOperator @operator, Expression expression)
        {
            Operator = @operator;
            Expression = expression;
        }

        public AstToken OperatorToken
        {
            get { return GetChildByTitle(AstNodeTitles.Operator); }
            set { SetChildByTitle(AstNodeTitles.Operator, value); }
        }

        public virtual UnaryOperator Operator
        {
            get;
            set;
        }

        public Expression Expression
        {
            get { return GetChildByTitle(AstNodeTitles.Expression); }
            set { SetChildByTitle(AstNodeTitles.Expression, value); }
        }

        public override string ToString()
        {
            return string.Format("{0} (Operator = {1})", GetType().Name, Operator);
        }

        public override bool Match(AstNode other)
        {
            var expression = other as UnaryOperatorExpression;
            return expression != null
                   && Expression.MatchOrNull(expression.Expression)
                   && Operator == expression.Operator;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitUnaryOperatorExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitUnaryOperatorExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitUnaryOperatorExpression(this, data);
        }
    }

    public enum UnaryOperator
    {
        Positive,
        Negative,
        Not,
        BitwiseNot,
        PostIncrement,
        PostDecrement,
        Dereference,
        AddressOf,
        Await,
        PreDecrement,
        PreIncrement
    }
}

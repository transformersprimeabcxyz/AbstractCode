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
    public class BinaryOperatorExpression : Expression
    {
        public BinaryOperatorExpression()
        {
        }

        public BinaryOperatorExpression(Expression left, BinaryOperator @operator, Expression right)
            : this()
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public Expression Left
        {
            get { return GetChildByTitle(AstNodeTitles.TargetExpression); }
            set { SetChildByTitle(AstNodeTitles.TargetExpression, value); }
        }

        public AstToken OperatorToken
        {
            get { return GetChildByTitle(AstNodeTitles.Operator); }
            set { SetChildByTitle(AstNodeTitles.Operator, value); }
        }

        public virtual BinaryOperator Operator
        {
            get;
            set;
        }

        public Expression Right
        {
            get { return GetChildByTitle(AstNodeTitles.ValueExpression); }
            set { SetChildByTitle(AstNodeTitles.ValueExpression, value); }
        }

        public override string ToString()
        {
            return string.Format("{0} (Operator = {1})", GetType().Name, Operator);
        }

        public override bool Match(AstNode other)
        {
            var expression = other as BinaryOperatorExpression;
            return expression != null
                   && Left.MatchOrNull(expression.Left)
                   && Operator == expression.Operator
                   && Right.MatchOrNull(expression.Right);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitBinaryOperatorExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitBinaryOperatorExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitBinaryOperatorExpression(this, data);
        }
    }

    public enum BinaryOperator
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Modulus,

        Equals,
        NotEquals,

        GreaterThan,
        GreaterThanOrEquals,
        LessThan,
        LessThanOrEquals,

        LogicalAnd,
        LogicalOr,
        NullCoalescing,

        BitwiseAnd,
        BitwiseOr,
        BitwiseXor,

        ShiftLeft,
        ShiftRight,

        // Only in VB:
        Concat,
        IntegerDivide,
        RaisePower,
        Like,
    }
}

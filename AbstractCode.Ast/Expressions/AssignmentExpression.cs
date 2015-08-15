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

using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.Ast.Expressions
{
    public class AssignmentExpression : Expression
    {
        public AssignmentExpression()
        {
        }

        public AssignmentExpression(Expression target, AssignmentOperator @operator, Expression value)
        {
            Target = target;
            Operator = @operator;
            Value = value;
        }

        public Expression Target
        {
            get { return GetChildByTitle(AstNodeTitles.TargetExpression); }
            set { SetChildByTitle(AstNodeTitles.TargetExpression, value); }
        }

        public AstToken OperatorToken
        {
            get { return GetChildByTitle(AstNodeTitles.Operator); }
            set { SetChildByTitle(AstNodeTitles.Operator, value); }
        }

        public virtual AssignmentOperator Operator
        {
            get;
            set;
        }

        public Expression Value
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
            var expression = other as AssignmentExpression;
            return expression != null
                   && Target.MatchOrNull(expression.Target)
                   && Operator == expression.Operator
                   && Value.MatchOrNull(expression.Value);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitAssignmentExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitAssignmentExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitAssignmentExpression(this, data);
        }

        public override ResolveResult Resolve(IScope scope)
        {
            return Target.Resolve(scope);
        }
    }

    public enum AssignmentOperator
    {
        Assign,

        Add,
        Subtract,
        Multiply,
        Divide,
        Modulus,

        BitwiseAnd,
        BitwiseOr,
        BitwiseXor,

        ShiftLeft,
        ShiftRight,

        // Only in VB
        Concat,
        IntegerDivide,
        RaisePower,
    }
}

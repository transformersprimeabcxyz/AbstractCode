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
    public class AddRemoveHandlerStatement : Statement
    {
        public static readonly AstNodeTitle<Expression> EventExpressionTitle = new AstNodeTitle<Expression>("EventExpression");
        public static readonly AstNodeTitle<Expression> DelegateExpressionTitle = new AstNodeTitle<Expression>("DelegateExpression");
        public static readonly AstNodeTitle<AstToken> VariantElementTitle = new AstNodeTitle<AstToken>("VariantToken");
      
        public AddRemoveHandlerStatement()
        {

        }

        public AddRemoveHandlerStatement(Expression eventExpression, Expression delegateExpression, AddRemoveHandlerVariant variant)
        {
            EventExpression = eventExpression;
            DelegateExpression = delegateExpression;
            Variant = variant;
        }

        public Expression EventExpression
        {
            get { return GetChildByTitle(EventExpressionTitle); }
            set { SetChildByTitle(EventExpressionTitle, value); }
        }

        public Expression DelegateExpression
        {
            get { return GetChildByTitle(DelegateExpressionTitle); }
            set { SetChildByTitle(DelegateExpressionTitle, value); }
        }

        public AstToken VariantToken
        {
            get { return GetChildByTitle(VariantElementTitle); }
            set { SetChildByTitle(VariantElementTitle, value); }
        }

        public virtual AddRemoveHandlerVariant Variant
        {
            get;
            set;
        }

        public override bool Match(AstNode other)
        {
            var statement = other as AddRemoveHandlerStatement;
            return statement != null
                   && EventExpression.MatchOrNull(statement.EventExpression)
                   && Variant == statement.Variant
                   && DelegateExpression.Match(statement.DelegateExpression);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitAddRemoveHandlerStatement(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitAddRemoveHandlerStatement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitAddRemoveHandlerStatement(this, data);
        }
    }

    public enum AddRemoveHandlerVariant
    {
        Add,
        Remove,
    }
}

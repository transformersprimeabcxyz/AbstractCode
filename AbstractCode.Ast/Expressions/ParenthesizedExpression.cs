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
    public class ParenthesizedExpression : Expression
    {
        public ParenthesizedExpression()
        {
        }

        public ParenthesizedExpression(Expression expression)
        {
            this.Expression = expression;
        }

        public AstToken LeftParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.LeftParenthese); }
            set { SetChildByTitle(AstNodeTitles.LeftParenthese, value); }
        }

        public Expression Expression
        {
            get { return GetChildByTitle(AstNodeTitles.Expression); }
            set { SetChildByTitle(AstNodeTitles.Expression, value); }
        }

        public AstToken RightParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.RightParenthese); }
            set { SetChildByTitle(AstNodeTitles.RightParenthese, value); }
        }

        public override bool Match(AstNode other)
        {
            var expression = other as ParenthesizedExpression;
            return expression != null && Expression.MatchOrNull(expression.Expression);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitParenthesizedExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitParenthesizedExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitParenthesizedExpression(this, data);
        }

        public override ResolveResult Resolve(IScope scope)
        {
            return Expression.Resolve(scope);
        }
    }
}

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
    public class DirectionExpression : Expression
    {
        public static readonly AstNodeTitle<AstToken> DirectionElementTitle = new AstNodeTitle<AstToken>("DirectionToken");

        public DirectionExpression()
        {
        }

        public DirectionExpression(FieldDirection direction, Expression expression)
        {
            Direction = direction;
            Expression = expression;
        }

        public AstToken DirectionToken
        {
            get { return GetChildByTitle(DirectionElementTitle); }
            set { SetChildByTitle(DirectionElementTitle, value); }
        }

        public virtual FieldDirection Direction 
        {
            get;
            set; 
        }

        public Expression Expression
        {
            get { return GetChildByTitle(AstNodeTitles.Expression); }
            set { SetChildByTitle(AstNodeTitles.Expression, value); }
        }

        public override bool Match(AstNode other)
        {
            var expression = other as DirectionExpression;
            return expression != null
                   && Direction == expression.Direction
                   && Expression.Match(expression.Expression);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitDirectionExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitDirectionExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitDirectionExpression(this, data);
        }
    }

    public enum FieldDirection
    {
        None,
        Out,
        Ref,
    }
}

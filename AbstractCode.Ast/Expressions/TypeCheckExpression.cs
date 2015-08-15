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
    public class TypeCheckExpression : Expression
    {
        public TypeCheckExpression()
        {
        }

        public TypeCheckExpression(Expression targetExpression, TypeReference targetType)
        {
            TargetExpression = targetExpression;
            TargetType = targetType;
        }

        public Expression TargetExpression
        {
            get { return GetChildByTitle(AstNodeTitles.TargetExpression); }
            set { SetChildByTitle(AstNodeTitles.TargetExpression, value); }
        }

        public TypeReference TargetType
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public override bool Match(AstNode other)
        {
            var expression = other as TypeCheckExpression;
            return expression != null
                   && TargetExpression.MatchOrNull(expression.TargetExpression)
                   && TargetType.MatchOrNull(expression.TargetType);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitTypeCheckExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitTypeCheckExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitTypeCheckExpression(this, data);
        }
    }
}

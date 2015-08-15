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
    public class CreateObjectExpression : Expression
    {
        public CreateObjectExpression()
        {
            Arguments = new AstNodeCollection<Expression>(this, AstNodeTitles.Argument);
        }

        public CreateObjectExpression(TypeReference type)
            : this()
        {
            Type = type;
        }

        public CreateObjectExpression(TypeReference type, params Expression[] arguments)
            : this()
        {
            Type = type;
            Arguments.AddRange(arguments);
        }

        public AstToken NewKeyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public TypeReference Type
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public AstToken LeftParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.LeftParenthese); }
            set { SetChildByTitle(AstNodeTitles.LeftParenthese, value); }
        }

        public AstNodeCollection<Expression> Arguments
        {
            get;
        }

        public AstToken RightParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.RightParenthese); }
            set { SetChildByTitle(AstNodeTitles.RightParenthese, value); }
        }

        public ArrayInitializer Initializer
        {
            get { return GetChildByTitle(AstNodeTitles.ArrayInitializer); }
            set { SetChildByTitle(AstNodeTitles.ArrayInitializer, value); }
        }

        public override bool Match(AstNode other)
        {
            var expression = other as CreateObjectExpression;
            return expression != null
                   && Type.MatchOrNull(expression.Type)
                   && Arguments.Match(expression.Arguments)
                   && Initializer.MatchOrNull(expression.Initializer);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitCreateObjectExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitCreateObjectExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitCreateObjectExpression(this, data);
        }
    }
}

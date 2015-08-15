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
    public class CreateArrayExpression : Expression
    {
        public CreateArrayExpression()
        {
            Arguments = new AstNodeCollection<Expression>(this, AstNodeTitles.Argument);
        }

        public CreateArrayExpression(TypeReference type)
            : this()
        {
            Type = type;
        }

        public CreateArrayExpression(TypeReference type, params Expression[] arguments)
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

        public AstToken LeftBracket
        {
            get { return GetChildByTitle(AstNodeTitles.LeftBracket); }
            set { SetChildByTitle(AstNodeTitles.LeftBracket, value); }
        }

        public AstNodeCollection<Expression> Arguments
        {
            get;
        }

        public AstToken RightBracket
        {
            get { return GetChildByTitle(AstNodeTitles.RightBracket); }
            set { SetChildByTitle(AstNodeTitles.RightBracket, value); }
        }

        public ArrayInitializer Initializer
        {
            get { return GetChildByTitle(AstNodeTitles.ArrayInitializer); }
            set { SetChildByTitle(AstNodeTitles.ArrayInitializer, value); }
        }

        public override bool Match(AstNode other)
        {
            var expression = other as CreateArrayExpression;
            return expression != null
                   && expression.Type.MatchOrNull(expression.Type)
                   && expression.Arguments.Match(expression.Arguments)
                   && expression.Initializer.MatchOrNull(expression.Initializer);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitCreateArrayExpresion(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitCreateArrayExpresion(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitCreateArrayExpresion(this, data);
        }

    }
}

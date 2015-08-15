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

namespace AbstractCode.Ast.Members
{
    public class VariableDeclarator : AstNode
    {
        public VariableDeclarator()
        {

        }

        public VariableDeclarator(string identifier)
            : this(new Identifier(identifier))
        {
        }

        public VariableDeclarator(Identifier identifier)
            : this(identifier, null)
        {
        }

        public VariableDeclarator(string identifier, Expression value)
            : this(new Identifier(identifier), value)
        {
        }

        public VariableDeclarator(Identifier identifier, Expression value)
        {
            Identifier = identifier;
            Value = value;
        }

        public Identifier Identifier
        {
            get { return GetChildByTitle(AstNodeTitles.Identifier); }
            set { SetChildByTitle(AstNodeTitles.Identifier, value); }
        }

        public AstToken OperatorToken
        {
            get { return GetChildByTitle(AstNodeTitles.Operator); }
            set { SetChildByTitle(AstNodeTitles.Operator, value); }
        }

        public Expression Value
        {
            get { return GetChildByTitle(AstNodeTitles.ValueExpression); }
            set { SetChildByTitle(AstNodeTitles.ValueExpression, value); }
        }

        public bool HasValue
        {
            get { return Value != null; }
        }

        public override string ToString()
        {
            return string.Format("{0} (Name = {1})", GetType().Name, Identifier.Name);
        }

        public override bool Match(AstNode other)
        {
            var declarator = other as VariableDeclarator;
            return declarator != null
                   && Identifier.MatchOrNull(declarator.Identifier)
                   && Value.MatchOrNull(declarator.Value);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitVariableDeclarator(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitVariableDeclarator(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitVariableDeclarator(this, data);
        }
        
    }
}

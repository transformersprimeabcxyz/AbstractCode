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

using AbstractCode.Ast.Members;
using AbstractCode.Ast.Types;
using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;
using TypeReference = AbstractCode.Ast.Types.TypeReference;

namespace AbstractCode.Ast.Expressions
{
    public class IdentifierExpression : Expression, IConvertibleToType, IConvertibleToIdentifier
    {
        public IdentifierExpression()
        {

        }

        public IdentifierExpression(Identifier identifier)
        {
            Identifier = identifier;
        }

        public IdentifierExpression(string identifier)
            : this(new Identifier(identifier))
        {
        }

        public Identifier Identifier
        {
            get { return GetChildByTitle(AstNodeTitles.Identifier); }
            set { SetChildByTitle(AstNodeTitles.Identifier, value); }
        }

        public TypeReference ToTypeReference()
        {
            return Identifier.ToTypeReference();
        }

        public Identifier ToIdentifier()
        {
            return ((IConvertibleToIdentifier)Identifier).ToIdentifier();
        }
        
        public override string ToString()
        {
            return string.Format("{0} (Identifier = {1})", GetType().Name, Identifier.Name);
        }

        public override bool Match(AstNode other)
        {
            var expression = other as IdentifierExpression;
            return expression != null 
                && Identifier.Match(expression.Identifier);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitIdentifierExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitIdentifierExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitIdentifierExpression(this, data);
        }

        public override ResolveResult Resolve(IScope scope)
        {
            return Identifier?.Resolve(scope);
        }
    }
}

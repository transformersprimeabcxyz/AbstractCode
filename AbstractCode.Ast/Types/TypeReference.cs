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
using AbstractCode.Ast.Members;
using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.Ast.Types
{
    public abstract class TypeReference : AstNode, IConvertibleToType, IConvertibleToIdentifier, IConvertibleToExpression, IResolvable
    {
        public Identifier Identifier
        {
            get { return GetChildByTitle(AstNodeTitles.Identifier); }
            set { SetChildByTitle(AstNodeTitles.Identifier, value); }
        }

        TypeReference IConvertibleToType.ToTypeReference()
        {
            return this;
        }

        public Identifier ToIdentifier()
        {
            return new Identifier(FullName, Range);
        }

        public Expression ToExpression()
        {
            var identifierExpression = new IdentifierExpression(new Identifier(FullName, Range));
            if (Parent == null)
                return identifierExpression;
            return ReplaceWith(identifierExpression) as Expression;
        }

        public virtual TypeReference GetElementType()
        {
            return this;
        }

        public override string ToString()
        {
            return string.Format("{0} (FullName = {1})", GetType().Name, FullName);
        }

        public virtual string Name
        {
            get { return Identifier.Name; }
        }
            
        public abstract string FullName
        {
            get;
        }

        public abstract Symbols.TypeReference GetSymbolsReference();

        public ResolveResult Resolve(IScope scope)
        {
            return GetSymbolsReference().Resolve(scope);
        }
    }
}

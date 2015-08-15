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

using System.Collections.Generic;
using AbstractCode.Ast.Types;
using AbstractCode.Symbols;
using TypeReference = AbstractCode.Ast.Types.TypeReference;

namespace AbstractCode.Ast.Members
{
    public class ConstructorDeclaration : AbstractMethodDeclaration
    {
        private static readonly TypeReference ConstructorReturnType = new PrimitiveTypeReference(PrimitiveType.Void);

        public ConstructorDeclaration()
        {
        }

        public ConstructorDeclaration(IEnumerable<ParameterDeclaration> parameters)
        {
            Parameters.AddRange(parameters);
        }

        public override bool Match(AstNode other)
        {
            var declaration = other as ConstructorDeclaration;
            return declaration != null 
                && MatchModifiersAndAttributes(declaration)
                && Body.Match(declaration.Body);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitConstructorDeclaration(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitConstructorDeclaration(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitConstructorDeclaration(this, data);
        }

        public override MethodDefinition GetDefinition()
        {
            return null; //TODO
        }
    }
}

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

using System;
using System.Collections.Generic;
using ConstructorDeclarationBase = AbstractCode.Ast.Members.ConstructorDeclaration;

namespace AbstractCode.Ast.VisualBasic.Members
{
    public class ConstructorDeclaration : ConstructorDeclarationBase, IVBMethodDeclaration
    {
        public static readonly AstNodeTitle<AstToken> SubKeywordTitle = new AstNodeTitle<AstToken>("SubKeywordRole");

        public ConstructorDeclaration()
        {
        }

        public ConstructorDeclaration(IEnumerable<ParameterDeclaration> parameters)
            : base(parameters)
        {
        }

        public AstToken SubKeyword
        {
            get { return GetChildByTitle(SubKeywordTitle); }
            set { SetChildByTitle(SubKeywordTitle, value); }
        }

        MethodDeclarationVariant IVBMethodDeclaration.Variant
        {
            get { return MethodDeclarationVariant.Sub; }
            set { throw new InvalidOperationException(); }
        }

        AstToken IVBMethodDeclaration.VariantElement
        {
            get { return SubKeyword; }
            set { SubKeyword = value; }
        }

        bool IVBMethodDeclaration.CanHoldHandlesClause
        {
            get { return false; }
        }

        HandlesClause IVBMethodDeclaration.HandlesClause
        {
            get { return null; }
            set { throw new InvalidOperationException(); }
        }

        bool IVBMethodDeclaration.CanHoldImplementsClause
        {
            get { return false; }
        }

        ImplementsClause IVBMethodDeclaration.ImplementsClause
        {
            get { return null; }
            set { throw new InvalidOperationException(); }
        }
    }
}
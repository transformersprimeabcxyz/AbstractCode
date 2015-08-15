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
using DelegateDeclarationBase = AbstractCode.Ast.Members.DelegateDeclaration;

namespace AbstractCode.Ast.VisualBasic.Members
{
    public class DelegateDeclaration : DelegateDeclarationBase, IVBMethodDeclaration
    {
        public DelegateDeclaration()
        {
        }

        public MethodDeclarationVariant Variant
        {
            get;
            set;
        }

        public AstToken VariantElement
        {
            get { return GetChildByTitle(MethodDeclaration.VariantElementTitle); }
            set { SetChildByTitle(MethodDeclaration.VariantElementTitle, value); }
        }

        bool IVBMethodDeclaration.CanHoldHandlesClause
        {
            get { return false; }
        }

        HandlesClause IVBMethodDeclaration.HandlesClause
        {
            get { return null; }
            set { throw new NotSupportedException(); }
        }

        bool IVBMethodDeclaration.CanHoldImplementsClause
        {
            get { return false; }
        }

        ImplementsClause IVBMethodDeclaration.ImplementsClause
        {
            get { return null; }
            set { throw new NotSupportedException(); }
        }

        public AstToken AsKeyword
        {
            get { return GetChildByTitle(MethodDeclaration.AsKeywordTitle); }
            set { SetChildByTitle(MethodDeclaration.AsKeywordTitle, value); }
        }
    }
}
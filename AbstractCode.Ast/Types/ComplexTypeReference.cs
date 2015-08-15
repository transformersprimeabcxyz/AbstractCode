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

using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.Ast.Types
{
    public abstract class ComplexTypeReference : TypeReference 
    {
        private sealed class ComplexTypeReferenceWrapper : Symbols.TypeReference
        {
            private readonly ComplexTypeReference _reference;

            public ComplexTypeReferenceWrapper(ComplexTypeReference reference)
            {
                _reference = reference;
            }

            public override ResolveResult Resolve(IScope scope)
            {
                return ErrorResolveResult.Instance; // TODO
            }
        }

        private ComplexTypeReferenceWrapper _symbolsReference;

        protected ComplexTypeReference()
        {
        }

        protected ComplexTypeReference(TypeReference baseType)
        {
            BaseType = baseType;
        }

        public TypeReference BaseType
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public override string FullName
        {
            get { return BaseType.FullName; }
        }

        public override TypeReference GetElementType()
        {
            return BaseType.GetElementType();
        }

        public override Symbols.TypeReference GetSymbolsReference()
        {
            return _symbolsReference ?? (_symbolsReference = new ComplexTypeReferenceWrapper(this));
        }
    }
}

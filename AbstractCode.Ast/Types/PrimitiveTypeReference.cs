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
    public class PrimitiveTypeReference : TypeReference
    {
        
        private sealed class PrimitiveTypeReferenceWrapper : Symbols.TypeReference
        {
            private readonly PrimitiveTypeReference _reference;

            public PrimitiveTypeReferenceWrapper(PrimitiveTypeReference reference)
            {
                _reference = reference;
            }

            public override ResolveResult Resolve(IScope scope)
            {
                var systemNamespace = _reference.GetDeclaringScope().ResolveIdentifier("System") as NamespaceResolveResult;
                if (systemNamespace != null)
                {
                    foreach (var candidate in systemNamespace.ResolvedDefinitions)
                    {
                        var namespaceScope = candidate.GetScope();
                        var typeResult = namespaceScope?.ResolveIdentifier(_reference.PrimitiveType.ToString()) as MemberResolveResult;
                        if (typeResult != null && !typeResult.IsError)
                            return typeResult;
                    }
                }
                return new UnknownIdentifierResolveResult(_reference.Name);
            }
        }

        private PrimitiveTypeReferenceWrapper _symbolsReference;
        
        public PrimitiveTypeReference()
        {
        }

        public PrimitiveTypeReference(PrimitiveType type)
        {
            PrimitiveType = type;
        }

        public virtual PrimitiveType PrimitiveType
        {
            get;
            set;
        }

        public override string Name
        {
            get { return FullName; }
        }

        public override string FullName
        {
            get { return Identifier == null ? PrimitiveType.ToString() : Identifier.Name; }
        }

        public override Symbols.TypeReference GetSymbolsReference()
        {
            return _symbolsReference ?? (_symbolsReference = new PrimitiveTypeReferenceWrapper(this));
        }

        public override bool Match(AstNode other)
        {
            var reference = other as PrimitiveTypeReference;
            return reference != null
                   && PrimitiveType == reference.PrimitiveType;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitPrimitiveTypeReference(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitPrimitiveTypeReference(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitPrimitiveTypeReference(this, data);
        }

    }

    public enum PrimitiveType
    {
        Void,
        Object,
        Boolean,
        Byte,
        UInt16,
        UInt32,
        UInt64,
        UIntPtr,
        SByte,
        Int16,
        Int32,
        Int64,
        IntPtr,
        Single,
        Double,
        Decimal,
        Char,
        String,
    }
}

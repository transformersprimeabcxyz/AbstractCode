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
using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.Ast.Types
{
    public class SimpleTypeReference : TypeReference, ITypeArgumentProvider
    {
        private sealed class SimpleTypeReferenceWrapper : Symbols.TypeReference
        {
            private readonly SimpleTypeReference _reference;

            public SimpleTypeReferenceWrapper(SimpleTypeReference reference)
            {
                _reference = reference;
            }

            public override ResolveResult Resolve(IScope scope)
            {
                return scope.ResolveIdentifier(_reference.Name);
            }
        }

        private SimpleTypeReferenceWrapper _symbolsReference;

        public SimpleTypeReference()
        {
            TypeArguments = new AstNodeCollection<TypeReference>(this, AstNodeTitles.TypeArgument);
        }

        public SimpleTypeReference(Identifier identifier)
            : this()
        {
            base.Identifier = identifier;
        }

        public SimpleTypeReference(string identifier)
            : this(new Identifier(identifier))
        {
        }

        public override string FullName
        {
            get { return Identifier.Name; }
        }

        public AstNodeCollection<TypeReference> TypeArguments
        {
            get;
        }

        public override Symbols.TypeReference GetSymbolsReference()
        {
            return _symbolsReference ?? (_symbolsReference = new SimpleTypeReferenceWrapper(this));
        }

        public override string ToString()
        {
            return string.Format("{0} (Name = {1})", GetType().Name, Identifier.Name);
        }

        public override bool Match(AstNode other)
        {
            var reference = other as SimpleTypeReference;
            return reference != null
                   && Identifier.MatchOrNull(reference.Identifier)
                   && TypeArguments.Match(reference.TypeArguments);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitSimpleTypeReference(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitSimpleTypeReference(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitSimpleTypeReference(this, data);
        }

    }
}

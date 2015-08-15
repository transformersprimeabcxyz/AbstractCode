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
    public class MemberTypeReference : TypeReference, ITypeArgumentProvider
    {
        private sealed class MemberTypeReferenceWrapper : Symbols.TypeReference
        {
            private readonly MemberTypeReference _reference;

            public MemberTypeReferenceWrapper(MemberTypeReference reference)
            {
                _reference = reference;
            }

            public override ResolveResult Resolve(IScope scope)
            {
                var parentResult = _reference.Target.Resolve(scope);
                var actualResult = parentResult?.ScopeProvider?.GetScope().ResolveIdentifier(_reference.Identifier.Name);
                return actualResult ?? new UnknownIdentifierResolveResult(_reference.Name);
            }
        }

        private MemberTypeReferenceWrapper _symbolsReference;

        public MemberTypeReference()
        {
            TypeArguments = new AstNodeCollection<TypeReference>(this, AstNodeTitles.TypeArgument);
        }

        public MemberTypeReference(TypeReference target, Identifier identifier)
            : this()
        {
            Target = target;
            Identifier = identifier;
        }

        public TypeReference Target
        {
            get { return GetChildByTitle(AstNodeTitles.Type); }
            set { SetChildByTitle(AstNodeTitles.Type, value); }
        }

        public override string FullName
        {
            get { return string.Format("{0}.{1}", Target.FullName, Identifier.Name); }
        }

        public AstNodeCollection<TypeReference> TypeArguments
        {
            get;
        }

        public override Symbols.TypeReference GetSymbolsReference()
        {
            return _symbolsReference ?? (_symbolsReference = new MemberTypeReferenceWrapper(this));
        }

        public override string ToString()
        {
            return string.Format("{0} (Name = {1})", GetType().Name, Identifier.Name);
        }

        public override bool Match(AstNode other)
        {
            var reference = other as MemberTypeReference;
            return reference != null
                   && Target.MatchOrNull(reference.Target)
                   && Identifier.MatchOrNull(reference.Identifier)
                   && TypeArguments.Match(reference.TypeArguments);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitMemberTypeReference(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitMemberTypeReference(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitMemberTypeReference(this, data);
        }

    }
}

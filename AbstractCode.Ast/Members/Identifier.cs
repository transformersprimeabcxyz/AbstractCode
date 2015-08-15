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

using AbstractCode.Ast.Types;
using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;
using TypeReference = AbstractCode.Ast.Types.TypeReference;

namespace AbstractCode.Ast.Members
{
    public class Identifier : AstNode, IConvertibleToType, IConvertibleToIdentifier, IResolvable
    {
        public Identifier()
        {
        }

        public Identifier(string name)
            : this(name, TextRange.Empty)
        {
        }

        public Identifier(string name, TextRange textRange)
        {
            this.Name = name;
            this.Range = textRange;
        }

        public string Name
        {
            get;
        }

        public virtual Identifier CreateCopy()
        {
            return new Identifier(Name, Range);
        }

        public TypeReference ToTypeReference()
        {
            return new SimpleTypeReference(CreateCopy());
        }

        Identifier IConvertibleToIdentifier.ToIdentifier()
        {
            return CreateCopy();
        }

        public ResolveResult Resolve(IScope scope)
        {
            return scope != null ? scope.ResolveIdentifier(Name) : new UnknownIdentifierResolveResult(Name);
        }

        public override string ToString()
        {
            return string.Format("{0} (Name = {1})", GetType().Name, Name);
        }

        public override bool Match(AstNode other)
        {
            var identifier = other as Identifier;
            return identifier != null && identifier.Name == Name;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitIdentifier(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitIdentifier(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitIdentifier(this, data);
        }
    }
}

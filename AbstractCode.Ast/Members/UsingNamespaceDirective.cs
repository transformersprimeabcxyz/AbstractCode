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

namespace AbstractCode.Ast.Members
{
    public class UsingNamespaceDirective : UsingDirective
    {
        public UsingNamespaceDirective()
        {
        }

        public UsingNamespaceDirective(string @namespace)
            : this(new Identifier(@namespace))
        {
        }

        public UsingNamespaceDirective(Identifier namespaceIdentifier)
        {
            NamespaceIdentifier = namespaceIdentifier;
        }

        public Identifier NamespaceIdentifier
        {
            get { return GetChildByTitle(AstNodeTitles.Identifier); }
            set { SetChildByTitle(AstNodeTitles.Identifier, value); }
        }

        public override string ToString()
        {
            return string.Format("{0} (Namespace = {1})", GetType().Name, NamespaceIdentifier.Name);
        }

        public override bool Match(AstNode other)
        {
            var directive = other as UsingNamespaceDirective;
            return directive != null
                   && NamespaceIdentifier.MatchOrNull(directive.NamespaceIdentifier);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitUsingNamespaceDirective(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitUsingNamespaceDirective(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitUsingNamespaceDirective(this, data);
        }
    }
}

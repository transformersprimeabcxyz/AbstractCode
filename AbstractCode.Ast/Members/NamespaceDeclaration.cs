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
using AbstractCode.Symbols;

namespace AbstractCode.Ast.Members
{
    public class NamespaceDeclaration : AstNode, IUsingDeclarationProvider, IDefinitionProvider, IScopeProvider
    {
        private sealed class NamespaceDeclarationWrapper : NamespaceDefinition
        {
            private readonly NamespaceDeclaration _declaration;

            public NamespaceDeclarationWrapper(NamespaceDeclaration declaration)
            {
                _declaration = declaration;
            }

            public override AssemblyDefinition Assembly
            {
                get { return _declaration._parentCompilationUnit.Assembly; }
            }

            public override NamespaceDefinition Parent
            {
                get { return null; } // TODO
            }

            public override string Name
            {
                get { return _declaration.Identifier.Name; }
            }

            public override IEnumerable<NamespaceDefinition> GetNamespaceDefinitions()
            {
                foreach (var @namespace in _declaration.Namespaces)
                    yield return @namespace.GetDefinition();
            }

            public override IEnumerable<TypeDefinition> GetTypeDefinitions()
            {
                foreach (var type in _declaration.Types)
                    yield return type.GetDefinition();
            }

            public override IScope GetDeclaringScope()
            {
                return _declaration._parentCompilationUnit != null
                    ? _declaration._parentCompilationUnit.GetScope()
                    : base.GetDeclaringScope();
            }
        }

        private NamespaceDeclarationWrapper _definition;
        private CompilationUnit _parentCompilationUnit;

        public NamespaceDeclaration()
        {
            Namespaces = new AstNodeCollection<NamespaceDeclaration>(this, AstNodeTitles.NamespaceDeclaration);
            Types = new AstNodeCollection<TypeDeclaration>(this, AstNodeTitles.TypeDeclaration);
            UsingDirectives = new AstNodeCollection<UsingDirective>(this, AstNodeTitles.UsingDirective);
        }

        public NamespaceDeclaration(string identifier)
            : this(new Identifier(identifier))
        {
        }

        public NamespaceDeclaration(Identifier identifier)
            : this()
        {
            Identifier = identifier;
        }

        public AstToken Keyword
        {
            get { return GetChildByTitle(AstNodeTitles.Keyword); }
            set { SetChildByTitle(AstNodeTitles.Keyword, value); }
        }

        public AstNode StartScope
        {
            get { return GetChildByTitle(AstNodeTitles.StartScope); }
            set { SetChildByTitle(AstNodeTitles.StartScope, value); }
        }

        public Identifier Identifier
        {
            get { return GetChildByTitle(AstNodeTitles.Identifier); }
            set { SetChildByTitle(AstNodeTitles.Identifier, value); }
        }

        public AstNode EndScope
        {
            get { return GetChildByTitle(AstNodeTitles.EndScope); }
            set { SetChildByTitle(AstNodeTitles.EndScope, value); }
        }

        public AstNodeCollection<TypeDeclaration> Types
        {
            get;
        }

        public AstNodeCollection<NamespaceDeclaration> Namespaces 
        { 
            get; 
        }

        public AstNodeCollection<UsingDirective> UsingDirectives
        {
            get;
        }

        public override string ToString()
        {
            return string.Format("{0} (Namespace = {1})", GetType().Name, Identifier.Name);
        }

        public override bool Match(AstNode other)
        {
            var declaration = other as NamespaceDeclaration;
            return declaration != null
                   && declaration.Identifier.MatchOrNull(Identifier)
                   && declaration.UsingDirectives.Match(UsingDirectives)
                   && declaration.Namespaces.Match(Namespaces);
        }

        public IScope GetScope()
        {
            return GetDefinition().GetScope();
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitNamespaceDeclaration(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitNamespaceDeclaration(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitNamespaceDeclaration(this, data);
        }

        public NamespaceDefinition GetDefinition()
        {
            return _definition ?? (_definition = new NamespaceDeclarationWrapper(this));
        }

        IEnumerable<INamedDefinition> IDefinitionProvider.GetDefinitions()
        {
            yield return GetDefinition();
        }

        protected override void OnParentChanged()
        {
            _parentCompilationUnit = GetRoot() as CompilationUnit;
            base.OnParentChanged();
        }
    }
}
